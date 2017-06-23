namespace WixWPFWizardBA.Views.Pages.ProgressPage
{
    using System.ComponentModel;

    public class ProgressEntry : INotifyPropertyChanged
    {
        private ActionType _actionType;
        private string _description;
        private string _packageId;
        private int _progress;

        public int Progress
        {
            get => this._progress;
            set
            {
                if (this._progress != value)
                {
                    this._progress = value;
                    this.OnPropertyChanged(nameof(this.Progress));
                }
            }
        }

        public ActionType ActionType
        {
            get => this._actionType;
            set
            {
                if (this._actionType != value)
                {
                    this._actionType = value;
                    this.OnPropertyChanged(nameof(this.ActionType));
                }
            }
        }

        public string Description
        {
            get => this._description;
            set
            {
                if (this._description != value)
                {
                    this._description = value;
                    this.OnPropertyChanged(nameof(this.Description));
                }
            }
        }

        public string PackageId
        {
            get => this._packageId;
            set
            {
                if (this._packageId != value)
                {
                    this._packageId = value;
                    this.OnPropertyChanged(nameof(this.PackageId));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}