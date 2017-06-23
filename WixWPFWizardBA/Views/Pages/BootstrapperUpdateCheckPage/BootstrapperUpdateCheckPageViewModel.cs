namespace WixWPFWizardBA.Views.Pages.BootstrapperUpdateCheckPage
{
    public class BootstrapperUpdateCheckPageViewModel : PageViewModel
    {
        public BootstrapperUpdateCheckPageViewModel(WizardViewModel wizardViewModel)
            : base(wizardViewModel)
        {
            this.CanCancel = false;
            this.CanGoToPreviousPage = false;
            this.CanGoToNextPage = false;
        }
    }
}