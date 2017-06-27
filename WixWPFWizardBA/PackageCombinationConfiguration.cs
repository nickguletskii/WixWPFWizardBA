//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Sql;
    using Common;
    using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
    using Microsoft.Win32;
    using Utilities;
    using Views.Pages.ConfigurationPage;

    public class PackageCombinationConfiguration : INotifyPropertyChanged, IInstallationTypeProvider<InstallationType>
    {
        private readonly BootstrapperApplication _bootstrapper;
        private readonly WixVariableHelper _installationTypeHelper;
        private readonly WixVariableHelper _sqlServerInstallationTypeHelper;

        private readonly WixVariableHelper _sqlServerInstanceNameHelper;

        public PackageCombinationConfiguration(WixBootstrapper bootstrapper)
        {
            this._bootstrapper = bootstrapper;
            this._sqlServerInstanceNameHelper = new WixVariableHelper(bootstrapper, "InstanceName");
            this._installationTypeHelper = new WixVariableHelper(bootstrapper, "BundleInstallationType");
            this._sqlServerInstallationTypeHelper =
                new WixVariableHelper(bootstrapper, "BundleSqlServerInstallationType");
            this.UpdateSqlServerInstallationType();
        }

        public string SqlServerInstanceName
        {
            get => this._sqlServerInstanceNameHelper.Get();
            set
            {
                if (this._sqlServerInstanceNameHelper.Set(value))
                {
                    this.OnPropertyChanged(nameof(this.SqlServerInstanceName));
                    this.UpdateSqlServerInstallationType();
                }
            }
        }

        public SqlServerInstallationType SqlServerInstallationType
        {
            get => (SqlServerInstallationType)
                Enum.Parse(typeof(SqlServerInstallationType), this._sqlServerInstallationTypeHelper.Get());
            set
            {
                if (this._sqlServerInstallationTypeHelper.Set(value.ToString()))
                {
                    this.OnPropertyChanged(nameof(this.SqlServerInstallationType));
                }
            }
        }

        public InstallationType InstallationType
        {
            get => (InstallationType) Enum.Parse(typeof(InstallationType), this._installationTypeHelper.Get());
            set
            {
                if (this._installationTypeHelper.Set(value.ToString()))
                {
                    this.OnPropertyChanged(nameof(this.InstallationType));
                    this.UpdateSqlServerInstallationType();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdateSqlServerInstallationType()
        {
            try
            {
                if (this.InstallationType != InstallationType.MasterServer)
                {
                    this._bootstrapper.Engine.Log(LogLevel.Standard,
                        $"SqlServerInstallationType == None because client-only/slave server setup was selected.");
                    this.SqlServerInstallationType = SqlServerInstallationType.None;
                    return;
                }
                if (!this.CheckSqlServer32BitInstanceInstanceExists() && !this.CheckSqlServer64BitInstanceExists())
                {
                    this._bootstrapper.Engine.Log(LogLevel.Standard,
                        $"SqlServerInstallationType == Install because the instance {this.SqlServerInstanceName} wasn't found.");
                    this.SqlServerInstallationType = SqlServerInstallationType.Install;
                    return;
                }
                if (this.ShouldUpgradeMajor())
                {
                    this._bootstrapper.Engine.Log(LogLevel.Standard,
                        $"SqlServerInstallationType == UpgradeMajor because the instance {this.SqlServerInstanceName} is outdated.");
                    this.SqlServerInstallationType = SqlServerInstallationType.UpgradeMajor;
                    return;
                }
                if (this.ShouldUpgradeMinor())
                {
                    this._bootstrapper.Engine.Log(LogLevel.Standard,
                        $"SqlServerInstallationType == UpgradeMinor because the instance {this.SqlServerInstanceName} is outdated.");
                    this.SqlServerInstallationType = SqlServerInstallationType.UpgradeMinor;
                    return;
                }
                this._bootstrapper.Engine.Log(LogLevel.Standard,
                    $"SqlServerInstallationType == None because the instance {this.SqlServerInstanceName} seems satisfactory.");
                this.SqlServerInstallationType = SqlServerInstallationType.None;
            }
            catch (Exception e)
            {
                this._bootstrapper.Engine.Log(LogLevel.Error,
                    $"An exception has occurred during the detection of SQL server: {e}");
                this._bootstrapper.Engine.Log(LogLevel.Standard,
                    $"SqlServerInstallationType is unknown due to previous exceptions.");
                this.SqlServerInstallationType = SqlServerInstallationType.Unknown;
            }
        }

        public bool CheckIfDefaultSqlServerInstanceIsInstalled()
        {
            var sqldatasourceenumerator1 = SqlDataSourceEnumerator.Instance;
            var datatable1 = sqldatasourceenumerator1.GetDataSources();
            foreach (DataRow row in datatable1.Rows)
            {
                if ((string) row["InstanceName"] == "SQLEXPRESS")
                    return true;
            }
            return false;
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool CheckSqlServer32BitInstanceInstanceExists()
        {
            var path = @"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL\";
            if (SystemInformationUtilities.Is64BitSystem())
                return
                    RegistryUtilities.GetRegistryValue32Bit(RegistryHive.LocalMachine, path,
                        this.SqlServerInstanceName) != null;
            using (
                var key =
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL\"))
            {
                return key?.GetValue(this.SqlServerInstanceName) != null;
            }
        }

        public bool CheckSqlServer64BitInstanceExists()
        {
            var path = @"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL\";
            if (!SystemInformationUtilities.Is64BitSystem())
                return false;
            return RegistryUtilities.GetRegistryValue64Bit(RegistryHive.LocalMachine, path,
                       this.SqlServerInstanceName) != null;
        }

        public Version GetSqlServerInstanceVersion()
        {
            var path =
                $@"SOFTWARE\Microsoft\Microsoft SQL Server\{this.SqlServerInstanceName}\MSSQLServer\CurrentVersion\";
            this._bootstrapper.Engine.Log(LogLevel.Standard, $"Reading {path} for version information.");
            if (SystemInformationUtilities.Is64BitSystem())
            {
                this._bootstrapper.Engine.Log(LogLevel.Standard,
                    $"64 bit system, checking path {path} in both the 64 bit and 32 bit registries.");
                var data = (string) (RegistryUtilities.GetRegistryValue64Bit(RegistryHive.LocalMachine, path,
                                         "CurrentVersion")
                                     ?? RegistryUtilities.GetRegistryValue32Bit(RegistryHive.LocalMachine, path,
                                         "CurrentVersion"));
                this._bootstrapper.Engine.Log(LogLevel.Standard, $"Registry returned {data}");
                return new Version(data);
            }
            using (
                var key =
                    Registry.LocalMachine.OpenSubKey(path))
            {
                this._bootstrapper.Engine.Log(LogLevel.Standard,
                    $"Using normal APIs to check the registry for SQL server version");
                var data = (string) key.GetValue("CurrentVersion");
                this._bootstrapper.Engine.Log(LogLevel.Standard, $"Registry returned {data}");
                return new Version(data);
            }
        }

        public bool ShouldUpgradeMajor()
        {
            var version = this.GetSqlServerInstanceVersion();
            return version < new Version("12.0.0.0");
        }

        public bool ShouldUpgradeMinor()
        {
            var version = this.GetSqlServerInstanceVersion();
            return version >= new Version("12.0.0.0") && version < new Version("12.0.2000.8");
        }
    }
}