//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Views.Pages.ProgressPage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for ProgressPageView.xaml
    /// </summary>
    public partial class ProgressPageView : UserControl
    {
        public ProgressPageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new ProgressPageViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}