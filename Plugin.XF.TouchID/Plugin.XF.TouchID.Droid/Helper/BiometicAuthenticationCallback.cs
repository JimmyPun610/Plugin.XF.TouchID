using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Hardware.Biometrics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Security;
using Xamarin.Forms;

namespace Plugin.XF.TouchID.Helper
{
    class BiometicAuthenticationCallback : BiometricPrompt.AuthenticationCallback
    {
        public override void OnAuthenticationFailed()
        {
            base.OnAuthenticationFailed();
            MessagingCenter.Send<string, string>(TouchID.Abstractions.TouchID.FingerprintAuthentication, TouchID.Abstractions.TouchID.Authentication, Abstractions.TouchID.Error);
        }

        public override void OnAuthenticationHelp([GeneratedEnum] BiometricAcquiredStatus helpCode, ICharSequence helpString)
        {
            base.OnAuthenticationHelp(helpCode, helpString);
        }

        public override void OnAuthenticationSucceeded(BiometricPrompt.AuthenticationResult result)
        {
            base.OnAuthenticationSucceeded(result);
            MessagingCenter.Send<string, string>(TouchID.Abstractions.TouchID.FingerprintAuthentication, TouchID.Abstractions.TouchID.Authentication, Abstractions.TouchID.Success);
        }
    }
}