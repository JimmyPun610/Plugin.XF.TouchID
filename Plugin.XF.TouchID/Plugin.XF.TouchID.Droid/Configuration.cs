using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace Plugin.XF.TouchID
{
    public class Configuration
    {
        public static string PromptNegativeMessage = "Cancel";
        public static Action PromptNegativeAction;
        public static string PromptPositiveMessage = "Use Password";
        public static Action PromptPositiveAction;
        public static string PromptTitle = "Biometric Authentication";
        public static string DefaultAuthenticationMessage = "Please do the authentication for further action";
        public static string FingerprintFailedText = "Please try again";
        public static string FingerprintErrorText = "Too many failed attempts, please wait 30s to retry";
        public static string PasscodeAuthTitle = "Passcode authentication";
        public static string PasscodeAuthDesc = "Please input passcode to continue";
        public static Activity Activity { get; set; }
        public const int KeyguardManagerRequestCode = 2034;

        public static Color PopupTitleColor = Color.Blue;
        public static Color PopupBackgroundColor = Color.White;
        public static Color PopupDescriptionColor = Color.Black;
        public static Color PopupNegativeTextColor = Color.Red;
        public static Color PopupPositiveTextColor = Color.Black;


        public static void OnKeyguardManagerResult(Intent intent, int requestCode, Result resultCode) {
            if (resultCode == Result.Ok)
                XFTouchIDImplementation.AuthenticationResult?.Invoke(XFTouchIDImplementation.Success);
        }
    }
}