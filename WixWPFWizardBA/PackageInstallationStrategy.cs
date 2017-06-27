//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA
{
    using Common;
    using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

    public class PackageInstallationStrategy : PackageInstallationStrategyBase<PackageCombinationConfiguration,
        InstallationType>
    {
        public PackageInstallationStrategy(PackageCombinationConfiguration packageCombinationConfiguration) : base(
            PackageConfiguration.PackageList, packageCombinationConfiguration, packageCombinationConfiguration)
        {
            this.PackageCombinationConfiguration = packageCombinationConfiguration;
        }

        public PackageCombinationConfiguration PackageCombinationConfiguration { get; }

        public override FeatureState? PlanMsiFeature(LaunchAction launchAction, string packageId, string featureId)
        {
            if (packageId != PackageConfiguration.WixWpfWizardBaDemoMsi)
                return null;
            var installationType = this.PackageCombinationConfiguration.InstallationType;
            if (featureId == PackageConfiguration.ServerToolsFeature &&
                installationType == InstallationType.Client)
            {
                return FeatureState.Absent;
            }
            return FeatureState.Local;
        }

        public override void DetectAdditionalInformation()
        {
            this.PackageCombinationConfiguration.InstallationType =
                this.PackageCombinationConfiguration.CheckSqlServer64BitInstanceExists() ||
                this.PackageCombinationConfiguration.CheckSqlServer32BitInstanceInstanceExists()
                    ? InstallationType.MasterServer
                    : InstallationType.Client;
        }
    }
}