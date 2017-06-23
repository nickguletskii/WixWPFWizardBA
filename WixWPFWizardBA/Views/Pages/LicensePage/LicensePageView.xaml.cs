namespace WixWPFWizardBA.Views.Pages.LicensePage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for LicensePageView.xaml
    /// </summary>
    public partial class LicensePageView : UserControl
    {
        public LicensePageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new LicensePageViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}