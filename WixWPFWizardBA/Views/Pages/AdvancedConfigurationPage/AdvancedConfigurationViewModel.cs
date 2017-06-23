namespace WixWPFWizardBA.Views.Pages.AdvancedConfigurationPage
{
    using System;
    using System.IO;
    using System.Windows.Input;
    using ConfigurationPage;
    using Ookii.Dialogs.Wpf;
    using Utilities;

    public class AdvancedConfigurationViewModel : PageViewModel
    {
        private readonly WixVariableHelper _installFolderHelper;
        private readonly WixVariableHelper _sqlServerAdditionalParametersHelper;
        private readonly WixVariableHelper _sqlServerInstanceInstallFolderHelper;

        public AdvancedConfigurationViewModel(WizardViewModel wizardViewModel) : base(wizardViewModel)
        {
            this.CanCancel = true;
            this.CanGoToPreviousPage = true;
            this.CanGoToNextPage = true;

            var defaultInstallDirectoryName = wizardViewModel.Bootstrapper.BundleName;
            var defaultSqlServerInstanceInstallFolderName = "Microsoft SQL Server";

            // Change this if your app can be 64 bit!
            var defaultInstallFolderParent = this.ProgramFiles32BitFolder;

            var defaultSqlServerInstanceInstallFolderParent = wizardViewModel.PackageCombinationConfiguration
                .CheckSqlServer32BitInstanceInstanceExists()
                ? this.ProgramFiles32BitFolder
                : this.ProgramFilesNativeFolder;

            var defaultInstallFolder = Path.Combine(defaultInstallFolderParent, defaultInstallDirectoryName);

            var defaultSqlServerInstanceInstallFolder =
                Path.Combine(defaultSqlServerInstanceInstallFolderParent, defaultSqlServerInstanceInstallFolderName);

            this.InstallFolderBrowseCommand = new SimpleCommand(_ =>
            {
                var folderBrowserDialog = new VistaFolderBrowserDialog
                {
                    SelectedPath = this.GetBrowserDialogInitialPath(this.InstallFolder, defaultInstallFolderParent)
                };

                folderBrowserDialog.ShowDialog();
                var newPath = folderBrowserDialog.SelectedPath;
                if (!newPath.EndsWith(Path.DirectorySeparatorChar + defaultInstallDirectoryName))
                    newPath = Path.Combine(newPath, defaultInstallDirectoryName);
                this.InstallFolder = newPath;
            }, _ => true);


            this.SqlServerInstanceInstallFolderBrowseCommand = new SimpleCommand(_ =>
            {
                var folderBrowserDialog = new VistaFolderBrowserDialog
                {
                    SelectedPath = this.GetBrowserDialogInitialPath(this.SqlServerInstanceInstallFolder,
                        defaultSqlServerInstanceInstallFolderParent)
                };

                folderBrowserDialog.ShowDialog();
                var newPath = folderBrowserDialog.SelectedPath;
                if (!newPath.EndsWith(Path.DirectorySeparatorChar + defaultSqlServerInstanceInstallFolderName))
                    newPath = Path.Combine(newPath, defaultSqlServerInstanceInstallFolderName);
                this.SqlServerInstanceInstallFolder = newPath;
            }, _ => true);


            this._installFolderHelper = new WixVariableHelper(wizardViewModel.Bootstrapper, "InstallDir");
            this._sqlServerInstanceInstallFolderHelper =
                new WixVariableHelper(wizardViewModel.Bootstrapper, "SqlServerInstanceInstallDir",
                    x => x.TrimEnd('\\', '/'));
            this._sqlServerAdditionalParametersHelper =
                new WixVariableHelper(wizardViewModel.Bootstrapper, "SqlServerAdditionalParameters");

            this._installFolderHelper.Set(defaultInstallFolder);
            this._sqlServerInstanceInstallFolderHelper.Set(defaultSqlServerInstanceInstallFolder);
        }

        /// <summary>
        ///     Because the bootstrapper is a 32-bit process, ProgramFilesFolder will point to Program Files (x86) on 64 bit
        ///     systems.
        /// </summary>
        private string ProgramFiles32BitFolder =>
            this.WizardViewModel.Bootstrapper.Engine.FormatString("[ProgramFilesFolder]");


        private string ProgramFilesNativeFolder
        {
            get
            {
                if (SystemInformationUtilities.Is64BitSystem())
                {
                    return this.WizardViewModel.Bootstrapper.Engine.FormatString("[ProgramFiles64Folder]");
                }
                return this.WizardViewModel.Bootstrapper.Engine.FormatString("[ProgramFilesFolder]");
            }
        }

        public SimpleCommand SqlServerInstanceInstallFolderBrowseCommand { get; set; }

        public string InstallFolder
        {
            get => this._installFolderHelper.Get();
            set
            {
                if (this._installFolderHelper.Set(value))
                {
                    this.OnPropertyChanged(nameof(this.InstallFolder));
                }
            }
        }

        public string SqlServerInstanceInstallFolder
        {
            get => this._sqlServerInstanceInstallFolderHelper.Get();
            set
            {
                if (this._sqlServerInstanceInstallFolderHelper.Set(value))
                {
                    this.OnPropertyChanged(nameof(this.SqlServerInstanceInstallFolder));
                }
            }
        }

        public string SqlServerAdditionalParameters
        {
            get => this._sqlServerAdditionalParametersHelper.Get();
            set
            {
                if (this._sqlServerAdditionalParametersHelper.Set(value))
                {
                    this.OnPropertyChanged(nameof(this.SqlServerAdditionalParameters));
                }
            }
        }

        public ICommand InstallFolderBrowseCommand { get; }

        private string GetBrowserDialogInitialPath(string installFolder, string defaultParent)
        {
            if (installFolder == null || !this.IsPathValidRootedLocal(installFolder))
                return defaultParent;
            if (Directory.Exists(installFolder))
                return new DirectoryInfo(installFolder).FullName;

            // GetDirectoryName is confusing - this actually gets the parent directory.
            return this.GetBrowserDialogInitialPath(Path.GetDirectoryName(installFolder), defaultParent);
        }

        public bool IsPathValidRootedLocal(string pathString)
        {
            Uri pathUri;
            var isValidUri = Uri.TryCreate(pathString, UriKind.Absolute, out pathUri);

            return isValidUri && pathUri != null && pathUri.IsLoopback;
        }
    }
}