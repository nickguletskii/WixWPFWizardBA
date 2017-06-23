namespace WixWPFWizardBA.Views.Pages.ConfigurationPage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for ConfigurationPageView.xaml
    /// </summary>
    public partial class ConfigurationPageView : UserControl
    {
        public ConfigurationPageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new ConfigurationPageViewModel(wizardViewModel);

            this.InitializeComponent();
        }
    }
}