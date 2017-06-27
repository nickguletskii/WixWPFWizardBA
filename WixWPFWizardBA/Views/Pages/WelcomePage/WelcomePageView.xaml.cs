//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Views.Pages.WelcomePage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for WelcomePageView.xaml
    /// </summary>
    public partial class WelcomePageView : UserControl
    {
        public WelcomePageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new WelcomePageViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}