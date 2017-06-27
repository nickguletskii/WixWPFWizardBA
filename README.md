# WixWPFWizardBA

WixWPFWizardBA is a template for custom WPF-based wizard-like Wix Burn frontends.

Issue tracker: https://gitlab.com/nickguletskii/WixWPFWizardBA/issues

## Supported operating systems

WixWPFWizardBA supports Windows 7 SP1 and newer, and has been tested on Windows 7 SP1 and Windows 10.

## Features

* Implements most Wix launch actions (Help and UpdateReplaceEmbedded are not implemented).
* Easily customisable wizard interface.
* Customisable Wix installation planning (see PackageInstallationStrategyBase, PackageInstallationStrategy and PackageConfiguration).
* Default planning strategy supports package and msi feature selection by system architecture and the selected package combination (e.g. Client and Server + Client).
* Resume after reboot (see known issues).
* Bundle self update through Wix update feeds.
* GUI for creating offline installers.
* UI localisation based on the locale reported by Wix Burn (see known issues).

## Demo bootstrapper features

* Localised for English and Russian, but not all text may fit the views because the frontend was initially made for a russian product.
* Installs .NET Framework 4.6.2, a demo msi and SQL Server Express 2014 with advanced services. **WARNING:** To redistribute .NET Framework 4.6.2 and SQL Server Express 2014, you must agree to both the license of .NET Framework and the [Microsoft® SQL Server® Express License Terms for Redistribution and Hosting
](https://www.microsoft.com/en-us/download/details.aspx?id=29693). Both require you to require the user to agree to "terms that protect it at least as much as this agreement", "it" being the product being redistributed.
* Supports two installation types (package combinations): Client and MasterServer (Server + Client). There's also an unused SlaveServer installation type (which isn't present in the GUI).
* Supports SQL Server upgrades.
* Supports customisation of SQL Server instance installation directory.

## How to use

First of all, you should copy the WixWPFWizardBA project, change the namespaces, and change the few localisation strings that contain WixWPFWizardBA's name in them to display your project's name instead.

Then, you must integrate the bootstrapper into your bundle. For an example of how to do that, please see WixWPFWizardBA.DemoBootstrapper.

Once you add the required packages to the Wix definition of the bundle, you should add them to PackageConfiguration, or implement your own IPackageInstallationStrategy.

This list of instructions is by no means complete, so if you have any thoughts on how to expand it, please let me know on the [issue tracker](https://gitlab.com/nickguletskii/WixWPFWizardBA/issues).

## Known issues

* Doesn't implement UpdateReplaceEmbedded (I am not sure what it is supposed to do).
* Doesn't implement Help (I am not sure what that launch action is for, displaying command line options maybe?)
* The default UI doesn't change EULAs based on culture (should be trivial to implement if needed).
* There are many small UI issues, like text overflows.
* Windows reports that an error has occurred for a second when the bootstrapper initiates a reboot if launched from ARP (Add or Remove Programs).
* Passive and quiet installs aren't thoroughly tested.
* The language reported by Wix Burn doesn't match the language used by Windows. Maybe a different language detection mechanism should be used?
* When a reboot is requested mid-install, the bootstrapper correctly requests that the user restart their computer and continues after rebooting, but instead of showing that the setup will continue after a reboot, it displays that the installation has completed successfully.

## License

MIT License

Copyright (c) 2017 Nick Guletskii, Arseniy Aseev

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.