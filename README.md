# Plugin.XF.TouchID
Please open the sample project for more information

Tested on iOS with Touch Id and Android 9 with Touch Id

# News on version 1.1
- Support on Android 9.0 with default biometric prompt
- Added configuration "Plugin.XF.TouchID.Droid.Configuration.IsUseSecretKey" to state using Cipher on Android 9, currently seem some error on it, please set it to false if the biometric did not prompt on Android 9
- Biomteric prompt on Android 9 only support one alternative action, define as PromptPositiveAction and PromptPositiveMessage
- Change Android library target framework to Android 9, please set app project to Android 9 too
# Nuget installation
- Install to your Xamarin Project
```
Install-Package Plugin.XF.TouchID -Version 1.1.0.4
```

# iOS Guide
1. Setting the prompt message first, there are default messages in English only.
```C#
   global::Xamarin.Forms.Forms.Init();
   LoadApplication(new App());
   Plugin.XF.TouchID.iOS.Configuration.DefaultAuthenticationMessage = "Set the default authenticate message";
   Pugin.XF.TouchID.iOS.Configuration.DefaultFailAttemptNumberExceededMsg = "Set the default failed attempt exceed msg";
   return base.FinishedLaunching(app, options);
```
2. In your plist, add face id permission request
```
<key>NSFaceIDUsageDescription</key>
<string>Need your face to unlock secrets!</string>
```
![](https://github.com/JimmyPun610/Plugin.XF.TouchID/blob/master/Plugin.XF.TouchID/Screenshots/iOS.jpg?raw=true")
# Android Guide
1. In MainActivity
```C#
   global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
   Xamarin.Essentials.Platform.Init(this, savedInstanceState);
   Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
   Plugin.XF.TouchID.Droid.Configuration.Activity = this;
   //In case any error in secret key.
   Plugin.XF.TouchID.Droid.Configuration.IsUseSecretKey = false;
   // Default, press "Use password" => Prompt the android KeygaurdManager auth page, you may change it as you want
   Plugin.XF.TouchID.Droid.Configuration.PromptPositiveAction = () => { Plugin.XF.TouchID.CrossTouchID.Current.PromptKeyguardManagerAuth(); };
   LoadApplication(new App());
```

```C#
 public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            Plugin.XF.TouchID.Droid.Configuration.OnKeyguardManagerResult(data, requestCode, resultCode);
            base.OnActivityResult(requestCode, resultCode, data);
        }
```
2. Manifest.xml add Fingerprint permission

```xml
<uses-permission android:name="android.permission.USE_FINGERPRINT" />
<!--Android 9-->
<uses-permission android:name="android.permission.USE_BIOMETRIC"/>
```

3. Setting the prompt message style, there are default
```C#
   Plugin.XF.TouchID.Droid.Configuration.PromptNegativeMessage = "Cancel";
   Plugin.XF.TouchID.Droid.Configuration.PromptPositiveMessage = "Use Password";
   Plugin.XF.TouchID.Droid.Configuration.PromptTitle = "Biometric Authentication";
   Plugin.XF.TouchID.Droid.Configuration.DefaultAuthenticationMessage = "Please do the authentication for further action";
   Plugin.XF.TouchID.Droid.Configuration.FingerprintFailedText = "Please try again";
   Plugin.XF.TouchID.Droid.Configuration.FingerprintErrorText = "Too many failed attempts, please wait 30s to retry";
   Plugin.XF.TouchID.Droid.Configuration.PasscodeAuthTitle = "Passcode authentication";
   Plugin.XF.TouchID.Droid.Configuration.PasscodeAuthDesc = "Please input passcode to continue";
   /// <summary>
   /// Only set it in Android 9, In case error in secret key, set it to false
   /// </summary>
   public static bool IsUseSecretKey = true;
   Plugin.XF.TouchID.Droid.Configuration.PopupTitleColor = Color.Blue;
   Plugin.XF.TouchID.Droid.Configuration.PopupBackgroundColor = Color.White;
   Plugin.XF.TouchID.Droid.Configuration.PopupDescriptionColor = Color.Black;
   Plugin.XF.TouchID.Droid.Configuration.PopupNegativeTextColor = Color.Red;
   Plugin.XF.TouchID.Droid.Configuration.PopupPositiveTextColor = Color.Black;
```
![](https://github.com/JimmyPun610/Plugin.XF.TouchID/blob/master/Plugin.XF.TouchID/Screenshots/Android6.png?raw=true")
![](https://github.com/JimmyPun610/Plugin.XF.TouchID/blob/master/Plugin.XF.TouchID/Screenshots/Android9.png?raw=true")
# Use in Xamarin Forms
- Check the device availabilities 
```c#
//    Support = 0,
//    DeviceNotSecured = 1,
//    NotEnrolledFinger = 2,
//    HardwareNotSupport = 3,
//    OSVersionNotSupport = 4,
   Plugin.XF.TouchID.Abstractions.TouchIDAvailabilities possible = Plugin.XF.TouchID.CrossTouchID.Current.IsFingerprintAuthenticationPossible();
```
- Prompt Security page for user to enroll finger or add passcode
```c#
   Plugin.XF.TouchID.CrossTouchID.Current.PromptSecuritySettings();
```
- Do the authentication
```C#
   Plugin.XF.TouchID.CrossTouchID.Current.Authenticate(descrptionMessage: "Please do the authentication for further action",
                successAction: () => { DisplayAlert("TouchID result", "Success", "Great"); }
```
