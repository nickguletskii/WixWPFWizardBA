//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

    public abstract class BootstrapperManager
    {
        private const string BurnBundleVersionVariable = "WixBundleVersion";

        public const int CancelErrorCode = 1602;
        private readonly IList<string> _bundlesToUpgrade;


        private string _bootstrapperUpdateLocation;
        private UpdateState _bootstrapperUpdateState;

        private BurnInstallationState _burnInstallationState;

        private int _errorCode;
        private string _errorMessage;
        private LaunchAction _executedAction;
        private bool? _isInstalled;


        private LaunchAction _launchAction;
        private bool _restartConfirmed;
        private bool _restartRequired;
        private int _status;
        private VersionStatus _versionStatus;

        protected BootstrapperManager(WixBootstrapper bootstrapper)
        {
            this._bundlesToUpgrade = new List<string>();
            this.Bootstrapper = bootstrapper;

            bootstrapper.DetectUpdateBegin += this.Bootstrapper_DetectUpdateBegin;
            bootstrapper.DetectUpdate += this.Bootstrapper_DetectUpdate;
            bootstrapper.DetectUpdateComplete += this.Bootstrapper_DetectUpdateComplete;

            bootstrapper.DetectBegin += this.Bootstrapper_DetectBegin;
            bootstrapper.DetectComplete += this.Bootstrapper_DetectComplete;
            bootstrapper.DetectPackageComplete += this.Bootstrapper_DetectPackageComplete;
            bootstrapper.Error += this.Bootstrapper_Error;
            bootstrapper.Shutdown += this.Bootstrapper_Shutdown;
            bootstrapper.ResolveSource += this.Bootstrapper_ResolveSource;
            bootstrapper.ApplyBegin += this.Bootstrapper_ApplyBegin;
            bootstrapper.ApplyComplete += this.Bootstrapper_ApplyComplete;
            bootstrapper.PlanBegin += this.Bootstrapper_PlanBegin;
            bootstrapper.PlanPackageBegin += this.Bootstrapper_PlanPackageBegin;
            bootstrapper.PlanComplete += this.Bootstrapper_PlanComplete;
            bootstrapper.PlanRelatedBundle += this.Bootstrapper_PlanRelatedBundle;
            bootstrapper.PlanMsiFeature += this.Bootstrapper_PlanMsiFeature;
            bootstrapper.PlanRelatedBundle += this.Bootstrapper_PlanRelatedBundle;
            bootstrapper.DetectRelatedBundle += this.Bootstrapper_DetectRelatedBundle;

            this.BootstrapperUpdateState = UpdateState.Initializing;
        }

        /// <summary>
        ///     The package installation strategy that will be used to plan the installation.
        /// </summary>
        public abstract IPackageInstallationStrategy PackageInstallationStrategy { get; }

        /// <summary>
        ///     The state of the Wix bundle self-update mechanism.
        /// </summary>
        public UpdateState BootstrapperUpdateState
        {
            get => this._bootstrapperUpdateState;
            set
            {
                if (this._bootstrapperUpdateState != value)
                {
                    this._bootstrapperUpdateState = value;
                    this.OnPropertyChanged(nameof(this.BootstrapperUpdateState));
                    this.Log(LogLevel.Standard,
                        $"{nameof(this.BootstrapperUpdateState)} changed: {this.BootstrapperUpdateState}");
                }
            }
        }

        /// <summary>
        ///     The running bundle version.
        /// </summary>
        public Version Version => new Version(this.Bootstrapper.Engine.VersionVariables[BurnBundleVersionVariable]
            .ToString());

        /// <summary>
        ///     If true, the next Wix bootstrapper event handler that supports Cancel as the Result will be told to cancel the
        ///     installation.
        /// </summary>
        public bool ShouldCancel { get; set; }

        /// <summary>
        ///     The handle to the main bootstrapper window.
        /// </summary>
        public IntPtr WindowHandle { get; set; }

        /// <summary>
        ///     The Wix bootstrapper.
        /// </summary>
        public WixBootstrapper Bootstrapper { get; }


        /// <summary>
        ///     Error message set by Bootstrapper Error event.
        /// </summary>
        public string ErrorMessage
        {
            get => this._errorMessage;
            set
            {
                if (this._errorMessage != value)
                {
                    this._errorMessage = value;
                    this.OnPropertyChanged(nameof(this.ErrorMessage));
                    this.Log(LogLevel.Standard,
                        $"Setting error message: {this.BurnInstallationState}");
                }
            }
        }

        /// <summary>
        ///     Error code set by Bootstrapper Error event.
        /// </summary>
        public int ErrorCode
        {
            get => this._errorCode;
            set
            {
                if (this._errorCode != value)
                {
                    this._errorCode = value;
                    this.OnPropertyChanged(nameof(this.ErrorCode));
                    this.Log(LogLevel.Standard,
                        $"Setting error code: {this.BurnInstallationState}");
                }
            }
        }

        /// <summary>
        ///     The state of Wix Burn install.
        /// </summary>
        public BurnInstallationState BurnInstallationState
        {
            get => this._burnInstallationState;
            set
            {
                this._burnInstallationState = value;
                this.OnPropertyChanged(nameof(this.BurnInstallationState));
                this.Log(LogLevel.Standard,
                    $"{nameof(this.BurnInstallationState)} changed: {this.BurnInstallationState}");
            }
        }

        /// <summary>
        ///     IsInstalled is true if Wix tells us that the bundle is installed through the DetectBegin event.
        /// </summary>
        public bool? IsInstalled
        {
            get => this._isInstalled;
            set
            {
                this._isInstalled = value;
                this.OnPropertyChanged(nameof(this.IsInstalled));
                this.Log(LogLevel.Standard,
                    $"{nameof(this.IsInstalled)} changed: {this.IsInstalled}");
            }
        }


        /// <summary>
        ///     The version status of this bundle in relation to the currently installed bundle.
        /// </summary>
        public VersionStatus VersionStatus
        {
            get => this._versionStatus;
            set
            {
                this._versionStatus = value;
                this.OnPropertyChanged(nameof(this.VersionStatus));
                this.Log(LogLevel.Standard,
                    $"{nameof(this.VersionStatus)} changed: {this.VersionStatus}");
            }
        }

        /// <summary>
        ///     The action that will be executed when the installaiton will begin.
        /// </summary>
        public LaunchAction LaunchAction
        {
            get => this._launchAction;
            set
            {
                if (this._launchAction != value)
                {
                    this._launchAction = value;
                    this.OnPropertyChanged(nameof(this.LaunchAction));
                    this.Log(LogLevel.Standard,
                        $"{nameof(this.LaunchAction)} changed: {this.LaunchAction}");
                }
            }
        }

        /// <summary>
        ///     The action that will be executed when the installaiton will begin.
        /// </summary>
        public LaunchAction ExecutedAction
        {
            get => this._executedAction;
            set
            {
                if (this._executedAction != value)
                {
                    this._executedAction = value;
                    this.OnPropertyChanged(nameof(this.ExecutedAction));
                    this.Log(LogLevel.Standard,
                        $"{nameof(this.ExecutedAction)} changed: {this.ExecutedAction}");
                }
            }
        }

        /// <summary>
        ///     The status given by the Wix bootstrapper's ApplyComplete event.
        /// </summary>
        public int Status
        {
            get => this._status;
            private set
            {
                if (this._status != value)
                {
                    this._status = value;
                    this.OnPropertyChanged(nameof(this.Status));
                    this.Log(LogLevel.Standard,
                        $"{nameof(this.Status)} changed: {this.Status:X8}");
                }
            }
        }

        /// <summary>
        ///     RestartRequired is true if the bootstrapper ApplyComplete event requested or initated a reboot.
        /// </summary>
        public bool RestartRequired
        {
            get => this._restartRequired;
            set
            {
                if (this._restartRequired != value)
                {
                    this._restartRequired = value;
                    this.OnPropertyChanged(nameof(this.RestartRequired));
                    this.Log(LogLevel.Standard,
                        $"{nameof(this.RestartRequired)} changed: {this.RestartRequired}");
                }
            }
        }

        /// <summary>
        ///     RestartConfirmed is set to true when the user presses the Reboot button on the finish page.
        /// </summary>
        public bool RestartConfirmed
        {
            get => this._restartConfirmed;
            set
            {
                if (this._restartConfirmed != value)
                {
                    this._restartConfirmed = value;
                    this.OnPropertyChanged(nameof(this.RestartConfirmed));
                    this.Log(LogLevel.Standard,
                        $"{nameof(this.RestartConfirmed)} changed: {this.RestartConfirmed}");
                }
            }
        }

        /// <summary>
        ///     BootstrapperUpdateLocation is set to the URL of the newest bundle detected by the bootstrapper when the
        ///     DetectUpdate event is fired.
        /// </summary>
        public string BootstrapperUpdateLocation
        {
            get => this._bootstrapperUpdateLocation;
            set
            {
                if (this._bootstrapperUpdateLocation != value)
                {
                    this._bootstrapperUpdateLocation = value;
                    this.OnPropertyChanged(nameof(this.BootstrapperUpdateLocation));
                    this.Log(LogLevel.Standard,
                        $"{nameof(this.BootstrapperUpdateLocation)} changed: {this.BootstrapperUpdateLocation}");
                }
            }
        }

        /// <summary>
        ///     IsInteractive is true when the bootstrapper is running in interactive mode, i.e. Display.Full. Passive and quiet
        ///     installs are not interactive.
        /// </summary>
        protected bool IsInteractive => this.Bootstrapper.Command.Display == Display.Full;

        /// <summary>
        ///     IsVisible is true when the GUI should be visible. This is false on silent installs.
        /// </summary>
        public bool IsVisible => this.Bootstrapper.Command.Display == Display.Full ||
                                 this.Bootstrapper.Command.Display == Display.Passive;

        /// <summary>
        ///     CancelButtonPressedButNotConfirmed is set to true when the user presses the cancel button. This, however, doesn't
        ///     mean that the installation should be cancelled - the user can still choose not to cancel in the confirmation
        ///     dialog.
        /// </summary>
        public bool CancelButtonPressed { get; set; }

        /// <summary>
        ///     InitiateBootstrapperUpdate sets the launch action to UpdateReplace and begins planning the update.
        /// </summary>
        public void InitiateBootstrapperUpdate()
        {
            this.LaunchAction = LaunchAction.UpdateReplace;
            this.BeginPlanningAction(LaunchAction.UpdateReplace);
        }


        private void Bootstrapper_DetectUpdateComplete(object sender, DetectUpdateCompleteEventArgs e)
        {
            this.Log(LogLevel.Debug,
                $"Bootstrapper has called {nameof(this.Bootstrapper_DetectUpdateComplete)}!");

            if (this.BootstrapperUpdateState != UpdateState.Failed && e.Status < 0)
            {
                this.Log(LogLevel.Standard,
                    $"Failed to detect updates, status: {e.Status:X8}");
                this.BootstrapperUpdateState = UpdateState.Failed;
                // Re-detect, updates are now disabled
                this.Bootstrapper.Engine.Detect();
            }
            else if (this.Bootstrapper.Command.Action == LaunchAction.Uninstall
                     || this.BootstrapperUpdateState == UpdateState.Initializing
                     || this.BootstrapperUpdateState == UpdateState.Checking
                     || !this.IsInteractive)
            {
                this.BootstrapperUpdateState = UpdateState.Unknown;
            }
        }

        private void Bootstrapper_DetectUpdate(object sender, DetectUpdateEventArgs e)
        {
            this.Log(LogLevel.Debug,
                $"Bootstrapper has called {nameof(this.Bootstrapper_DetectUpdate)}!");

            this.Log(LogLevel.Standard,
                $"Bootstrapper found bundle version {e.Version} at \"{e.UpdateLocation}\", current version: {this.Version}");
            if (e.Version > this.Version)
            {
                this.BootstrapperUpdateLocation = e.UpdateLocation;
                this.Bootstrapper.Engine.SetUpdate(null, e.UpdateLocation, e.Size, UpdateHashType.None, null);
                this.BootstrapperUpdateState = UpdateState.Available;
                e.Result = Result.Ok;
            }
            else if (e.Version <= this.Version)
            {
                this.BootstrapperUpdateState = UpdateState.Current;
                e.Result = Result.Cancel;
            }
        }

        private void Bootstrapper_DetectUpdateBegin(object sender, DetectUpdateBeginEventArgs e)
        {
            this.Log(LogLevel.Debug,
                $"Bootstrapper has called {nameof(this.Bootstrapper_DetectUpdateBegin)}");
            //http://wixtoolset.org/releases/feed/v3.11
            if (this.IsInteractive
                && (this.Bootstrapper.Command.Resume == ResumeType.None ||
                    this.Bootstrapper.Command.Resume == ResumeType.Arp)
                &&
                this.BootstrapperUpdateState != UpdateState.Failed
                && this.Bootstrapper.Command.Action != LaunchAction.Uninstall)
            {
                this.BootstrapperUpdateState = UpdateState.Checking;
                e.Result = Result.Ok;
            }
            this.ExecuteOnDispatcherIfInteractive(this.BeginUpdate);
        }

        /// <summary>
        ///     Called when the bootstrapper's DetectUpdateBegin event is handled.
        /// </summary>
        protected abstract void BeginUpdate();


        private void Bootstrapper_DetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
        {
            this.Log(LogLevel.Debug,
                $"Package detection complete: {e.PackageId}, State: {e.State}");
        }

        private void Bootstrapper_DetectBegin(object sender, DetectBeginEventArgs e)
        {
            this.Log(LogLevel.Debug,
                $"Bootstrapper has called {nameof(this.Bootstrapper_DetectBegin)}");
            if (!e.Installed)
            {
                this.Log(LogLevel.Standard,
                    $"Bootstrapper Detect resulted in DetectedAbsent");
                this.IsInstalled = false;
                this.BurnInstallationState = BurnInstallationState.Detected;
                return;
            }

            this.Log(LogLevel.Standard,
                $"Bootstrapper Detect resulted in DetectedPresent");
            this.IsInstalled = true;
            this.BurnInstallationState = BurnInstallationState.Detected;
        }

        private void Bootstrapper_ResolveSource(object sender, ResolveSourceEventArgs e)
        {
            this.Log(LogLevel.Standard,
                $"Bootstrapper has called {nameof(this.Bootstrapper_ResolveSource)}, Download source: {e.DownloadSource}");
            e.Result = Result.Download;
        }

        private void Bootstrapper_Error(object sender, ErrorEventArgs e)
        {
            e.Result = Result.Restart;
            this.Log(LogLevel.Debug,
                $"Bootstrapper has called {nameof(this.Bootstrapper_Error)}");
            lock (this)
            {
                try
                {
                    this.Log(LogLevel.Error,
                        $"Bootstrapper received error code {e.ErrorCode}: {e.ErrorMessage}");

                    if (this.ShouldCancel)
                    {
                        e.Result = Result.Cancel;
                        return;
                    }

                    if (this.BurnInstallationState == BurnInstallationState.Applying && e.ErrorCode == 1223
                        /* Cancelled */)
                    {
                        return;
                    }

                    if (ErrorType.HttpServerAuthentication == e.ErrorType ||
                        ErrorType.HttpProxyAuthentication == e.ErrorType)
                    {
                        e.Result = Result.TryAgain;
                        return;
                    }

                    this.ErrorCode = e.ErrorCode;
                    this.ErrorMessage = e.ErrorMessage;

                    if (!this.IsInteractive) return;

                    var messageBoxButtonValue = e.UIHint & 0xF;

                    MessageBoxButton messageBoxButton;
                    if (Enum.GetValues(typeof(MessageBoxButton)).OfType<int>().Contains(messageBoxButtonValue))
                    {
                        messageBoxButton = (MessageBoxButton) messageBoxButtonValue;
                    }
                    else
                    {
                        messageBoxButton = MessageBoxButton.OK;
                    }

                    var result = MessageBoxResult.None;
                    WixBootstrapper.BootstrapperDispatcher.Invoke(
                        (Action)
                        delegate
                        {
                            result = MessageBox.Show(WixBootstrapper.RootView, e.ErrorMessage,
                                string.Format(Localisation.BootstrapperManager_InstallErrorMessageTitle,
                                    this.Bootstrapper.BundleName),
                                messageBoxButton, MessageBoxImage.Error);
                        });

                    if (messageBoxButtonValue == (int) messageBoxButton)
                    {
                        e.Result = (Result) result;
                    }
                }
                finally
                {
                    this.Log(LogLevel.Error,
                        $"Bootstrapper handled error {e.ErrorCode}: {e.ErrorMessage}");
                }
            }
        }

        private void Bootstrapper_DetectComplete(object sender, DetectCompleteEventArgs e)
        {
            this.Log(LogLevel.Debug,
                $"Bootstrapper has called {nameof(this.Bootstrapper_DetectComplete)}");
            // Allow the PackageInstallationStrategy to do a further investigation of the system.
            this.PackageInstallationStrategy.DetectAdditionalInformation();

            if (this.VersionStatus == VersionStatus.Current)
            {
                this.BurnInstallationState = BurnInstallationState.Failed;
                this.Log(LogLevel.Standard, "An attempt to reinstall the application was made.");
                if (!this.IsInteractive)
                {
                    this.ShutDownWithCancelCode();
                }
            }
            if (this.VersionStatus == VersionStatus.NewerAlreadyInstalled)
            {
                this.BurnInstallationState = BurnInstallationState.DetectedNewer;
                this.Log(LogLevel.Standard, "An attempt to downgrade the application was made.");
                this.ExecuteOnDispatcherIfInteractive(
                    () =>
                        MessageBox.Show(WixBootstrapper.RootView,
                            string.Format(Localisation.BootstrapperManager_NewerVersionInstalledMessage,
                                this.Bootstrapper.BundleName),
                            string.Format(Localisation.BootstrapperManager_NewerVersionInstalledTitle,
                                this.Bootstrapper.BundleName),
                            MessageBoxButton.OK,
                            MessageBoxImage.Error));
                this.ShutDownWithCancelCode();
            }
            else if (this.IsInteractive &&
                     (this.Bootstrapper.Command.Resume == ResumeType.None ||
                      this.Bootstrapper.Command.Resume == ResumeType.Arp))
            {
                this.ExecuteOnDispatcher(this.OnBootstrapperShouldGoToFirstPage);
            }
            else
            {
                this.Log(LogLevel.Standard,
                    $"Wix bootstrapper detect complete, resume: {this.Bootstrapper.Command.Resume}, launch action: {this.Bootstrapper.Command.Action}!");

                this.ExecuteOnDispatcherOrImmediateIfNotAvailable(() =>
                {
                    this.SetInstallModeFromBootstrapper();
                    this.BeginPlanningAction(this.LaunchAction);
                });
            }
        }

        /// <summary>
        ///     Initiates bootstrapper planning with the specified launch action.
        /// </summary>
        /// <param name="launchAction"></param>
        public void BeginPlanningAction(LaunchAction launchAction)
        {
            this.Log(LogLevel.Debug, $"Planning action with {launchAction}");
            this.Bootstrapper.Engine.Plan(launchAction);
        }

        /// <summary>
        ///     OnBootstrapperShouldGoToFirstPage is called when the bootstrapper completes detecting the bundle, but ONLY if the
        ///     bootstrapper is running interactively.
        /// </summary>
        protected abstract void OnBootstrapperShouldGoToFirstPage();

        private void Bootstrapper_ApplyBegin(object sender, ApplyBeginEventArgs e)
        {
            this.Log(LogLevel.Debug,
                $"Bootstrapper has called {nameof(this.Bootstrapper_ApplyBegin)}");
            this.BurnInstallationState = BurnInstallationState.Applying;

            this.ExecuteOnDispatcherOrImmediateIfNotAvailable(this.ApplyBegin);
        }

        /// <summary>
        ///     Called when the bootstrapper begins applying the planned changes.
        /// </summary>
        protected abstract void ApplyBegin();

        /// <summary>
        ///     Copies the bootstrapper's action into the current launch action.
        /// </summary>
        private void SetInstallModeFromBootstrapper()
        {
            this.Log(LogLevel.Standard,
                $"Setting the currently selected launch action to {this.Bootstrapper.Command.Action}, as selected by the bootstrapper!");
            this.LaunchAction = this.Bootstrapper.Command.Action;
        }

        /// <summary>
        ///     Executes the action on the dispatcher thread if interactive.
        /// </summary>
        /// <param name="action"></param>
        public void ExecuteOnDispatcherIfInteractive(Action action)
        {
            if (this.IsInteractive)
                this.ExecuteOnDispatcher(action);
        }

        /// <summary>
        ///     Executes action on the dispatcher thread if visible, or on the current thread if not.
        /// </summary>
        /// <param name="action"></param>
        public void ExecuteOnDispatcherOrImmediateIfNotAvailable(Action action)
        {
            if (this.IsVisible)
                this.ExecuteOnDispatcher(action);
            else
                action();
        }

        public void ExecuteOnDispatcher(Action action)
        {
            WixBootstrapper.BootstrapperDispatcher.Invoke(action);
        }

        /// <summary>
        ///     Tells the bootstrapper to begin applying the planned changes.
        /// </summary>
        public void ApplyAction()
        {
            this.Log(LogLevel.Standard, "ApplyAction called.");
            this.BurnInstallationState = BurnInstallationState.Applying;
            this.Bootstrapper.Engine.Apply(this.WindowHandle);
        }

        private void Bootstrapper_PlanMsiFeature(object sender, PlanMsiFeatureEventArgs e)
        {
            this.Log(LogLevel.Standard,
                $"Bootstrapper planning MsiFeature {e.FeatureId} of {e.PackageId}!");
            var state = this.PackageInstallationStrategy.PlanMsiFeature(this.LaunchAction, e.PackageId, e.FeatureId);
            if (state != null)
                e.State = (FeatureState) state;
        }

        private void Bootstrapper_PlanComplete(object sender, PlanCompleteEventArgs e)
        {
            this.Log(LogLevel.Debug,
                $"Bootstrapper has called {nameof(this.Bootstrapper_PlanComplete)}");
            this.Bootstrapper.PlanComplete -= this.Bootstrapper_PlanComplete;
            if (this.BurnInstallationState != BurnInstallationState.Applying)
            {
                this.ApplyAction();
            }
        }

        private void Bootstrapper_PlanPackageBegin(object sender, PlanPackageBeginEventArgs e)
        {
            this.Log(LogLevel.Debug,
                $"Bootstrapper has called {nameof(this.Bootstrapper_PlanPackageBegin)}, package id: {e.PackageId}");

            if (this.LaunchAction == LaunchAction.UpdateReplace)
                return;
            var state = this.PackageInstallationStrategy.PlanPackage(this.LaunchAction, e.PackageId);
            if (state != null)
                e.State = (RequestState) state;
        }

        private void Bootstrapper_PlanBegin(object sender, PlanBeginEventArgs e)
        {
            this.Log(LogLevel.Debug,
                $"Bootstrapper has called {nameof(this.Bootstrapper_PlanBegin)}");

            this.ExecuteOnDispatcherOrImmediateIfNotAvailable(
                this.PlanBegin);
        }

        /// <summary>
        ///     Called when the bootstrapper begins planning the installation
        /// </summary>
        protected abstract void PlanBegin();


        private void Bootstrapper_Shutdown(object sender, ShutdownEventArgs e)
        {
            this.Log(LogLevel.Standard, "Bootstrapper shutting down");
            if (this.RestartConfirmed)
                e.Result = Result.Restart;
        }

        private void Bootstrapper_DetectRelatedBundle(object sender, DetectRelatedBundleEventArgs e)
        {
            this.Log(LogLevel.Standard,
                $"Bootstrapper_DetectRelatedBundle: Bundle {e.ProductCode}, operation: {e.Operation}");
            if (e.Operation == RelatedOperation.None)
            {
                this.Log(LogLevel.Error,
                    $"This version of {this.Bootstrapper.BundleName} is already installed");
                this.VersionStatus = VersionStatus.Current;
            }
            if (e.Operation == RelatedOperation.Downgrade)
            {
                this.Log(LogLevel.Error, $"A newer version of {this.Bootstrapper.BundleName} is already installed");
                this.VersionStatus = VersionStatus.NewerAlreadyInstalled;
            }
            if (e.Operation == RelatedOperation.MajorUpgrade || e.Operation == RelatedOperation.MinorUpdate)
            {
                this.Log(LogLevel.Standard,
                    $"Adding product {e.ProductCode} to list of bundles to update");
                this._bundlesToUpgrade.Add(e.ProductCode);
                this.VersionStatus = VersionStatus.OlderInstalled;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Bootstrapper_PlanRelatedBundle(object sender, PlanRelatedBundleEventArgs e)
        {
            foreach (var bundle in this._bundlesToUpgrade)
            {
                this.Log(LogLevel.Standard,
                    $"Check for upgrade: product code: {bundle}, bundleId = {e.BundleId}");
                if (string.CompareOrdinal(bundle, e.BundleId) == 0)
                {
                    this.Log(LogLevel.Standard, "setting bundle to Absent");
                    e.State = RequestState.Absent;
                }
            }
        }

        private void Bootstrapper_ApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            this.Log(LogLevel.Error,
                $"Bootstrapper ApplyComplete with status {e.Status:X8}, restart required: {e.Restart}, previous burn state: {this.BurnInstallationState}");
            lock (this)
            {
                if (e.Status >= 0 && this.LaunchAction == LaunchAction.UpdateReplace)
                {
                    this.Bootstrapper.Engine.Quit(this.Status);
                    return;
                }
                this.Status = e.Status;
                this.BurnInstallationState = this.Status >= 0
                    ? BurnInstallationState.Applied
                    : BurnInstallationState.Failed;
                this.RestartRequired = e.Restart == ApplyRestart.RestartRequired ||
                                       e.Restart == ApplyRestart.RestartInitiated;
                this.ExecutedAction = this.LaunchAction;
                if (this.IsInteractive)
                    this.ExecuteOnDispatcher(this.TransitionToFinishPhase);
                else
                {
                    this.Bootstrapper.Engine.Quit(this.Status);
                }
            }
        }

        protected abstract void TransitionToFinishPhase();

        protected void ShutDownWithCancelCode()
        {
            this.Bootstrapper.Engine.Quit(CancelErrorCode);
        }

        public void RequestCancellation()
        {
            switch (this.BurnInstallationState)
            {
                case BurnInstallationState.Initializing:
                case BurnInstallationState.Detected:
                case BurnInstallationState.Failed:
                case BurnInstallationState.DetectedNewer:
                case BurnInstallationState.Applied:
                    this.CancelButtonPressed = true;
                    if (this.ShowCancelDialog() == MessageBoxResult.Yes)
                    {
                        this.ShouldCancel = true;
                        this.ShutDownWithCancelCode();
                    }
                    else
                    {
                        this.CancelButtonPressed = false;
                    }
                    break;
                case BurnInstallationState.Applying:
                    this.CancelButtonPressed = true;
                    var result = this.ShowCancelDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        this.ShouldCancel = true;
                    }
                    else
                    {
                        this.CancelButtonPressed = false;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected abstract MessageBoxResult ShowCancelDialog();

        public void Log(LogLevel level, string message)
        {
            this.Bootstrapper.Engine.Log(level, message);
        }
    }
}