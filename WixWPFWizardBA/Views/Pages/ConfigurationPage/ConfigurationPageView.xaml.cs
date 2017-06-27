//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Views.Pages.ConfigurationPage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for ConfigurationPageView.xaml
    /// </summary>
    public partial class ConfigurationPageView : UserControl
    {
        public ConfigurationPageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new ConfigurationPageViewModel(wizardViewModel);

            this.InitializeComponent();
        }
    }
}