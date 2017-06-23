namespace WixWPFWizardBA.Views.Pages.FinishErrorPage
{
    using System.Windows.Input;

    public class FinishErrorPageViewModel : PageViewModel
    {
        public FinishErrorPageViewModel(WizardViewModel wizardViewModel) : base(wizardViewModel)
        {
            this.NextButtonText = Localisation.FinishPage_ExitButtonText;
            this.NextPageCommand = new SimpleCommand(_ => { this.Bootstrapper.Engine.Quit(wizardViewModel.Status); },
                _ => true);
            this.CanCancel = false;
            this.CanGoToPreviousPage = false;
            this.CanGoToNextPage = true;
        }

        public string ErrorTitle
        {
            get
            {
                switch ((uint) this.WizardViewModel.Status)
                {
                    case 0x80070642u:
                        return Localisation.FinishErrorPage_FinishErrorCanceled;
                    case 0x80072ee7u:
                        return Localisation.FinishErrorPage_FinishErrorConnectionError;
                    case 0x80072EFDu:
                        return Localisation.FinishErrorPage_FinishErrorCouldntDownloadInstallPackages;
                    case 0x80070002u:
                        return Localisation.FinishErrorPage_FinishErrorCouldntRetrieveInstallPackages;
                    case 0x84BE0BC2u:
                        return Localisation.FinishErrorPage_FinishErrorSqlServerRebootPending;
                    default:
                        return string.Format(Localisation.FinishErrorPage_FinishErrorUnknown,
                            this.WizardViewModel.Status);
                }
            }
        }

        public override ICommand NextPageCommand { get; }
    }
}