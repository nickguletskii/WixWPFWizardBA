//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Views.Pages.BootstrapperUpdateAvailablePage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for BootstrapperUpdateAvailablePageView.xaml
    /// </summary>
    public partial class BootstrapperUpdateAvailablePageView : UserControl
    {
        public BootstrapperUpdateAvailablePageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new BootstrapperUpdateAvailablePageViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}