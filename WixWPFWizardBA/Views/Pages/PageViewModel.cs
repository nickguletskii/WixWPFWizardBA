//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Views.Pages
{
    using System.Windows.Input;

    public abstract class PageViewModel : BootstrapperAwareViewModel
    {
        private string _backButtonText;
        private bool _canCancel;
        private string _cancelButtonText;
        private bool _canGoToNextPage;
        private bool _canGoToPreviousPage;
        private string _nextButtonText;

        protected PageViewModel(WizardViewModel wizardViewModel)
            : base(wizardViewModel.Bootstrapper)
        {
            this.WizardViewModel = wizardViewModel;
            this.CancelCommand = new SimpleCommand(_ => this.WizardViewModel.RequestCancellation(),
                _ => this.CanCancel && !wizardViewModel.CancelButtonPressed && !wizardViewModel.ShouldCancel);
            this.NextPageCommand = new SimpleCommand(_ => this.BeginNextPhase(), _ => this.CanGoToNextPage);
            this.PreviousPageCommand = new SimpleCommand(_ => this.GoToPreviousPage(), _ => this.CanGoToPreviousPage);

            this.NextButtonText =
                this.WizardViewModel.NextPageType == PageType.ProgressPage
                    ? Localisation.InstallButtonText
                    : Localisation.Wizard_NextButtonText;

            this.BackButtonText = Localisation.Wizard_BackButtonText;
            this.CancelButtonText = Localisation.Wizard_CancelButtonText;
        }

        public WizardViewModel WizardViewModel { get; }

        public virtual ICommand CancelCommand { get; }
        public virtual ICommand NextPageCommand { get; }
        public virtual ICommand PreviousPageCommand { get; }


        public string CancelButtonText
        {
            get => this._cancelButtonText;
            set
            {
                if (this._cancelButtonText != value)
                {
                    this._cancelButtonText = value;
                    this.OnPropertyChanged(nameof(this.CancelButtonText));
                }
            }
        }

        public string NextButtonText
        {
            get => this._nextButtonText;
            set
            {
                if (this._nextButtonText != value)
                {
                    this._nextButtonText = value;
                    this.OnPropertyChanged(nameof(this.NextButtonText));
                }
            }
        }

        public string BackButtonText
        {
            get => this._backButtonText;
            set
            {
                if (this._backButtonText != value)
                {
                    this._backButtonText = value;
                    this.OnPropertyChanged(nameof(this.BackButtonText));
                }
            }
        }

        public bool CanCancel
        {
            get => this._canCancel;
            set
            {
                if (this._canCancel != value)
                {
                    this._canCancel = value;
                    this.OnPropertyChanged(nameof(this.CanCancel));
                }
            }
        }

        public bool CanGoToPreviousPage
        {
            get => this._canGoToPreviousPage;
            set
            {
                if (this._canGoToPreviousPage != value)
                {
                    this._canGoToPreviousPage = value;
                    this.OnPropertyChanged(nameof(this.CanGoToPreviousPage));
                }
            }
        }

        public bool CanGoToNextPage
        {
            get => this._canGoToNextPage;
            set
            {
                if (this._canGoToNextPage != value)
                {
                    this._canGoToNextPage = value;
                    this.OnPropertyChanged(nameof(this.CanGoToNextPage));
                }
            }
        }

        public void BeginNextPhase()
        {
            switch (this.WizardViewModel.NextPageType)
            {
                case PageType.PlanPage:
                    this.WizardViewModel.BeginPlanningAction(this.WizardViewModel.LaunchAction);
                    return;
                case PageType.ProgressPage:
                    this.WizardViewModel.ApplyAction();
                    return;
            }
            this.WizardViewModel.GoToPage(this.WizardViewModel.NextPageType);
        }

        private void GoToPreviousPage()
        {
            if (this.WizardViewModel.PreviousPageType == PageType.None
                || this.WizardViewModel.PreviousPageType == PageType.ProgressPage
                || this.WizardViewModel.PreviousPageType == PageType.PlanPage)
            {
                this.WizardViewModel.RequestCancellation();
            }
            this.WizardViewModel.GoToPage(this.WizardViewModel.PreviousPageType);
        }
    }
}