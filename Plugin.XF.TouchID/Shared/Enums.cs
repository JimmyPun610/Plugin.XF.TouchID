using System;

namespace Plugin.XF.TouchID
{
    public enum TouchIDStatus
    {
        Support = 0,
        DeviceNotSecured = 1,
        NotEnrolledFinger = 2,
        HardwareNotSupport = 3,
        OSVersionNotSupport = 4,
    }
}
