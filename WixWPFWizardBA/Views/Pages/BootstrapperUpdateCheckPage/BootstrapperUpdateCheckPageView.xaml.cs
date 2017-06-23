namespace WixWPFWizardBA.Views.Pages.BootstrapperUpdateCheckPage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for BootstrapperUpdateCheckPageView.xaml
    /// </summary>
    public partial class BootstrapperUpdateCheckPageView : UserControl
    {
        public BootstrapperUpdateCheckPageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new BootstrapperUpdateCheckPageViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}