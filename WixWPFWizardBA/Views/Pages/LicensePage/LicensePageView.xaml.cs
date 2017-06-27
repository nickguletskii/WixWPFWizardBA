//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
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