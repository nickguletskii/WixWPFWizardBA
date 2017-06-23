namespace WixWPFWizardBA.Views.Pages.BootstrapperUpdateAvailablePage
{
    using System.Diagnostics;

    public class BootstrapperUpdateAvailablePageViewModel : PageViewModel
    {
        public BootstrapperUpdateAvailablePageViewModel(WizardViewModel wizardViewModel)
            : base(wizardViewModel)
        {
            this.CanCancel = true;
            this.CanGoToPreviousPage = false;
            this.CanGoToNextPage = true;

            this.UpdateCommand = new SimpleCommand(_ => { this.WizardViewModel.InitiateBootstrapperUpdate(); },
                _ => true);
            this.DownloadUpdateCommand = new SimpleCommand(
                _ => { Process.Start(this.WizardViewModel.BootstrapperUpdateLocation); }, _ => true);
        }

        public SimpleCommand UpdateCommand { get; }
        public SimpleCommand DownloadUpdateCommand { get; }
    }
}