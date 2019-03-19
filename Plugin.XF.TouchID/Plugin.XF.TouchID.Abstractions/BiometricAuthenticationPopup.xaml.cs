using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plugin.XF.TouchID.Abstractions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BiometricAuthenticationPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public Action NegativeAction;
        public Action ExitAction;
        public BiometricAuthenticationPopup(string title, string description, string negativeText, Action negativeAction)
        {
            InitializeComponent();
            TitleLabel.Text = title;
            DescriptionLabel.Text = description;
            NegativeButton.Text = negativeText;
            NegativeAction = negativeAction;
        }

        private async void NegativeButtonTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            NegativeAction?.Invoke();
            await PopupNavigation.Instance.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            ExitAction?.Invoke();
            return true;
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            if(this.CloseWhenBackgroundIsClicked)
                ExitAction?.Invoke();
            return false;
        }
    }
}