# Plugin.XF.TouchID
This is the library for Xamarin Form to use Biometric ID to do the local authentication.
Current iOS version is ok to use.
Android version is pending the development

# iOS Guide
1. Setting the prompt message first, there are default messages in English only.
```C#
   global::Xamarin.Forms.Forms.Init();
   LoadApplication(new App());
   Plugin.XF.TouchID.iOS.Configuration.DefaultAuthenticationMessage = "Set the default authenticate message";
   Pugin.XF.TouchID.iOS.Configuration.DefaultFailAttemptNumberExceededMsg = "Set the default failed attempt exceed msg";
   return base.FinishedLaunching(app, options);
```

# Android Guide
1. In MainActivity
```C#
global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
Xamarin.Essentials.Platform.Init(this, savedInstanceState);
Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
Plugin.XF.TouchID.Configuration.Activity = this;
// Default, press "Use password" => Prompt the android KeygaurdManager auth page, you may change it as you want
Plugin.XF.TouchID.Configuration.PromptPositiveAction = () => { Plugin.XF.TouchID.CrossTouchID.Current.PromptKeyguardManagerAuth(); };
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
            Plugin.XF.TouchID.Configuration.OnKeyguardManagerResult(data, requestCode, resultCode);
            base.OnActivityResult(requestCode, resultCode, data);
        }
```
3. Setting the prompt message style, there are default
```C#
Plugin.XF.TouchID.Configuration.PromptNegativeMessage = "Cancel";
Plugin.XF.TouchID.Configuration.PromptPositiveMessage = "Use Password";
Plugin.XF.TouchID.Configuration.PromptTitle = "Biometric Authentication";
Plugin.XF.TouchID.Configuration.DefaultAuthenticationMessage = "Please do the authentication for further action";
Plugin.XF.TouchID.Configuration.FingerprintFailedText = "Please try again";
Plugin.XF.TouchID.Configuration.FingerprintErrorText = "Too many failed attempts, please wait 30s to retry";
Plugin.XF.TouchID.Configuration.PasscodeAuthTitle = "Passcode authentication";
Plugin.XF.TouchID.Configuration.PasscodeAuthDesc = "Please input passcode to continue";

Plugin.XF.TouchID.Configuration.PopupTitleColor = Color.Blue;
Plugin.XF.TouchID.Configuration.PopupBackgroundColor = Color.White;
Plugin.XF.TouchID.Configuration.PopupDescriptionColor = Color.Black;
Plugin.XF.TouchID.Configuration.PopupNegativeTextColor = Color.Red;
Plugin.XF.TouchID.Configuration.PopupPositiveTextColor = Color.Black;
```

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
