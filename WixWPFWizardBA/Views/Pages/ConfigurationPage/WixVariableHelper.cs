namespace WixWPFWizardBA.Views.Pages.ConfigurationPage
{
    public class WixVariableHelper : BootstrapperAwareViewModel
    {
        private bool _isValueRetreived;
        private string _value;

        public WixVariableHelper(WixBootstrapper bootstrapper, string wixVariable)
            : base(bootstrapper)
        {
            this.WixVariable = wixVariable;
        }

        private string WixVariable { get; }

        public string Get()
        {
            if (!this._isValueRetreived)
            {
                this._value = this.Bootstrapper.Engine.StringVariables[this.WixVariable];

                if (!string.IsNullOrEmpty(this._value))
                {
                    this.Bootstrapper.Engine.StringVariables[this.WixVariable] = this._value;
                }

                this._isValueRetreived = true;
            }

            return this._value;
        }

        public bool Set(string value)
        {
            if (this._value != value)
            {
                this._value = value;

                this.Bootstrapper.Engine.StringVariables[this.WixVariable] = this._value;
                return true;
            }
            return false;
        }
    }
}