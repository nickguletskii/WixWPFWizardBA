namespace WixWPFWizardBA.Views
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Interop;
    using Common;
    using Pages;

    /// <summary>
    ///     Interaction logic for WelcomePageView.xaml
    /// </summary>
    public partial class WizardWindow
    {
        public WizardWindow()
        {
            this.DataContext = new WizardViewModel(null);
            this.InitializeComponent();
        }

        public WizardWindow(WixBootstrapper wixBootstrapper)
        {
            this.DataContext = new WizardViewModel(wixBootstrapper)
            {
                WindowHandle = new WindowInteropHelper(this).Handle
            };
            this.InitializeComponent();
        }

        public WizardViewModel ViewModel => (WizardViewModel) this.DataContext;

        private void WizardWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (this.ViewModel.BurnInstallationState != BurnInstallationState.Applying &&
                (this.ViewModel.CurrentPageType == PageType.FinishPage
                 || this.ViewModel.CurrentPageType == PageType.FinishErrorPage))
            {
                return;
            }
            var vm = (PageViewModel) ((FrameworkElement) this.ViewModel.CurrentPageView).DataContext;
            if (this.ViewModel.ShouldCancel)
            {
                MessageBox.Show(this,
                    string.Format(Localisation.CancelAlreadyInProgressMessageBoxBody, this.ViewModel.Bootstrapper.BundleName),
                    string.Format(Localisation.CancelAlreadyInProgressMessageBoxTitle, this.ViewModel.Bootstrapper.BundleName),
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Cancel = true;
                return;
            }

            if (vm.CanCancel && !this.ViewModel.CancelButtonPressed)
                this.ViewModel.RequestCancellation();
            else
                MessageBox.Show(this,
                    string.Format(Localisation.CancelUnavailableMessageBoxBody, this.ViewModel.Bootstrapper.BundleName),
                    string.Format(Localisation.CancelUnavailableMessageBoxTitle, this.ViewModel.Bootstrapper.BundleName),
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Cancel = true;
        }
    }
}