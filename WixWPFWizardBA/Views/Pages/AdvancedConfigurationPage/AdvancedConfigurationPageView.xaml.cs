namespace WixWPFWizardBA.Views.Pages.AdvancedConfigurationPage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for AdvancedConfigurationPageView.xaml
    /// </summary>
    public partial class AdvancedConfigurationPageView : UserControl
    {
        public AdvancedConfigurationPageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new AdvancedConfigurationViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}