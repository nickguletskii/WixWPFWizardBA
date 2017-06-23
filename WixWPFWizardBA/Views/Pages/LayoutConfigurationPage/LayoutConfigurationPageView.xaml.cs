namespace WixWPFWizardBA.Views.Pages.LayoutConfigurationPage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for LayoutConfigurationPageView.xaml
    /// </summary>
    public partial class LayoutConfigurationPageView : UserControl
    {
        public LayoutConfigurationPageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new LayoutConfigurationViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}