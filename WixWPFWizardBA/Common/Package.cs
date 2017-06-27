//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Common
{
    using System;

    public class Package<TParam, TInstallationType>
    {
        /// <summary>
        ///     The id of the package as described in the bundle definition file.
        /// </summary>
        public string PackageId { get; set; }

        /// <summary>
        ///     The human-readable name of the package.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     The architectures that this package should be installed on.
        /// </summary>
        public Architecture Architectures { get; set; }

        /// <summary>
        ///     The list of installation types this package is associated with. This can be used to quickly implement various
        ///     package combination presets.
        /// </summary>
        public TInstallationType[] InstallationTypes { get; set; }

        /// <summary>
        ///     A predicate that must return true if the package is to be installed.
        /// </summary>
        public Func<TParam, bool> AdditionalPredicate { get; set; } = (TParam param) => true;

        /// <summary>
        ///     Set to false if this package can't be removed.
        /// </summary>
        public bool IsRemovable { get; set; } = true;

        /// <summary>
        ///     Set to false if this package can't be repaired.
        /// </summary>
        public bool IsRepairable { get; set; } = true;

        /// <summary>
        ///     Set to false if this package shouldn't be downloaded when creating an offline installer.
        /// </summary>
        public bool AcquireDuringLayout { get; set; } = true;
    }
}