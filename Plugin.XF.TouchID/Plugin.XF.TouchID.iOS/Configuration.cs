using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Plugin.XF.TouchID.iOS
{
    public class Configuration
    {
        public static string DefaultAuthenticationMessage = "Please do the authentication for further action";
        public static string DefaultFailAttemptNumberExceededMsg = "Your passcode is required to enable biometric ID";
    }
}