namespace WixWPFWizardBA.Views.Pages.ConfigurationPage
{
    public sealed class ConfigurationPageViewModel : PageViewModel
    {
        public ConfigurationPageViewModel(WizardViewModel wizardViewModel)
            : base(wizardViewModel)
        {
            this.CanCancel = true;
            this.CanGoToPreviousPage = true;
            this.CanGoToNextPage = true;
        }
    }
}