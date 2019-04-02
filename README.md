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
