namespace WixWPFWizardBA.Common
{
    using System;

    [Flags]
    public enum Architecture
    {
        X86 = 1 << 0,
        X64 = 1 << 1
    }
}