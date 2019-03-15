# Plugin.XF.TouchID
This is the library for Xamarin Form to use Biometric ID to do the local authentication.
Current iOS version is ok to use.
Android version is pending the development

# iOS Guide
1. In AppDelegate.cs, set the message first
```c#
Plugin.XF.TouchID.iOS.Configuration.DefaultAuthenticationMessage = "Set the default authenticate message";
Plugin.XF.TouchID.iOS.Configuration.DefaultFailAttemptNumberExceededMsg = "Set the default failed attempt exceed msg";
```
