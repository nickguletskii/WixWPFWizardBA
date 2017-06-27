//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
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