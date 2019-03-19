using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V4.Hardware.Fingerprint;
using Android.Views;
using Android.Widget;
using Plugin.XF.TouchID.Droid.Helper;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Plugin.XF.TouchID
{
    public class XFTouchIDImplementation : TouchID.Abstractions.TouchID
    {
        public XFTouchIDImplementation()
        {

        }
        Android.Support.V4.OS.CancellationSignal cancellationSignal = new Android.Support.V4.OS.CancellationSignal();

        public override bool IsHardwareDetected()
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
        public override bool IsDeviceSecured()
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
        public override bool IsFingerPrintEnrolled()
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
        public override bool IsPermissionGranted()
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="descrptionMessage">Will show on the touch ID authenticate dialog</param>
        /// <param name="successAction">Action will take if touch ID correct</param>
        /// <param name="failedAction">Action for touch ID not correct, No use in iOS generally</param>
        /// <param name="errorAction">Action for touch ID not correct many times, No use in iOS generally</param>
        public override async Task Authenticate(string descrptionMessage, Action successAction = null, Action failedAction = null, Action errorAction = null)
        {
            var popupDialog1 = new Plugin.XF.TouchID.Abstractions.BiometricAuthenticationPopup(Configuration.PromptTitle, descrptionMessage, Configuration.PromptNegativeMessage,
             () => {

             });
            popupDialog1.ExitAction = () =>
            {
                PopupNavigation.Instance.PopAllAsync();
            };
            await PopupNavigation.Instance.PushAsync(popupDialog1);


            return;


            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(Android.App.Application.Context);
                const int flags = 0; /* always zero (0) */
                CryptoObjectHelper cryptoHelper = new CryptoObjectHelper();
                // Using the Support Library classes for maximum reach
                FingerprintManagerCompat fingerPrintManager = FingerprintManagerCompat.From(Android.App.Application.Context);
                // AuthCallbacks is a C# class defined elsewhere in code.
                FingerprintManagerCompat.AuthenticationCallback authenticationCallback = new AuthenticationCallBack();

                var popupDialog = new Plugin.XF.TouchID.Abstractions.BiometricAuthenticationPopup(Configuration.PromptTitle, descrptionMessage, Configuration.PromptNegativeMessage,
                    ()=> {

                    });


                await PopupNavigation.Instance.PushAsync(popupDialog);
                MessagingCenter.Subscribe<string, string>(Abstractions.TouchID.FingerprintAuthentication, Abstractions.TouchID.Authentication, (sender, arg) =>
                {
                    string result = arg;
                    if (arg == Abstractions.TouchID.Success)
                    {
                        SuccessAction?.Invoke();
                    }
                    else if (arg == Abstractions.TouchID.Failed)
                    {
                        FailedAction?.Invoke();
                    }
                    else
                    {
                        ErrorAction?.Invoke();
                    }
                    MessagingCenter.Unsubscribe<string>(Abstractions.TouchID.FingerprintAuthentication, Abstractions.TouchID.Authentication);
                });
                cancellationSignal = new Android.Support.V4.OS.CancellationSignal();
                // Here is where the CryptoObjectHelper builds the CryptoObject. 
                fingerprintManager.Authenticate(cryptoHelper.BuildCryptoObject(), flags, cancellationSignal, authenticationCallback, null);
            }
          
            //if ((int)Build.VERSION.SdkInt >= 28)
            //{
            //    //Android 9+
            //    //var biometricPrompt = new BiometricPrompt
            //    //    .Builder(Configuration.Context)
            //    //    .SetTitle(Configuration.PromptTitle)
            //    //    .SetDescription(descrptionMessage)
            //    //    .SetNegativeButton(Configuration.PromptNegativeMessage, Configuration.Context.MainExecutor, new NegativeButtonOnClickListener(()=>
            //    //    {
            //    //        ManagerIdentitfyCallback
            //    //    }))
            //    //    .Build();
            //    //BiometricPrompt.AuthenticationCallback authenticationCallback = 


            //}
            //else if((int)Build.VERSION.SdkInt >= 23)
            //{
            //    //Android 6+

                


               
            //}

        
        }
        public override void CancelCurrentAuthentication()
        {
            cancellationSignal.Cancel();
         }
        private bool isAndroidVersionSupport()
        {
            if (Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                return true;
            }
            else return false;
        }
        public override Abstractions.TouchIDAvailabilities IsFingerprintAuthenticationPossible()
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
                            return Abstractions.TouchIDAvailabilities.Support;
                        }
                        else
                        {
                            return Abstractions.TouchIDAvailabilities.NotEnrolledFinger;
                        }
                    }
                    else
                    {
                        return Abstractions.TouchIDAvailabilities.DeviceNotSecured;
                    }
                }
                else
                {
                    return Abstractions.TouchIDAvailabilities.HardwareNotSupport;
                }
            }
            else
            {
                return Abstractions.TouchIDAvailabilities.OSVersionNotSupport;
            }

        }
        public override void PromptSecuritySettings()
        {
            Forms.Context.StartActivity(new Intent(Android.Provider.Settings.ActionSecuritySettings));
        }


    }
}