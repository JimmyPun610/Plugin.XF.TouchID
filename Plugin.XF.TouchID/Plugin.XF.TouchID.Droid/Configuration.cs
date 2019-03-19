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

namespace Plugin.XF.TouchID
{
    public class Configuration
    {
        public static string PromptNegativeMessage = "Use Password";
        public static string PromptTitle = "Biometric Authentication";
        public static string DefaultAuthenticationMessage = "Please do the authentication for further action";
    }
}