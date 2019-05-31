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

namespace Plugin.XF.TouchID.Helper
{
    public class BiometricAuthenticationCallback : BiometricPrompt.AuthenticationCallback
    {
        public Action<BiometricPrompt.AuthenticationResult> Success;
        public Action Failed;
        public Action<BiometricAcquiredStatus, ICharSequence> Help;



        public override void OnAuthenticationSucceeded(BiometricPrompt.AuthenticationResult result)
        {
            base.OnAuthenticationSucceeded(result);
            if(Success != null)
                Success(result);
            XFTouchIDImplementation.AuthenticationResult?.Invoke(Abstractions.TouchID.Success);
        }

        public override void OnAuthenticationFailed()
        {
            base.OnAuthenticationFailed();
            if(Failed != null)
                Failed();
            XFTouchIDImplementation.AuthenticationResult?.Invoke(Abstractions.TouchID.Failed);
        }

        public override void OnAuthenticationHelp([GeneratedEnum] BiometricAcquiredStatus helpCode, ICharSequence helpString)
        {
            base.OnAuthenticationHelp(helpCode, helpString);
            if (Help != null)
                Help(helpCode, helpString);
            XFTouchIDImplementation.AuthenticationResult?.Invoke(Abstractions.TouchID.Error);
        }
    }
}