//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA
{
    using System.Collections.Generic;
    using Common;

    public static class PackageConfiguration
    {
        public const string Sql2014ExpressPackage86Id = "Sql2014Express_x86";
        public const string Sql2014ExpressUpgradePackage86Id = "Sql2014ExpressUpgrade_x86";
        public const string Sql2014ExpressPatchPackage86Id = "Sql2014ExpressPatch_x86";
        public const string Sql2014ExpressPackage64Id = "Sql2014Express_x64";
        public const string Sql2014ExpressUpgradePackage64Id = "Sql2014ExpressUpgrade_x64";
        public const string Sql2014ExpressPatchPackage64Id = "Sql2014ExpressPatch_x64";
        public const string Netfx462RedistPackageId = "NetFx462Redist";
        public const string Netfx35RedistPackageId = "BANetFx35Redist";
        public const string WixWpfWizardBaDemoMsi = "WixWPFWizardBA.DemoMsi";
        public const string ServerToolsFeature = "ServerToolsFeature";

        public static IList<Package<PackageCombinationConfiguration, InstallationType>> PackageList { get; } =
            new List<Package<PackageCombinationConfiguration, InstallationType>>
            {
                new Package<PackageCombinationConfiguration, InstallationType>
                {
                    PackageId = WixWpfWizardBaDemoMsi,
                    DisplayName = Localisation.PackageConfiguration_PackageList_WixWpfWizardBA_Demo_msi,
                    Architectures = Architecture.X86 | Architecture.X64,
                    InstallationTypes = new[]
                        {InstallationType.Client, InstallationType.MasterServer, InstallationType.SlaveServer}
                },
                new Package<PackageCombinationConfiguration, InstallationType>
                {
                    PackageId = Netfx35RedistPackageId,
                    DisplayName = Localisation.PackageConfiguration_PackageList__NET_Framework_3_5,
                    Architectures = Architecture.X86 | Architecture.X64,
                    InstallationTypes = new[]
                        {InstallationType.Client, InstallationType.MasterServer, InstallationType.SlaveServer},
                    IsRemovable = false
                },
                new Package<PackageCombinationConfiguration, InstallationType>
                {
                    PackageId = Netfx462RedistPackageId,
                    DisplayName = Localisation.PackageConfiguration_PackageList__NET_Framework_4_6_2,
                    Architectures = Architecture.X86 | Architecture.X64,
                    InstallationTypes = new[]
                        {InstallationType.Client, InstallationType.MasterServer, InstallationType.SlaveServer},
                    IsRemovable = false
                },
                new Package<PackageCombinationConfiguration, InstallationType>
                {
                    PackageId = Sql2014ExpressPackage86Id,
                    DisplayName = Localisation
                        .PackageConfiguration_PackageList_SQL_Server_2014_with_Advanced_Services__32_bit_,
                    Architectures = Architecture.X86,
                    InstallationTypes = new[] {InstallationType.MasterServer},
                    AdditionalPredicate =
                        packageCombinationConfiguration => packageCombinationConfiguration.SqlServerInstallationType !=
                                                           SqlServerInstallationType.None,
                    IsRemovable = false,
                    IsRepairable = false
                },
                new Package<PackageCombinationConfiguration, InstallationType>
                {
                    PackageId = Sql2014ExpressUpgradePackage86Id,
                    DisplayName = Localisation
                        .PackageConfiguration_PackageList_Upgrade_to_SQL_Server_2014_with_Advanced_Services__32_bit_,
                    Architectures = Architecture.X86 | Architecture.X64,
                    InstallationTypes = new[] {InstallationType.MasterServer},
                    AdditionalPredicate =
                        packageCombinationConfiguration =>
                            packageCombinationConfiguration.SqlServerInstallationType ==
                            SqlServerInstallationType.UpgradeMajor
                            && packageCombinationConfiguration.CheckSqlServer32BitInstanceInstanceExists(),
                    IsRemovable = false,
                    IsRepairable = false,
                    AcquireDuringLayout = false
                },
                new Package<PackageCombinationConfiguration, InstallationType>
                {
                    PackageId = Sql2014ExpressPatchPackage86Id,
                    DisplayName = Localisation
                        .PackageConfiguration_PackageList_SQL_Server_2014_with_Advanced_Services__32_bit__patch,
                    Architectures = Architecture.X86 | Architecture.X64,
                    InstallationTypes = new[] {InstallationType.MasterServer},
                    AdditionalPredicate =
                        packageCombinationConfiguration => packageCombinationConfiguration.SqlServerInstallationType ==
                                                           SqlServerInstallationType.UpgradeMinor
                                                           && packageCombinationConfiguration
                                                               .CheckSqlServer32BitInstanceInstanceExists(),
                    IsRemovable = false,
                    IsRepairable = false,
                    AcquireDuringLayout = false
                },
                new Package<PackageCombinationConfiguration, InstallationType>
                {
                    PackageId = Sql2014ExpressPackage64Id,
                    DisplayName = Localisation
                        .PackageConfiguration_PackageList_SQL_Server_2014_with_Advanced_Services__64_bit_,
                    Architectures = Architecture.X64,
                    InstallationTypes = new[] {InstallationType.MasterServer},
                    AdditionalPredicate =
                        packageCombinationConfiguration => packageCombinationConfiguration.SqlServerInstallationType !=
                                                           SqlServerInstallationType.None,
                    IsRemovable = false,
                    IsRepairable = false
                },
                new Package<PackageCombinationConfiguration, InstallationType>
                {
                    PackageId = Sql2014ExpressUpgradePackage64Id,
                    DisplayName = Localisation
                        .PackageConfiguration_PackageList_Upgrade_to_SQL_Server_2014_with_Advanced_Services__64_bit_,
                    Architectures = Architecture.X64,
                    InstallationTypes = new[] {InstallationType.MasterServer},
                    AdditionalPredicate =
                        packageCombinationConfiguration => packageCombinationConfiguration.SqlServerInstallationType ==
                                                           SqlServerInstallationType.UpgradeMajor
                                                           && packageCombinationConfiguration
                                                               .CheckSqlServer64BitInstanceExists(),
                    IsRemovable = false,
                    IsRepairable = false,
                    AcquireDuringLayout = false
                },
                new Package<PackageCombinationConfiguration, InstallationType>
                {
                    PackageId = Sql2014ExpressPatchPackage64Id,
                    DisplayName = Localisation
                        .PackageConfiguration_PackageList_SQL_Server_2014_with_Advanced_Services__64_bit__patch,
                    Architectures = Architecture.X64,
                    InstallationTypes = new[] {InstallationType.MasterServer},
                    AdditionalPredicate =
                        packageCombinationConfiguration => packageCombinationConfiguration.SqlServerInstallationType ==
                                                           SqlServerInstallationType.UpgradeMinor &&
                                                           packageCombinationConfiguration
                                                               .CheckSqlServer64BitInstanceExists(),
                    IsRemovable = false,
                    IsRepairable = false,
                    AcquireDuringLayout = false
                }
            };
    }
}