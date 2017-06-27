//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Views.Pages.FinishPage
{
    using System.Windows.Input;

    public class FinishPageViewModel : PageViewModel
    {
        public FinishPageViewModel(WizardViewModel wizardViewModel) : base(wizardViewModel)
        {
            var restartRequired = wizardViewModel.RestartRequired;
            this.BackButtonText = Localisation.FinishPage_ExitButtonText;
            this.NextButtonText = restartRequired
                ? Localisation.FinishPage_RebootButtonText
                : Localisation.FinishPage_ExitButtonText;
            this.NextPageCommand = new SimpleCommand(_ =>
            {
                if (restartRequired)
                {
                    wizardViewModel.RestartConfirmed = true;
                    wizardViewModel.Bootstrapper.Engine.Quit(wizardViewModel.Status);
                    return;
                }
                this.Bootstrapper.Engine.Quit(wizardViewModel.Status);
            }, _ => true);
            this.PreviousPageCommand = new SimpleCommand(
                _ => { this.Bootstrapper.Engine.Quit(wizardViewModel.Status); }, _ => restartRequired);
            this.CanCancel = false;
            this.CanGoToPreviousPage = restartRequired;
            this.CanGoToNextPage = true;
        }

        public override ICommand NextPageCommand { get; }
        public override ICommand PreviousPageCommand { get; }
    }
}