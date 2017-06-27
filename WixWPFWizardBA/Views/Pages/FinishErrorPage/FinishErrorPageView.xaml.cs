//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Views.Pages.FinishErrorPage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for FinishErrorPage.xaml
    /// </summary>
    public partial class FinishErrorPageView : UserControl
    {
        public FinishErrorPageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new FinishErrorPageViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}