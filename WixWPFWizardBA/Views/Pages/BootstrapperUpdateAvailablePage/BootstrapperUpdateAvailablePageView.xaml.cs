namespace WixWPFWizardBA.Views.Pages.BootstrapperUpdateAvailablePage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for BootstrapperUpdateAvailablePageView.xaml
    /// </summary>
    public partial class BootstrapperUpdateAvailablePageView : UserControl
    {
        public BootstrapperUpdateAvailablePageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new BootstrapperUpdateAvailablePageViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}