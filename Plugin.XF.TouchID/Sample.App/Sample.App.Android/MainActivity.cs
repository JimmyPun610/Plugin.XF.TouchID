using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace Sample.App.Droid
{
    [Activity(Label = "Sample.App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            Plugin.XF.TouchID.Droid.Configuration.Activity = this;
            Plugin.XF.TouchID.Droid.Configuration.PromptPositiveAction = () => { Plugin.XF.TouchID.CrossTouchID.Current.PromptKeyguardManagerAuth(); };
            //In case any error in secret key.
            //Plugin.XF.TouchID.Droid.Configuration.IsUseSecretKey = false;
            LoadApplication(new App());
        }
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
    }
}