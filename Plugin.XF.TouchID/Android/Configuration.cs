using Android.App;
using Android.Content;
using Org.Apache.Http.Impl.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.XF.TouchID
{
    internal static class Configuration
    {
        public const int KeyguardManagerRequestCode = 2034;
        public static Activity CurrentActivity { get; set; }
        
        public static bool IsUseSecretKey = true;
        /// <summary>
        /// This can be key name you want. Should be unique for the app.
        /// </summary>
        public static string KeyName = "";
        

    }
}
