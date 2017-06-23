namespace WixWPFWizardBA.Views.Pages.ProgressPage
{
    using System;
    using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

    public static class ActionTypeUtils
    {
        public static ActionType ToActionType(CacheOperation operation)
        {
            switch (operation)
            {
                case CacheOperation.Copy:
                    return ActionType.Copy;
                case CacheOperation.Download:
                    return ActionType.Download;
                case CacheOperation.Extract:
                    return ActionType.Extract;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }
        }
    }
}