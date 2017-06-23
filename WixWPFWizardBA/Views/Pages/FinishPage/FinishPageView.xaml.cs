namespace WixWPFWizardBA.Views.Pages.FinishPage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for FinishPageView.xaml
    /// </summary>
    public partial class FinishPageView : UserControl
    {
        public FinishPageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new FinishPageViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}