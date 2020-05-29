using Plugin.XF.TouchID;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace SampleApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
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
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Plugin.XF.TouchID.TouchID.PromptSecuritySettings();
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
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
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert("Status",
                   $"Is Device Secured : {TouchID.IsDeviceSecured().ToString()} {Environment.NewLine}" +
                   $"Is Fingerprint Enrolled : {TouchID.IsFingerPrintEnrolled().ToString()} {Environment.NewLine}" +
                   $"Is Hardware Support : {TouchID.IsHardwareDetected().ToString()} {Environment.NewLine}" +
                   $"Is Permission Grant : {TouchID.IsPermissionGranted().ToString()} {Environment.NewLine}"+
                   $"Fingerprint Status : {TouchID.IsFingerprintAuthenticationPossible().ToString()} {Environment.NewLine}" 
                , "Ok");
            });
        }
    }
}
