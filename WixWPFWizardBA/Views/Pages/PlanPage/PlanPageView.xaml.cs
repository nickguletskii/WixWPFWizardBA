namespace WixWPFWizardBA.Views.Pages.PlanPage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for PlanPageView.xaml
    /// </summary>
    public partial class PlanPageView : UserControl
    {
        public PlanPageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new PlanPageViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}