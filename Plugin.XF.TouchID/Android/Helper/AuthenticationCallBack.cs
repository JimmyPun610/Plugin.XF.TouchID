using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Hardware.Fingerprint;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Javax.Crypto;


namespace Plugin.XF.TouchID
{
    public class AuthenticationCallBack : Android.Support.V4.Hardware.Fingerprint.FingerprintManagerCompat.AuthenticationCallback
    {
        // Can be any byte array, keep unique to application.
        static readonly byte[] SECRET_BYTES = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        // The TAG can be any string, this one is for demonstration.
        static readonly string TAG = "X:" + typeof(AuthenticationCallBack).Name;

        FingerprintDialog _fingerprintDialog;

        public AuthenticationCallBack()
        {
          
        }

        internal void SetFingerprintDialog(FingerprintDialog fingerprintDialog)
        {
            this._fingerprintDialog = fingerprintDialog;
        }

        public override void OnAuthenticationSucceeded(FingerprintManagerCompat.AuthenticationResult result)
        {
            bool isTrusted = false;
            if (result.CryptoObject.Cipher != null)
            {
                try
                {
                    // Calling DoFinal on the Cipher ensures that the encryption worked.
                    byte[] doFinalResult = result.CryptoObject.Cipher.DoFinal(SECRET_BYTES);

                    // No errors occurred, trust the results.    
                    isTrusted = true;
                }
                catch (BadPaddingException bpe)
                {
                    // Can't really trust the results.
                    Log.Error(TAG, "Failed to encrypt the data with the generated key." + bpe);
                }
                catch (IllegalBlockSizeException ibse)
                {
                    // Can't really trust the results.
                    Log.Error(TAG, "Failed to encrypt the data with the generated key." + ibse);
                }
            }
            else
            {
                // No cipher used, assume that everything went well and trust the results.
                isTrusted = true;
            }
            if (isTrusted)
            {
                Log.Info("Fingerprint", "Success");
                _fingerprintDialog?.DismissDialog();
                TouchID.AuthenticationResult?.Invoke(SystemMessages.Success);
            }

        }

        public override void OnAuthenticationError(int errMsgId, ICharSequence errString)
        {
            // Report the error to the user. Note that if the user canceled the scan,
            // this method will be called and the errMsgId will be FingerprintState.ErrorCanceled.
            if(errMsgId == 7)
            {
                Log.Error("Fingerprint", "Error in authentication." + errString + "ID:" + errMsgId);
                _fingerprintDialog?.DismissDialog();
                TouchID.AuthenticationResult?.Invoke(SystemMessages.Error);
            }
            
        }

        public override void OnAuthenticationFailed()
        {
            // Tell the user that the fingerprint was not recognized.
            Log.Info("Fingerprint", "Fail to authenicate.");
            _fingerprintDialog?.SetAuthenticationFailed();
            TouchID.AuthenticationResult?.Invoke(SystemMessages.Failed);
        }

        public override void OnAuthenticationHelp(int helpMsgId, ICharSequence helpString)
        {
            // Notify the user that the scan failed and display the provided hint.
        }
    }
}
