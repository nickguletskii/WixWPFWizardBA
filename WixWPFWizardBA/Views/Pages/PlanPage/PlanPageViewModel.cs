namespace WixWPFWizardBA.Views.Pages.PlanPage
{
    public class PlanPageViewModel : PageViewModel
    {
        public PlanPageViewModel(WizardViewModel wizardViewModel)
            : base(wizardViewModel)
        {
            this.CanCancel = true;
            this.CanGoToPreviousPage = false;
            this.CanGoToNextPage = false;
        }
    }
}