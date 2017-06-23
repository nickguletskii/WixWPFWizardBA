namespace WixWPFWizardBA.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
    using Utilities;

    public class PackageInstallationStrategyBase<TParam, TInstallationType> : IPackageInstallationStrategy
    {
        private readonly TParam _param;

        public PackageInstallationStrategyBase(IList<Package<TParam, TInstallationType>> packageList,
            TParam param, IInstallationTypeProvider<TInstallationType> installationTypeProvider)
        {
            this._param = param;
            this.PackageList = packageList;
            this.InstallationTypeProvider = installationTypeProvider;
        }

        public IList<Package<TParam, TInstallationType>> PackageList { get; }
        public IInstallationTypeProvider<TInstallationType> InstallationTypeProvider { get; }

        public virtual FeatureState? PlanMsiFeature(LaunchAction launchAction, string packageId, string featureId)
        {
            // Let Burn decide what to do with packages we don't know about
            if (this.PackageList.All(x => x.PackageId != packageId))
                return null;

            // Unless stated otherwise, install the feature.
            return FeatureState.Local;
        }

        public virtual RequestState? PlanPackage(LaunchAction launchAction, string packageId)
        {
            var installationType = this.InstallationTypeProvider.InstallationType;

            var architecture = SystemInformationUtilities.Is64BitSystem() ? Architecture.X64 : Architecture.X86;
            var packageConfig = this.PackageList.FirstOrDefault(x => x.PackageId == packageId);
            switch (launchAction)
            {
                case LaunchAction.Layout:
                    if (packageConfig == null
                        || packageConfig.AcquireDuringLayout)
                    {
                        return RequestState.Present;
                    }
                    else
                    {
                        return RequestState.None;
                    }
                case LaunchAction.Uninstall:
                    if (packageConfig == null
                        || packageConfig.IsRemovable)
                        return RequestState.Absent;
                    return RequestState.None;
                case LaunchAction.Cache:
                case LaunchAction.Install:
                case LaunchAction.Modify:
                    if (packageConfig == null
                        ||
                        packageConfig.InstallationTypes.Contains(installationType)
                        && packageConfig.AdditionalPredicate(this._param)
                        && (packageConfig.Architectures & architecture) == architecture)
                        return RequestState.Present;
                    else
                        return RequestState.Absent;
                case LaunchAction.Repair:
                    if (packageConfig == null || packageConfig.IsRepairable)
                    {
                        return RequestState.Repair;
                    }
                    return RequestState.None;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }

        public virtual void DetectAdditionalInformation()
        {
        }

        public virtual string GetPackageNameFromId(string packageId)
        {
            var packageConfig = this.PackageList.FirstOrDefault(x => x.PackageId == packageId);
            if (packageConfig != null)
                return packageConfig.DisplayName;
            return packageId;
        }
    }
}