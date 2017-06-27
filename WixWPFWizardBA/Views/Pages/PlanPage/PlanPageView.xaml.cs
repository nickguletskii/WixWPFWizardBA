//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Views.Pages.PlanPage
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for PlanPageView.xaml
    /// </summary>
    public partial class PlanPageView : UserControl
    {
        public PlanPageView(WizardViewModel wizardViewModel)
        {
            this.DataContext = new PlanPageViewModel(wizardViewModel);
            this.InitializeComponent();
        }
    }
}