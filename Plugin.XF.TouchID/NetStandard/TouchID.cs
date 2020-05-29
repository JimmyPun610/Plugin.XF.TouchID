using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.XF.TouchID
{
    public static partial class TouchID
    {

 

        public static bool IsHardwareDetected()
        {
          throw new NotImplementedException();
        }
        public static bool IsDeviceSecured()
        {
            throw new NotImplementedException();
        }
        public static bool IsFingerPrintEnrolled()
        {
            throw new NotImplementedException();
        }
        public static bool IsPermissionGranted()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Call for the authentication
        /// </summary>
        /// <param name="dialogConfiguration">Configuration of the dialog</param>
        public static async System.Threading.Tasks.Task Authenticate(DialogConfiguration dialogConfiguration)
        {
            throw new NotImplementedException();
        }
        public static TouchIDStatus IsFingerprintAuthenticationPossible()
        {
            throw new NotImplementedException();
        }
        public static void PromptSecuritySettings()
        {
            throw new NotImplementedException();
        }
        internal static void PromptKeyguardManagerAuth(string title, string desc)
        {
            throw new NotImplementedException();
        }
    }
}
