namespace WixWPFWizardBA.Views.Pages.ProgressPage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for ProgressPageView.xaml
    /// </summary>
    public partial class ProgressPageView : UserControl
    {
        public ProgressPageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new ProgressPageViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}