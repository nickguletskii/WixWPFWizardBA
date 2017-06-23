namespace WixWPFWizardBA.Views.Pages.FinishErrorPage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for FinishErrorPage.xaml
    /// </summary>
    public partial class FinishErrorPageView : UserControl
    {
        public FinishErrorPageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new FinishErrorPageViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}