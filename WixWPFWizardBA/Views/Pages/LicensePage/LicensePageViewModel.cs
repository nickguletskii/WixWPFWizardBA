namespace WixWPFWizardBA.Views.Pages.LicensePage
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Documents;

    public class LicensePageViewModel : PageViewModel
    {
        private bool _isLicenseAccepted;
        private FlowDocument _licenseDocument;

        public LicensePageViewModel(WizardViewModel wizardViewModel) : base(wizardViewModel)
        {
            using (
                var stream =
                    Assembly.GetExecutingAssembly().GetManifestResourceStream($"{nameof(WixWPFWizardBA)}.License.rtf"))
            {
                var flowDocument = new FlowDocument();
                var textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
                textRange.Load(stream, DataFormats.Rtf);
                this.LicenseDocument = flowDocument;
            }
            this.CanCancel = true;
            this.CanGoToPreviousPage = true;
            this.CanGoToNextPage = false;
        }


        public FlowDocument LicenseDocument
        {
            get => this._licenseDocument;
            set
            {
                if (this._licenseDocument != value)
                {
                    this._licenseDocument = value;
                    this.OnPropertyChanged(nameof(this.LicenseDocument));
                }
            }
        }

        public bool IsLicenseAccepted
        {
            get => this._isLicenseAccepted;
            set
            {
                if (this._isLicenseAccepted != value)
                {
                    this._isLicenseAccepted = value;
                    this.OnPropertyChanged(nameof(this.IsLicenseAccepted));
                    this.CanGoToNextPage = value;
                }
            }
        }
    }
}