# Plugin.XF.TouchID
This project provides a easy way to call biometric authentication (Face / Fingerprint) function in Xamarin Forms

### Support
#### Android 6 - 10 TouchID (Target API Level 29)
#### iOS 10+ FaceID and TouchID

### Release notes
#### Version 2.0
1. Remove Xamarin Form Dependency
2. Add Support to Android 10
3. Android target framework change to 10

### Nuget installation
#### Install to your Xamarin Project
```
Install-Package Plugin.XF.TouchID
```

### iOS Guide
1. In AppDelegate.cs
```C#
   global::Xamarin.Forms.Forms.Init();
   LoadApplication(new App());
   //Init the library
   Plugin.XF.TouchID.TouchID.Init();
   return base.FinishedLaunching(app, options);
```
2. In your info.plist, add face id permission request
```
<key>NSFaceIDUsageDescription</key>
<string>Need your face to unlock secrets!</string>
```

FaceID | TouchID
------ | -----
<img src="https://github.com/JimmyPun610/Plugin.XF.TouchID/blob/master/Plugin.XF.TouchID/Screenshots/iOSFace.PNG?raw=true" width="200"> | <img src="https://github.com/JimmyPun610/Plugin.XF.TouchID/blob/master/Plugin.XF.TouchID/Screenshots/iOSTouch.png?raw=true" width="200">

### Android Guide

1. Set TargetFramework as Android10.0 (Q) API Level 29

2. In MainActivity
```C#
   global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
   Xamarin.Essentials.Platform.Init(this, savedInstanceState);
   //Init the library
   //Use secret key, please use a unique keyname
   Plugin.XF.TouchID.TouchID.Init(this, "plugin.xf.touchid.fingerprintkey");
   //If you do not want to use secret key
   //Plugin.XF.TouchID.TouchID.Init(this);
   LoadApplication(new App());
```

```C#
   protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
   {
        Plugin.XF.TouchID.TouchID.OnKeyguardManagerResult(data, requestCode, resultCode);
        base.OnActivityResult(requestCode, resultCode, data);
   }
```

3. Manifest.xml add Fingerprint permission
```xml
<uses-permission android:name="android.permission.USE_FINGERPRINT" />
<!--Android 9+-->
<uses-permission android:name="android.permission.USE_BIOMETRIC"/>
```

Android 6-8 | Android 9+
------ | -----
<img src="https://github.com/JimmyPun610/Plugin.XF.TouchID/blob/master/Plugin.XF.TouchID/Screenshots/Android6.png?raw=true" width="200"> | <img src="https://github.com/JimmyPun610/Plugin.XF.TouchID/blob/master/Plugin.XF.TouchID/Screenshots/Android9.png?raw=true" width="200">

### Use in Xamarin Forms

#### Check the device availabilities 
```c#
//    Support = 0,
//    DeviceNotSecured = 1,
//    NotEnrolledFinger = 2,
//    HardwareNotSupport = 3,
//    OSVersionNotSupport = 4,
   Plugin.XF.TouchID.TouchIDStatus possible = Plugin.XF.TouchID.TouchID.IsFingerprintAuthenticationPossible());
```

#### Prompt Security page for user to enroll finger or add passcode
```c#
   Plugin.XF.TouchID.TouchID.PromptSecuritySettings();
```

#### Do the authentication

##### Use passcode / pin for alternative authentication (Android only, iOS default allowed)
```C#
	var dialogConfig = new Plugin.XF.TouchID.DialogConfiguration(dialogTitle: "Sign In", //Display in Android only
                                                                         dialogDescritpion: "Detect you biometic to auth", //Display on Android and iOS(TouchID)
                                                                         successAction: () =>
                                                                         {
                                                                             //Will fired when authentication success
                                                                             Device.BeginInvokeOnMainThread(() =>
                                                                             {
                                                                                 DisplayAlert("Congratulation", "You pass the authentication", "OK");
                                                                             });
                                                                         },
                                                                         alterAuthButtonText: "Use PIN", //Display in Android only
                                                                         fingerprintDialogConfiguration: new Plugin.XF.TouchID.FingerprintDialogConfiguration
                                                                         {
                                                                             //For Android 6-8 only
                                                                             FingerprintHintString = "Touch Sensor",
                                                                             FingerprintNotRecoginzedString = "Not regonized"
                                                                         },
                                                                         failedAction: () =>
                                                                         {  
                                                                             //For Android 6-8 only
                                                                             Device.BeginInvokeOnMainThread(() =>
                                                                             {
                                                                                 DisplayAlert("Alert", "Too many unsuccessful attempt, please try again later", "OK");
                                                                             });
                                                                         });

   await Plugin.XF.TouchID.TouchID.Authenticate(dialogConfig);
```
##### Use customized action as alternative (Android only, iOS default use password)
```C#
  var dialogConfig = new Plugin.XF.TouchID.DialogConfiguration(dialogTitle: "Sign In", //Display in Android only
                                                                         dialogDescritpion: "Detect you biometic to auth", //Display on Android and iOS(TouchID)
                                                                         successAction: () =>
                                                                         {
                                                                             //Will fired when authentication success
                                                                             Device.BeginInvokeOnMainThread(() =>
                                                                             {
                                                                                 DisplayAlert("Congratulation", "You pass the authentication", "OK");
                                                                             });
                                                                         },
                                                                         customizedAction: new Plugin.XF.TouchID.CustomizedAction("Cancel", () =>
                                                                         {
                                                                             //Android Only
                                                                             Device.BeginInvokeOnMainThread(() =>
                                                                             {
                                                                                 DisplayAlert("Alert", "You cancel the authentication", "OK");
                                                                             });
                                                                         }),
                                                                         fingerprintDialogConfiguration: new Plugin.XF.TouchID.FingerprintDialogConfiguration
                                                                         {
                                                                             //For Android 6-8 only
                                                                             FingerprintHintString = "Touch Sensor",
                                                                             FingerprintNotRecoginzedString = "Not regonized"
                                                                         },
                                                                         failedAction: () =>
                                                                         {
                                                                             //For Android 6-8 only
                                                                             Device.BeginInvokeOnMainThread(() =>
                                                                             {
                                                                                 DisplayAlert("Alert", "Too many unsuccessful attempt, please try again later", "OK");
                                                                             });
                                                                         });
	await Plugin.XF.TouchID.TouchID.Authenticate(dialogConfig);
```
