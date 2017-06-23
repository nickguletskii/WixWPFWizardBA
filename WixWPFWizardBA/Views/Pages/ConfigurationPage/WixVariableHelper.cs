namespace WixWPFWizardBA.Views.Pages.ConfigurationPage
{
    using System;

    public class WixVariableHelper : BootstrapperAwareViewModel
    {
        private readonly Func<string, string> _normaliser;
        private bool _isValueRetreived;
        private string _value;

        public WixVariableHelper(WixBootstrapper bootstrapper, string wixVariable,
            Func<string, string> normaliser = null)
            : base(bootstrapper)
        {
            this._normaliser = normaliser ?? (x => x);
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
                    this.Bootstrapper.Engine.StringVariables[this.WixVariable] = this._normaliser(this._value);
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

                this.Bootstrapper.Engine.StringVariables[this.WixVariable] = this._normaliser(this._value);
                return true;
            }
            return false;
        }
    }
}