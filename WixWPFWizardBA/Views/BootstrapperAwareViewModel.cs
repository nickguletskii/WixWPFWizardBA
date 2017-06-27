//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Views
{
    using System.ComponentModel;

    public class BootstrapperAwareViewModel : INotifyPropertyChanged
    {
        public BootstrapperAwareViewModel(WixBootstrapper bootstrapper)
        {
            this.Bootstrapper = bootstrapper;
        }

        public WixBootstrapper Bootstrapper { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string GetBootstrapperStringVariable(string variableName)
        {
            return this.Bootstrapper.Engine.StringVariables[variableName];
        }

        public void SetBootstrapperStringVariable(string variableName, string variableValue)
        {
            this.Bootstrapper.Engine.StringVariables[variableName] = variableValue;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}