using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.XF.TouchID.Abstractions
{
    public enum TouchIDAvailabilities
    {
        Support = 0,
        DeviceNotSecured = 1,
        NotEnrolledFinger = 2,
        HardwareNotSupport = 3,
        OSVersionNotSupport = 4,
    }
    public interface ITouchID
    {
        bool IsHardwareDetected();
        bool IsDeviceSecured();
        bool IsFingerPrintEnrolled();
        bool IsPermissionGranted();
        Task Authenticate(string descrptionMessage, Action successAction = null, Action failedAction = null, Action errorAction = null);
        void CancelCurrentAuthentication();
        TouchIDAvailabilities IsFingerprintAuthenticationPossible();
        void PromptSecuritySettings();

    }
}
