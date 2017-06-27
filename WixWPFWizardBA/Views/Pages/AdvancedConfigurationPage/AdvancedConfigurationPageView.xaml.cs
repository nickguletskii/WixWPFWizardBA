//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Views.Pages.AdvancedConfigurationPage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for AdvancedConfigurationPageView.xaml
    /// </summary>
    public partial class AdvancedConfigurationPageView : UserControl
    {
        public AdvancedConfigurationPageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new AdvancedConfigurationViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}