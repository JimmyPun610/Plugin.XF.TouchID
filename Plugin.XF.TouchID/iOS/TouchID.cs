using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Foundation;
using LocalAuthentication;
using UIKit;

namespace Plugin.XF.TouchID
{
    public static partial class TouchID
    {
        static LAContext _context;

        public static void Init() { }

        public static bool IsHardwareDetected()
        {
            string model = UIDevice.CurrentDevice.Model;
      
            if (model.ToLower().Contains("ipad"))
            {
                List<string> nonSupportediPadList = new List<string>
                {
                 "ipad1,1","ipad2,1","ipad2,2","ipad2,3","ipad2,4","ipad2,5","ipad2,6","ipad2,7","ipad3,1","ipad3,2","ipad3,3",
                    "ipad3,4","ipad3,5","ipad3,6","ipad4,1","ipad4,2","ipad4,3","ipad4,4","ipad4,5","ipad4,6"
                };
                if (nonSupportediPadList.Contains(model.ToLower()))
                {
                    return false;
                }
            }
            else if (model.ToLower().Contains("iphone"))
            {
                Regex regex = new System.Text.RegularExpressions.Regex("(.[1-9]),");
                string versionNumberStr = regex.Match(model).Groups.LastOrDefault()?.Value;
                double versionNumber = 0;
                if (!string.IsNullOrWhiteSpace(versionNumberStr))
                    double.TryParse(versionNumberStr, out versionNumber);
                if (versionNumber <= 5)
                    return false;
            }

            return true;
        }

        internal static void PromptKeyguardManagerAuth(string title, string desc)
        {
 
        }

        public static bool IsDeviceSecured()
        {
            NSError error = null;
            _context = new LAContext();
            bool isDeviceSecured = _context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, out error);
            return isDeviceSecured;
        }
        public static bool IsFingerPrintEnrolled()
        {
            NSError error = null;
            bool isDeviceSecured = _context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out error);
            return isDeviceSecured;
        }
        public static bool IsPermissionGranted()
        {
            return true;
        }

        public static async System.Threading.Tasks.Task Authenticate(DialogConfiguration dialogConfiguration)
        {
            _context = new LAContext();
            _context.InvokeOnMainThread(async () =>
            {
                
                var result = await _context.EvaluatePolicyAsync(LAPolicy.DeviceOwnerAuthentication, Configuration.DefaultFailAttemptNumberExceededMsg);
                bool success = result.Item1;
                NSError error = result.Item2;
                if (success)
                    dialogConfiguration.SuccessAction?.Invoke();
                else
                {
                    if(error.Code == -2)
                    {
                        //user cancel
                        return;
                    }else if(error.Code == -1) {
                        //too many failed attempt
                        dialogConfiguration.FailedAction?.Invoke();
                    }

                }
            });
            //NSError AuthError;

            //if (_context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError))
            //{
            //    Tuple<bool, NSError> result = await _context.EvaluatePolicyAsync(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, dialogConfiguration.DialogDescription);
            //    bool success = result.Item1;
            //    NSError error = result.Item2;
            //    if (result.Item1 == true)
            //        dialogConfiguration.SuccessAction?.Invoke();
            //    else
            //    {
            //        if (error.Code == -2)
            //        {
            //            //User cancel
            //            return;
            //        }else if(error.Code == -1)
            //        {

            //        }

            //        //if (error.LocalizedDescription == "Fallback authentication mechanism selected.")
            //        //{
            //        //    var s = error.Code;
            //        //    result = await _context.EvaluatePolicyAsync(LAPolicy.DeviceOwnerAuthentication, descrptionMessage);
            //        //    if (result.Item1)
            //        //        successAction?.Invoke();
            //        //}
            //    }
            //}
            //else
            //{

            //    _context.InvokeOnMainThread(async () =>
            //    {
            //        var result = await _context.EvaluatePolicyAsync(LAPolicy.DeviceOwnerAuthentication, Configuration.DefaultFailAttemptNumberExceededMsg);
            //        if (result.Item1)
            //            dialogConfiguration.SuccessAction?.Invoke();
            //    });


            //}
        }
        private static bool isIOSVersionSupportFingerprint()
        {
            return UIDevice.CurrentDevice.CheckSystemVersion(8, 0);
        }
        public static TouchIDStatus IsFingerprintAuthenticationPossible()
        {
            if (isIOSVersionSupportFingerprint())
            {
                if (IsHardwareDetected())
                {
                    if (IsDeviceSecured())
                    {
                        if (IsFingerPrintEnrolled())
                        {
                            return TouchIDStatus.Support;
                        }
                        else
                        {
                            return TouchIDStatus.NotEnrolledFinger;
                        }
                    }
                    else
                    {
                        return TouchIDStatus.DeviceNotSecured;
                    }
                }
                else
                {
                    //Action if hardware not support
                    return TouchIDStatus.HardwareNotSupport;
                }
            }
            else
            {
                //Action if OS not support
                return TouchIDStatus.OSVersionNotSupport;
            }
        }
        public static void PromptSecuritySettings()
        {
            NSUrl url = new NSUrl("App-Prefs:root=TOUCHID_PASSCODE");
            if (UIApplication.SharedApplication.CanOpenUrl(url))
            {
                UIApplication.SharedApplication.OpenUrl(url);
            }
        }
     
    }
}
