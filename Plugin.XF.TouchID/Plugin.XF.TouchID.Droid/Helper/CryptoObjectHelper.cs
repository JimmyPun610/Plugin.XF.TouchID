using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Security.Keystore;
using Java.Security;
using Android.Support.V4.Hardware.Fingerprint;
using Javax.Crypto;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Hardware.Biometrics;

namespace Plugin.XF.TouchID.Droid.Helper
{
    public class CryptoObjectHelper
    {
        // This can be key name you want. Should be unique for the app.
        static readonly string KEY_NAME = "com.android.fingerprint_authentication_key";

        // We always use this keystore on Android.
        static readonly string KEYSTORE_NAME = "AndroidKeyStore";

        // Should be no need to change these values.
        static readonly string KEY_ALGORITHM = KeyProperties.KeyAlgorithmAes;
        static readonly string BLOCK_MODE = KeyProperties.BlockModeCbc;
        static readonly string ENCRYPTION_PADDING = KeyProperties.EncryptionPaddingPkcs7;
        static readonly string TRANSFORMATION = KEY_ALGORITHM + "/" +
                                                BLOCK_MODE + "/" +
                                                ENCRYPTION_PADDING;
        readonly KeyStore _keystore;
    
        public CryptoObjectHelper()
        {
      
            _keystore = KeyStore.GetInstance(KEYSTORE_NAME);
            _keystore.Load(null);
        }
        public BiometricPrompt.CryptoObject BuildBiometricPromptCryptoObject()
        {
            Cipher cipher = CreateCipher();
            return new BiometricPrompt.CryptoObject(cipher);
        }


        public FingerprintManagerCompat.CryptoObject BuildFingerprintManagerCompatCryptoObject()
        {
            Cipher cipher = CreateCipher();
            return new FingerprintManagerCompat.CryptoObject(cipher);
        }

        Cipher CreateCipher(bool retry = true)
        {
            IKey key = GetKey();
            Cipher cipher = Cipher.GetInstance(TRANSFORMATION);
            try
            {
                cipher.Init(CipherMode.EncryptMode, key);
            }
            catch (KeyPermanentlyInvalidatedException e)
            {
                _keystore.DeleteEntry(KEY_NAME);
                if (retry)
                {
                    CreateCipher(false);
                }
                else
                {
                    throw new Exception("Could not create the cipher for fingerprint authentication.", e);
                }
            }
            return cipher;
        }

        IKey GetKey()
        {
            IKey secretKey;
            try
            {
                secretKey = _keystore.GetKey(KEY_NAME, null);
            }catch(Exception ex)
            {
                CreateKey();
            }
            secretKey = _keystore.GetKey(KEY_NAME, null);
            return secretKey;
        }

        void CreateKey()
        {

          


            KeyGenerator keyGen = KeyGenerator.GetInstance(KeyProperties.KeyAlgorithmAes, KEYSTORE_NAME);
            KeyGenParameterSpec keyGenSpec =
                new KeyGenParameterSpec.Builder(KEY_NAME, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt)
                    .SetBlockModes(BLOCK_MODE)
                    .SetEncryptionPaddings(ENCRYPTION_PADDING)
                    .SetUserAuthenticationRequired(true)
                    .Build();
            keyGen.Init(keyGenSpec);
            keyGen.GenerateKey();
        }

    }
}