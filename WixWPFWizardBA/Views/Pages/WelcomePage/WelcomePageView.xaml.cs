namespace WixWPFWizardBA.Views.Pages.WelcomePage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for WelcomePageView.xaml
    /// </summary>
    public partial class WelcomePageView : UserControl
    {
        public WelcomePageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new WelcomePageViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}