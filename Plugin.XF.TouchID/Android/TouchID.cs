using System;
using System.Collections.Generic;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Hardware.Biometrics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V4.Hardware.Fingerprint;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Plugin.XF.TouchID
{
    public static partial class TouchID
    {

        /// <summary>
        /// Do not use secret key authentication
        /// </summary>
        /// <param name="currentActivity"></param>
        /// <param name="allowAlternativeAuth">Use alternative authentication methods (passcode / pin etc), can setup custom negative action if set it false</param>
        public static void Init(Activity currentActivity)
        {
            Configuration.CurrentActivity = currentActivity;
            Configuration.IsUseSecretKey = false;
            Configuration.KeyName = string.Empty;

        }
        /// <summary>
        /// To use secret key in authentication, please use this init method and provide unique key name
        /// </summary>
        /// <param name="currentActivity"></param>
        /// <param name="keyName">Unique key name for the app</param>
        /// <param name="allowAlternativeAuth">Use alternative authentication methods (passcode / pin etc), can setup custom negative action if set it false</param>
        public static void Init(Activity currentActivity, string keyName)
        {
            Configuration.CurrentActivity = currentActivity;
            Configuration.IsUseSecretKey = true;
            Configuration.KeyName = keyName;

        }

        internal static Action<string> AuthenticationResult;
        /// <summary>
        /// Return result from Keyguard set in Android
        /// protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        public static void OnKeyguardManagerResult(Intent intent, int requestCode, Result resultCode)
        {
            if (resultCode == Result.Ok)
                AuthenticationResult?.Invoke(SystemMessages.Success);
        }

        public static bool IsHardwareDetected()
        {
            FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(Android.App.Application.Context);
            if (!fingerprintManager.IsHardwareDetected)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool IsDeviceSecured()
        {
            KeyguardManager keyguardManager = (KeyguardManager)Android.App.Application.Context.GetSystemService(Android.App.Application.KeyguardService);
            if (!keyguardManager.IsKeyguardSecure)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool IsFingerPrintEnrolled()
        {
            FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(Android.App.Application.Context);
            if (!fingerprintManager.HasEnrolledFingerprints)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool IsPermissionGranted()
        {
            Android.Content.PM.Permission permissionResult = ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.UseFingerprint);
            if (permissionResult == Android.Content.PM.Permission.Granted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #region Android 9+ Helper
        class NegativeButtonOnClickListener : Java.Lang.Object, IDialogInterfaceOnClickListener
        {
            Action CancelAction;
            public NegativeButtonOnClickListener(Action cancelAction)
            {
                CancelAction = cancelAction;
            }
            public void OnClick(IDialogInterface dialog, int which)
            {
                CancelAction?.Invoke();
            }
        }
        #endregion

        public static async System.Threading.Tasks.Task Authenticate(DialogConfiguration dialogConfiguration)
        {

            //Android 10+ Q SdkInt 29+
            if ((int)Build.VERSION.SdkInt >= 29)
            {
                CancellationSignal BiometricCancellationSignal = new CancellationSignal();
                //Android 10
                BiometricCancellationSignal = new CancellationSignal();
                BiometricPrompt biometricPrompt;
                if (dialogConfiguration.IsUseAlternativeAuthentication)
                {
                    //Allow using PIN / Passcode as alternative, will disable crypto in authentication
                    biometricPrompt = new BiometricPrompt
                       .Builder(Configuration.CurrentActivity)
                       .SetTitle(dialogConfiguration.DialogTitle)
                       .SetDescription(dialogConfiguration.DialogDescription)
                       .SetDeviceCredentialAllowed(true)
                       .Build();
                }
                else
                {
                    biometricPrompt = new BiometricPrompt
                                          .Builder(Configuration.CurrentActivity)
                                          .SetTitle(dialogConfiguration.DialogTitle)
                                          .SetDescription(dialogConfiguration.DialogDescription)
                                          .SetNegativeButton(dialogConfiguration.AlternativeActionMessage, Configuration.CurrentActivity.MainExecutor, new NegativeButtonOnClickListener(() =>
                                          {
                                              BiometricCancellationSignal.Cancel();
                                              dialogConfiguration.AlternativeAction?.Invoke();
                                          }))
                                          .Build();
                }
                BiometricPrompt.AuthenticationCallback authenticationCallback = new BiometricAuthenticationCallback();
                AuthenticationResult = null;
                AuthenticationResult = (arg) =>
                {
                    string result = arg;
                    if (arg == SystemMessages.Success)
                    {
                        BiometricCancellationSignal.Cancel();
                        dialogConfiguration.SuccessAction?.Invoke();
                    }
                    else if (arg == SystemMessages.Error)
                    {
                        BiometricCancellationSignal.Cancel();
                        dialogConfiguration.FailedAction?.Invoke();
                    }
                };
                if (dialogConfiguration.IsUseAlternativeAuthentication)
                {
                    //Secret key is not allowed to use when SetDeviceCredentialAllowed is true
                    biometricPrompt.Authenticate(BiometricCancellationSignal, Configuration.CurrentActivity.MainExecutor, authenticationCallback);
                }
                else
                {
                    if (Configuration.IsUseSecretKey)
                    {
                        CryptoObjectHelper cryptoHelper = new CryptoObjectHelper();
                        biometricPrompt.Authenticate(cryptoHelper.BuildBiometricPromptCryptoObject(), BiometricCancellationSignal, Configuration.CurrentActivity.MainExecutor, authenticationCallback);
                    }
                    else biometricPrompt.Authenticate(BiometricCancellationSignal, Configuration.CurrentActivity.MainExecutor, authenticationCallback);
                }
            }
            //Android 9+ P SdkInt 28
            else if ((int)Build.VERSION.SdkInt >= 28)
            {
                //Android 9
                try
                {
                    CancellationSignal BiometricCancellationSignal = new CancellationSignal();
                    BiometricPrompt biometricPrompt;
                    biometricPrompt = new BiometricPrompt
                                           .Builder(Configuration.CurrentActivity)
                                           .SetTitle(dialogConfiguration.DialogTitle)
                                           .SetDescription(dialogConfiguration.DialogDescription)
                                           .SetNegativeButton(dialogConfiguration.AlternativeActionMessage, Configuration.CurrentActivity.MainExecutor, new NegativeButtonOnClickListener(() =>
                                           {
                                               BiometricCancellationSignal.Cancel();
                                               dialogConfiguration.AlternativeAction?.Invoke();
                                           }))
                                           .Build();

                    BiometricCancellationSignal = new CancellationSignal();

                    BiometricPrompt.AuthenticationCallback authenticationCallback = new BiometricAuthenticationCallback();
                    AuthenticationResult = null;
                    AuthenticationResult = (arg) =>
                    {
                        string result = arg;
                        if (arg == SystemMessages.Success)
                        {
                            BiometricCancellationSignal.Cancel();
                            dialogConfiguration.SuccessAction?.Invoke();
                        }
                        else if (arg == SystemMessages.Error)
                        {
                            BiometricCancellationSignal.Cancel();
                            dialogConfiguration.FailedAction?.Invoke();
                        }

                    };
                    if (Configuration.IsUseSecretKey)
                    {
                        CryptoObjectHelper cryptoHelper = new CryptoObjectHelper();
                        biometricPrompt.Authenticate(cryptoHelper.BuildBiometricPromptCryptoObject(), BiometricCancellationSignal, Configuration.CurrentActivity.MainExecutor, authenticationCallback);
                    }
                    else biometricPrompt.Authenticate(BiometricCancellationSignal, Configuration.CurrentActivity.MainExecutor, authenticationCallback);
                }
                catch (Exception ex)
                {
                    Configuration.CurrentActivity.RunOnUiThread(() =>
                    {
                        Toast.MakeText(Configuration.CurrentActivity, $"Throw exception : {ex.Message}", ToastLength.Long).Show();
                    });
                }

            }
            //Android 6+ to 8.x SdkInt 23
            else if ((int)Build.VERSION.SdkInt >= 23)
            {
                //Android 6+ to 8.x
                FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(Configuration.CurrentActivity);
                const int flags = 0; /* always zero (0) */
                CryptoObjectHelper cryptoHelper = new CryptoObjectHelper();
                // Using the Support Library classes for maximum reach
                FingerprintManagerCompat fingerPrintManager = FingerprintManagerCompat.From(Configuration.CurrentActivity);
                // AuthCallbacks is a C# class defined elsewhere in code.

                Android.Support.V4.OS.CancellationSignal FingerprintCancellationSignal = new Android.Support.V4.OS.CancellationSignal();
                FingerprintManagerCompat.AuthenticationCallback authenticationCallback = new AuthenticationCallBack();

                Android.Support.V4.App.FragmentTransaction transcation = ((FragmentActivity)Configuration.CurrentActivity).SupportFragmentManager.BeginTransaction();
                FingerprintDialog fingerprintDialog = new FingerprintDialog(dialogConfiguration, authenticationCallback, FingerprintCancellationSignal);
                fingerprintDialog.Show(transcation, "Dialog Fragment");

                AuthenticationResult = null;
                AuthenticationResult = (arg) =>
                {
                    string result = arg;
                    if (arg == SystemMessages.Success)
                    {
                        FingerprintCancellationSignal.Cancel();
                        dialogConfiguration.SuccessAction?.Invoke();
                    }
                    else if (arg == SystemMessages.Error)
                    {
                        FingerprintCancellationSignal.Cancel();
                        dialogConfiguration.FailedAction?.Invoke();
                    }
                };
                try
                {
                    fingerprintManager.Authenticate(cryptoHelper.BuildFingerprintManagerCompatCryptoObject(), flags, FingerprintCancellationSignal, authenticationCallback, null);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Configuration.CurrentActivity, $"Throw exception : {ex.Message}", ToastLength.Long);
                }
            }
        }
        internal static void PromptKeyguardManagerAuth(string title, string desc)
        {
            //BuildVersionCodes.Q Android 10
            if ((int)Build.VERSION.SdkInt < 29)
            {
                KeyguardManager km = (KeyguardManager)Configuration.CurrentActivity.GetSystemService(Activity.KeyguardService);
                if (km.IsKeyguardSecure)
                {
                    Intent authIntent = km.CreateConfirmDeviceCredentialIntent(title, desc);
                    Configuration.CurrentActivity.StartActivityForResult(authIntent, Configuration.KeyguardManagerRequestCode);
                }
            }
        }
        private static bool isAndroidVersionSupport()
        {
            //BuildVersionCodes.M Android 6+
            if ((int)Android.OS.Build.VERSION.SdkInt >= 23)
            {
                return true;
            }
            else return false;
        }
        public static TouchIDStatus IsFingerprintAuthenticationPossible()
        {
            if (isAndroidVersionSupport())
            {
                bool isHardwareSupport = IsHardwareDetected();
                if (isHardwareSupport)
                {
                    bool isDeviceSecured = IsDeviceSecured();
                    if (isDeviceSecured)
                    {
                        bool isFingerprintEnrolled = IsFingerPrintEnrolled();
                        if (isFingerprintEnrolled)
                        {
                            return TouchIDStatus.Support;
                        }
                        else
                        {
                            return TouchIDStatus.NotEnrolledFinger;
                        }
                    }
                    else
                    {
                        return TouchIDStatus.DeviceNotSecured;
                    }
                }
                else
                {
                    return TouchIDStatus.HardwareNotSupport;
                }
            }
            else
            {
                return TouchIDStatus.OSVersionNotSupport;
            }

        }
        public static void PromptSecuritySettings()
        {
            Configuration.CurrentActivity.StartActivity(new Intent(Android.Provider.Settings.ActionSecuritySettings));
        }

    }
}
