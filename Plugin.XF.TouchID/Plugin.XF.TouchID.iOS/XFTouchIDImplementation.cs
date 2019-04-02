using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using LocalAuthentication;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Plugin.XF.TouchID
{
    public class XFTouchIDImplementation : TouchID.Abstractions.TouchID
    {
        public XFTouchIDImplementation()
        {

        }
        LAContext _context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="descrptionMessage">Will show on the touch ID authenticate dialog</param>
        /// <param name="successAction">Action will take if touch ID correct</param>
        public override async Task Authenticate(string descrptionMessage, Action successAction = null)
        {
            NSError AuthError;
            _context = new LAContext();
            if (_context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError))
            {
 
                if (string.IsNullOrWhiteSpace(descrptionMessage))
                    descrptionMessage = iOS.Configuration.DefaultAuthenticationMessage;
                Tuple<bool, NSError> result = await _context.EvaluatePolicyAsync(LAPolicy.DeviceOwnerAuthentication, descrptionMessage);
                bool success = result.Item1;
                NSError error = result.Item2;
                if (result.Item1 == true)
                    successAction?.Invoke();
                else
                {
                    if(error.Code == -2)
                    {
                        //User cancel
                        return;
                    }
                        
                    //if (error.LocalizedDescription == "Fallback authentication mechanism selected.")
                    //{
                    //    var s = error.Code;
                    //    result = await _context.EvaluatePolicyAsync(LAPolicy.DeviceOwnerAuthentication, descrptionMessage);
                    //    if (result.Item1)
                    //        successAction?.Invoke();
                    //}
                }
            }
            else
            {
                
                _context.InvokeOnMainThread(async () =>
                {
                    var result = await _context.EvaluatePolicyAsync(LAPolicy.DeviceOwnerAuthentication, iOS.Configuration.DefaultFailAttemptNumberExceededMsg);
                    if (result.Item1)
                        successAction?.Invoke();
                });
                
                
            }
        }

        public override void PromptPasscodeAuth()
        {
            
        }

        public override bool IsDeviceSecured()
        {
            NSError error = null;
            bool isDeviceSecured = _context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, out error);
            return isDeviceSecured;
        }

        public override Abstractions.TouchIDAvailabilities IsFingerprintAuthenticationPossible()
        {
            if (isIOSVersionSupportFingerprint())
            {
                if (IsHardwareDetected())
                {
                    if (IsDeviceSecured())
                    {
                        if (IsFingerPrintEnrolled())
                        {
                            return  Abstractions.TouchIDAvailabilities.Support;
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
                    //Action if hardware not support
                    return Abstractions.TouchIDAvailabilities.HardwareNotSupport;
                }
            }
            else
            {
                //Action if OS not support
                return Abstractions.TouchIDAvailabilities.OSVersionNotSupport;
            }
        }
        private bool isIOSVersionSupportFingerprint()
        {
            return UIDevice.CurrentDevice.CheckSystemVersion(8, 0);
        }
        public override bool IsFingerPrintEnrolled()
        {
            NSError error = null;
            bool isDeviceSecured = _context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out error);
            return isDeviceSecured;
        }

        public override bool IsHardwareDetected()
        {
            string model = UIDevice.CurrentDevice.Model;
            string deviceVersion = DeviceInfo.Model;
            if (deviceVersion.ToLower().Contains("ipad"))
            {
                List<string> nonSupportediPadList = new List<string>
                {
                 "ipad1,1","ipad2,1","ipad2,2","ipad2,3","ipad2,4","ipad2,5","ipad2,6","ipad2,7","ipad3,1","ipad3,2","ipad3,3",
                    "ipad3,4","ipad3,5","ipad3,6","ipad4,1","ipad4,2","ipad4,3","ipad4,4","ipad4,5","ipad4,6"
                };
                if (nonSupportediPadList.Contains(deviceVersion.ToLower()))
                {
                    return false;
                }
            }
            else if (deviceVersion.ToLower().Contains("iphone"))
            {
                string[] versionName = deviceVersion.Split(',');
                var charArray = versionName.FirstOrDefault().ToCharArray();
                int versionNumber = int.Parse(charArray[charArray.Length - 1].ToString());
                if (versionNumber <= 5)
                    return false;
            }

            return true;
        }

        public override bool IsPermissionGranted()
        {
            return true;
        }
        public override void PromptSecuritySettings()
        {
            NSUrl url = new NSUrl("App-Prefs:root=TOUCHID_PASSCODE");
            if (UIApplication.SharedApplication.CanOpenUrl(url))
            {
                UIApplication.SharedApplication.OpenUrl(url);
            }
        }

    }
}