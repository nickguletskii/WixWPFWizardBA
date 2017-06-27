//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Views.Pages.WelcomePage
{
    using Common;
    using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

    public class WelcomePageViewModel : PageViewModel
    {
        public WelcomePageViewModel(WizardViewModel wizardViewModel)
            : base(wizardViewModel)
        {
            this.CanCancel = true;
            this.CanGoToPreviousPage = false;
            this.CanGoToNextPage = false;
            this.LaunchInstallCommand = new SimpleCommand(_ =>
                {
                    wizardViewModel.LaunchAction = LaunchAction.Install;
                    this.BeginNextPhase();
                },
                _ => wizardViewModel.IsInstalled == false &&
                     wizardViewModel.VersionStatus != VersionStatus.OlderInstalled && wizardViewModel.VersionStatus !=
                     VersionStatus.NewerAlreadyInstalled);
            this.LaunchUpdateCommand = new SimpleCommand(_ =>
                {
                    wizardViewModel.LaunchAction = LaunchAction.Install;
                    this.BeginNextPhase();
                },
                _ => wizardViewModel.VersionStatus == VersionStatus.OlderInstalled);
            this.LaunchLayoutCommand = new SimpleCommand(_ =>
                {
                    wizardViewModel.LaunchAction = LaunchAction.Layout;
                    this.BeginNextPhase();
                },
                _ => true);
            this.LaunchModifyCommand = new SimpleCommand(_ =>
                {
                    wizardViewModel.LaunchAction = LaunchAction.Modify;
                    this.BeginNextPhase();
                },
                _ => wizardViewModel.IsInstalled == true);
            this.LaunchRepairCommand = new SimpleCommand(_ =>
                {
                    wizardViewModel.LaunchAction = LaunchAction.Repair;
                    this.BeginNextPhase();
                },
                _ => wizardViewModel.IsInstalled == true);
            this.LaunchUninstallCommand = new SimpleCommand(_ =>
                {
                    wizardViewModel.LaunchAction = LaunchAction.Uninstall;
                    this.BeginNextPhase();
                },
                _ => wizardViewModel.IsInstalled == true);
        }

        public SimpleCommand LaunchUninstallCommand { get; }

        public SimpleCommand LaunchRepairCommand { get; }

        public SimpleCommand LaunchModifyCommand { get; }

        public SimpleCommand LaunchLayoutCommand { get; }

        public SimpleCommand LaunchInstallCommand { get; }

        public SimpleCommand LaunchUpdateCommand { get; }
    }
}