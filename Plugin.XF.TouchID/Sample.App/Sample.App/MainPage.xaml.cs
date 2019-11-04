using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sample.App
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Plugin.XF.TouchID.Abstractions.TouchIDAvailabilities possible = Plugin.XF.TouchID.CrossTouchID.Current.IsFingerprintAuthenticationPossible();
  
            if (possible != Plugin.XF.TouchID.Abstractions.TouchIDAvailabilities.Support)
            {
                Plugin.XF.TouchID.CrossTouchID.Current.PromptSecuritySettings();
            }
            else
            {
                Plugin.XF.TouchID.CrossTouchID.Current.Authenticate(descrptionMessage: "Please do the authentication for further action",
                successAction: () => { DisplayAlert("TouchID result", "Success", "Great"); }
            );
            }

            //var canDo = Plugin.XF.TouchID.CrossTouchID.Current.IsFingerprintAuthenticationPossible();
            //switch (canDo)
            //{
            //    case Plugin.XF.TouchID.Abstractions.TouchIDAvailabilities.Support:
            //        {
            //            Plugin.XF.TouchID.CrossTouchID.Current.Authenticate(descrptionMessage: "",
            //    successAction: () => { DisplayAlert("TouchID result", "Success", "Great"); },
            //    failedAction: () => { DisplayAlert("TouchID result", "Fail", "Try again"); },
            //    errorAction: () => { DisplayAlert("TouchID result", "Fail too many times", "Oh no"); }
            //);
            //            break;
            //        }
            //    default:
            //        Plugin.XF.TouchID.CrossTouchID.Current
            //            .PromptSecuritySettings();
            //        break;
            //}


        }

    }
}