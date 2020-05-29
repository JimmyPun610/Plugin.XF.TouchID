using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.XF.TouchID
{
    public class FingerprintDialogConfiguration
    {
        public string FingerprintHintString = "Touch Sensor";
        public string FingerprintNotRecoginzedString = "Not recoginzed, please try again later";

        public FingerprintDialogConfiguration()
        {

        }

        public FingerprintDialogConfiguration(string hint, string failedText)
        {
            this.FingerprintHintString = hint;
            this.FingerprintNotRecoginzedString = failedText;
        }
    }
}
