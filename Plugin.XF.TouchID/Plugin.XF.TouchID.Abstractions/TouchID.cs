using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.XF.TouchID.Abstractions
{
    public class TouchID : ITouchID
    {
        public const string FingerprintAuthentication = "FingerprintAuthentication";
        public const string Authentication = "Authentication";
        public const string Success = "Success";
        public const string Failed = "Failed";
        public const string Error = "Error";

        public Action SuccessAction;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="descrptionMessage">Will show on the touch ID authenticate dialog</param>
        /// <param name="successAction">Action will take if touch ID correct</param>
        public virtual async Task Authenticate(string descrptionMessage, Action successAction = null)
        {
            throw new NotImplementedException();
        }

        public virtual void PromptKeyguardManagerAuth()
        {
            throw new NotImplementedException();

        }



        public virtual bool IsDeviceSecured()
        {
            throw new NotImplementedException();
        }

        public virtual TouchIDAvailabilities IsFingerprintAuthenticationPossible()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsFingerPrintEnrolled()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsHardwareDetected()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsPermissionGranted()
        {
            throw new NotImplementedException();
        }

        public virtual void PromptSecuritySettings()
        {
            throw new NotImplementedException();
        }
    }
}
