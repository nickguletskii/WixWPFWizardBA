namespace WixWPFWizardBA.Views
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Threading;
    using Common;
    using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
    using Pages.AdvancedConfigurationPage;
    using Pages.BootstrapperUpdateAvailablePage;
    using Pages.BootstrapperUpdateCheckPage;
    using Pages.ConfigurationPage;
    using Pages.FinishErrorPage;
    using Pages.FinishPage;
    using Pages.LayoutConfigurationPage;
    using Pages.LicensePage;
    using Pages.PlanPage;
    using Pages.ProgressPage;
    using Pages.WelcomePage;

    public sealed class WizardViewModel : BootstrapperManager, INotifyPropertyChanged
    {
        private PageType _currentPageType = PageType.None;
        private UIElement _currentPageView;

        public WizardViewModel(WixBootstrapper bootstrapper) : base(bootstrapper)
        {
            this.PackageCombinationConfiguration = new PackageCombinationConfiguration(bootstrapper);
            this.PackageInstallationStrategy = new PackageInstallationStrategy(this.PackageCombinationConfiguration);
        }

        public PackageCombinationConfiguration PackageCombinationConfiguration { get; }

        public PageType CurrentPageType
        {
            get => this._currentPageType;
            set
            {
                this._currentPageType = value;
                this.OnPropertyChanged(nameof(this.CurrentPageType));
            }
        }

        public PageType NextPageType
        {
            get
            {
                if (this.GetNextPageOrDefault(this.CurrentPageType) != null)
                {
                    return (PageType) this.GetNextPageOrDefault(this.CurrentPageType);
                }

                return PageType.None;
            }
        }

        public PageType PreviousPageType
        {
            get
            {
                if (this.GetPreviousPageOrDefault(this.CurrentPageType) != null)
                {
                    return (PageType) this.GetPreviousPageOrDefault(this.CurrentPageType);
                }

                return PageType.None;
            }
        }

        public UIElement CurrentPageView
        {
            get => this._currentPageView;
            set
            {
                this._currentPageView = value;
                this.OnPropertyChanged(nameof(this.CurrentPageView));
            }
        }


        public override IPackageInstallationStrategy PackageInstallationStrategy { get; }

        protected override void BeginUpdate()
        {
            this.GoToPage(PageType.BootstrapperUpdateCheckPage);
        }

        protected override void OnBootstrapperShouldGoToFirstPage()
        {
            this.GoToFirstPage();
        }

        protected override void ApplyBegin()
        {
            this.CurrentPageType = PageType.ProgressPage;

            this.Log(LogLevel.Standard, $"Building progress page view with {this.LaunchAction}");

            FrameworkElement view = null;
            view = new ProgressPageView(this);
            this.CurrentPageView = view;
        }

        protected override void PlanBegin()
        {
            this.GoToPage(PageType.PlanPage);
        }

        private PageType? GetNextPageOrDefault(PageType currentPageType)
        {
            switch (currentPageType)
            {
                case PageType.WelcomePage:
                    switch (this.LaunchAction)
                    {
                        case LaunchAction.Cache:
                            return PageType.ConfigurationPage;
                        case LaunchAction.Install:
                        case LaunchAction.Modify:
                            return PageType.LicensePage;
                        case LaunchAction.Uninstall:
                        case LaunchAction.Repair:
                            return PageType.PlanPage;
                        case LaunchAction.Layout:
                            return PageType.LayoutConfigurationPage;
                        case LaunchAction.Unknown:
                            return null;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(this.LaunchAction), this.LaunchAction, null);
                    }
                case PageType.LicensePage:
                    switch (this.LaunchAction)
                    {
                        case LaunchAction.Install:
                            return this.VersionStatus == VersionStatus.OlderInstalled
                                ? PageType.PlanPage
                                : PageType.ConfigurationPage;
                        case LaunchAction.Modify:
                            return PageType.ConfigurationPage;
                        case LaunchAction.Cache:
                            return PageType.PlanPage;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(this.LaunchAction), this.LaunchAction, null);
                    }
                case PageType.LayoutConfigurationPage:
                    switch (this.LaunchAction)
                    {
                        case LaunchAction.Layout:
                            return PageType.PlanPage;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(this.LaunchAction), this.LaunchAction, null);
                    }
                case PageType.AdvancedConfigurationPage:
                    return PageType.PlanPage;
                case PageType.ConfigurationPage:
                    switch (this.LaunchAction)
                    {
                        case LaunchAction.Install:
                            return PageType.AdvancedConfigurationPage;
                        case LaunchAction.Modify:
                        case LaunchAction.Cache:
                            return PageType.PlanPage;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(this.LaunchAction), this.LaunchAction, null);
                    }
                case PageType.BootstrapperUpdateAvailablePage:
                    return PageType.WelcomePage;
                default:
                    return null;
            }
        }

        private PageType? GetPreviousPageOrDefault(PageType currentPageType)
        {
            switch (currentPageType)
            {
                case PageType.WelcomePage:
                    return this.BootstrapperUpdateState == UpdateState.Available
                        ? (PageType?) PageType.BootstrapperUpdateAvailablePage
                        : null;
                case PageType.LicensePage:
                    return PageType.WelcomePage;
                case PageType.AdvancedConfigurationPage:
                    return PageType.ConfigurationPage;
                case PageType.LayoutConfigurationPage:
                    return PageType.WelcomePage;
                case PageType.ConfigurationPage:
                    switch (this.LaunchAction)
                    {
                        case LaunchAction.Install:
                        case LaunchAction.Modify:
                            return PageType.LicensePage;
                        case LaunchAction.Cache:
                            return PageType.WelcomePage;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(this.LaunchAction), this.LaunchAction, null);
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentPageType), currentPageType, null);
            }
        }

        public void GoToFirstPage()
        {
            this.GoToPage(this.BootstrapperUpdateState == UpdateState.Available
                ? PageType.BootstrapperUpdateAvailablePage
                : PageType.WelcomePage);
        }

        public void GoToPage(PageType pageType)
        {
            if (pageType == this.CurrentPageType)
                return;

            this.Log(LogLevel.Standard, $"Switching to {pageType} from {this.CurrentPageType}");
            this.CurrentPageType = pageType;

            var view = this.CreateNewView(pageType);
            this.CurrentPageView = view;
        }

        private UIElement CreateNewView(PageType pageType)
        {
            switch (pageType)
            {
                case PageType.WelcomePage:
                    return new WelcomePageView(this);
                case PageType.LicensePage:
                    return new LicensePageView(this);
                case PageType.AdvancedConfigurationPage:
                    return new AdvancedConfigurationPageView(this);
                case PageType.LayoutConfigurationPage:
                    return new LayoutConfigurationPageView(this);
                case PageType.ConfigurationPage:
                    return new ConfigurationPageView(this);
                case PageType.PlanPage:
                    return new PlanPageView(this);
                case PageType.FinishPage:
                    return new FinishPageView(this);
                case PageType.FinishErrorPage:
                    return new FinishErrorPageView(this);
                case PageType.BootstrapperUpdateCheckPage:
                    return new BootstrapperUpdateCheckPageView(this);
                case PageType.BootstrapperUpdateAvailablePage:
                    return new BootstrapperUpdateAvailablePageView(this);
            }
            throw new ArgumentOutOfRangeException(nameof(pageType));
        }

        protected override void TransitionToFinishPhase()
        {
            this.GoToPage(this.Status < 0 ? PageType.FinishErrorPage : PageType.FinishPage);
        }

        public void GoToNextPage()
        {
            this.GoToPage(this.NextPageType);
        }

        protected override MessageBoxResult ShowCancelDialog()
        {
            if (this.IsInteractive)
            {
                var result = MessageBoxResult.No;
                WixBootstrapper.BootstrapperDispatcher.Invoke(DispatcherPriority.Background,
                    new Action(() =>
                    {
                        result = MessageBox.Show(WixBootstrapper.RootView,
                            string.Format(Localisation.CancelDialogBody, this.Bootstrapper.BundleName),
                            string.Format(Localisation.CancelDialogTitle, this.Bootstrapper.BundleName),
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);
                    }));
                return result;
            }
            return MessageBoxResult.Yes;
        }
    }
}