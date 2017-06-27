//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Views.Pages.BootstrapperUpdateCheckPage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for BootstrapperUpdateCheckPageView.xaml
    /// </summary>
    public partial class BootstrapperUpdateCheckPageView : UserControl
    {
        public BootstrapperUpdateCheckPageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new BootstrapperUpdateCheckPageViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}