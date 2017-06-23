namespace WixWPFWizardBA.Views.Pages.AdvancedConfigurationPage
{
    using System;
    using System.IO;
    using System.Windows.Input;
    using ConfigurationPage;
    using Ookii.Dialogs.Wpf;

    public class AdvancedConfigurationViewModel : PageViewModel
    {
        private readonly WixVariableHelper _installFolderHelper;
        private readonly WixVariableHelper _sqlServerAdditionalParametersHelper;

        public AdvancedConfigurationViewModel(WizardViewModel wizardViewModel) : base(wizardViewModel)
        {
            this.CanCancel = true;
            this.CanGoToPreviousPage = true;
            this.CanGoToNextPage = true;

            this.InstallFolderBrowseCommand = new SimpleCommand(_ =>
            {
                var folderBrowserDialog = new VistaFolderBrowserDialog
                {
                    SelectedPath = this.GetBrowserDialogInitialPath(this.InstallFolder)
                };

                folderBrowserDialog.ShowDialog();
                var newPath = folderBrowserDialog.SelectedPath;
                if (!newPath.EndsWith(Path.DirectorySeparatorChar + this.Bootstrapper.DefaultInstallDirectoryName))
                    newPath = Path.Combine(newPath, this.Bootstrapper.DefaultInstallDirectoryName);
                this.InstallFolder = newPath;
            }, _ => true);


            this._installFolderHelper = new WixVariableHelper(wizardViewModel.Bootstrapper, "InstallDir");
            this._sqlServerAdditionalParametersHelper =
                new WixVariableHelper(wizardViewModel.Bootstrapper, "SqlServerAdditionalParameters");

            // Expand default installation directory to concrete path.

            var installFolder = wizardViewModel.Bootstrapper.Engine.FormatString(this._installFolderHelper.Get());
            this._installFolderHelper.Set(installFolder);
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

        private string GetBrowserDialogInitialPath(string installFolder)
        {
            if (installFolder == null || !this.IsPathValidRootedLocal(installFolder))
                return this.WizardViewModel.Bootstrapper.Engine.FormatString(
                    "[ProgramFilesFolder]" + Path.DirectorySeparatorChar);
            if (Directory.Exists(installFolder))
                return new DirectoryInfo(installFolder).FullName;
            return this.GetBrowserDialogInitialPath(Path.GetDirectoryName(installFolder));
        }

        public bool IsPathValidRootedLocal(string pathString)
        {
            Uri pathUri;
            var isValidUri = Uri.TryCreate(pathString, UriKind.Absolute, out pathUri);

            return isValidUri && pathUri != null && pathUri.IsLoopback;
        }
    }
}