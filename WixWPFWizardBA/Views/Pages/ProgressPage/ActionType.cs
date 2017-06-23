namespace WixWPFWizardBA.Views.Pages.ProgressPage
{
    using System;

    [Flags]
    public enum ActionType
    {
        Execute = 1 << 0,
        Copy = 1 << 1,
        Download = 1 << 2,
        Extract = 1 << 3,
        Caching = Copy | Download | Extract
    }
}