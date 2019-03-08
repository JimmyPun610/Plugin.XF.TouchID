using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Plugin.XF.TouchID.CrossTouchID.Current.Authenticate(descrptionMessage: "",
                successAction: () => { DisplayAlert("TouchID result", "Success", "Great"); },
                failedAction: () => { DisplayAlert("TouchID result", "Fail", "Try again"); },
                errorAction: () => { DisplayAlert("TouchID result", "Fail too many times", "Oh no"); }
            );
        }

    }
}