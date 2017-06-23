namespace WixWPFWizardBA
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Threading;
    using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
    using Views;

    public class WixBootstrapper : BootstrapperApplication
    {
        public static Dispatcher BootstrapperDispatcher { get; private set; }

        public static WizardWindow RootView { get; set; }

        public string BundleName => this.Engine.StringVariables["WixBundleName"];

        protected override void Run()
        {
            var code = int.Parse(this.Engine.FormatString("[SystemLanguageID]"));
            var cultureInfo = CultureInfo.GetCultureInfo(code);

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Localisation.Culture = cultureInfo;

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(cultureInfo.IetfLanguageTag)));
            try
            {
                this.Engine.CloseSplashScreen();

                var rebootPending = this.Engine.StringVariables["RebootPending"];
                if (!string.IsNullOrEmpty(rebootPending) && rebootPending != "0")
                {
                    MessageBox.Show(
                        string.Format(Localisation.WixBootstrapper_RestartPendingDialogBody, this.BundleName),
                        string.Format(Localisation.WixBootstrapper_RestartPendingDialogTitle, this.BundleName),
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    this.Engine.Quit(3010);
                }

                this.Engine.Log(LogLevel.Verbose, "Launching Burn frontend");
                BootstrapperDispatcher = Dispatcher.CurrentDispatcher;
                AppDomain.CurrentDomain.UnhandledException +=
                    (sender, args) =>
                    {
                        this.Engine.Log(LogLevel.Error, $"Critical bootstrapper exception: {args.ExceptionObject}");
                    };
                BootstrapperDispatcher.UnhandledException +=
                    (sender, args) =>
                    {
                        this.Engine.Log(LogLevel.Error, $"Critical bootstrapper exception: {args.Exception}");
                    };

                RootView = new WizardWindow(this);
                RootView.Closed += (sender, args) => BootstrapperDispatcher.InvokeShutdown();

                this.Engine.Detect();

                foreach (var commandLineArg in this.Command.GetCommandLineArgs())
                {
                    if (commandLineArg.StartsWith("InstallationType=", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var param = commandLineArg.Split(new[] {'='}, 2);
                        RootView.ViewModel.PackageCombinationConfiguration.InstallationType =
                            (InstallationType) Enum.Parse(typeof(InstallationType), param[1]);
                    }
                }
                if (this.Command.Display == Display.Passive || this.Command.Display == Display.Full)
                {
                    RootView.Show();
                    Dispatcher.Run();
                }

                this.Engine.Quit(RootView.ViewModel.Status);
            }
            catch (Exception e)
            {
                this.Engine.Log(LogLevel.Error, $"Critical bootstrapper exception: {e}");
                throw e;
            }
        }
    }
}