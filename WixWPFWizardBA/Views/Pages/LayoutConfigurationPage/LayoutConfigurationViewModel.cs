//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Views.Pages.LayoutConfigurationPage
{
    using System.IO;
    using System.Windows.Input;
    using ConfigurationPage;
    using Ookii.Dialogs.Wpf;

    public class LayoutConfigurationViewModel : PageViewModel
    {
        private readonly WixVariableHelper _layoutFolderHelper;

        public LayoutConfigurationViewModel(WizardViewModel wizardViewModel) : base(wizardViewModel)
        {
            this.CanCancel = true;
            this.CanGoToPreviousPage = true;
            this.CanGoToNextPage = true;
            this.LayoutFolderBrowseCommand = new SimpleCommand(_ =>
            {
                var folderBrowserDialog = new VistaFolderBrowserDialog
                {
                    SelectedPath = Directory.GetCurrentDirectory()
                };

                folderBrowserDialog.ShowDialog();
                this.LayoutFolder = folderBrowserDialog.SelectedPath;
            }, _ => true);
            this._layoutFolderHelper = new WixVariableHelper(wizardViewModel.Bootstrapper, "WixBundleLayoutDirectory");
            this._layoutFolderHelper.Set(Directory.GetCurrentDirectory());
        }

        public string LayoutFolder
        {
            get => this._layoutFolderHelper.Get();
            set
            {
                if (this._layoutFolderHelper.Set(value))
                {
                    this.OnPropertyChanged(nameof(this.LayoutFolder));
                }
            }
        }


        public ICommand LayoutFolderBrowseCommand { get; }
    }
}