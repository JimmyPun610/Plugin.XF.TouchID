using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plugin.XF.TouchID.Abstractions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BiometricAuthenticationPopup : Rg.Plugins.Popup.Pages.PopupPage, INotifyPropertyChanged
    {
        public Action NegativeAction;
        public Action PositiveAction;
        public Action ExitAction;


        public BiometricAuthenticationPopup(string title, string description, string negativeText, string positiveText,
            Action negativeAction, Action positiveAction)
        {
            InitializeComponent();
            TitleLabelText = title;
            DescLabelText = description;
            NegativeText = negativeText;
            PositiveText = positiveText;
            NegativeAction = negativeAction;
            PositiveAction = positiveAction;

            this.BindingContext = this;
        }

        public void PromptFailed(string failText)
        {
            FailedLabel.Text = failText;
            FailedLabel.IsVisible = true;
        }

        public void PromptError(string errorText)
        {
            FailedLabel.IsVisible = true;
            FailedLabel.Text = errorText;
        }

        public void CustomizeUI(Color popupTitleColor, Color popupBackgroundColor, Color popupDescriptionColor, Color negativeTextColor, Color positiveTextColor)
        {
            PopupTitleColor = popupTitleColor;
            PopupBackgroundColor = popupBackgroundColor;
            PopupDescriptionColor = popupDescriptionColor;
            PopupNegativeTextColor = negativeTextColor;
            PopupPositiveTextColor = positiveTextColor;
        }

        private async void NegativeButtonTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
            NegativeAction?.Invoke();
        }
        private async void PositiveButtonTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
            PositiveAction?.Invoke();
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

        public static readonly BindableProperty TitleLabelTextProperty =
   BindableProperty.Create<BiometricAuthenticationPopup, string>(w => w.TitleLabelText, string.Empty);
        public string TitleLabelText
        {
            get { return (string)GetValue(TitleLabelTextProperty); }
            set { SetValue(TitleLabelTextProperty, value); }
        }

        public static readonly BindableProperty DescLabelTextProperty =
BindableProperty.Create<BiometricAuthenticationPopup, string>(w => w.DescLabelText, string.Empty);
        public string DescLabelText
        {
            get { return (string)GetValue(DescLabelTextProperty); }
            set { SetValue(DescLabelTextProperty, value); }
        }
        
        public static readonly BindableProperty NegativeTextProperty =
BindableProperty.Create<BiometricAuthenticationPopup, string>(w => w.NegativeText, string.Empty);
        public string NegativeText
        {
            get { return (string)GetValue(NegativeTextProperty); }
            set { SetValue(NegativeTextProperty, value); }
        }

        public static readonly BindableProperty PositiveTextProperty =
BindableProperty.Create<BiometricAuthenticationPopup, string>(w => w.PositiveText, string.Empty);
        public string PositiveText
        {
            get { return (string)GetValue(PositiveTextProperty); }
            set { SetValue(PositiveTextProperty, value); }
        }

        public static readonly BindableProperty PopupTitleColorProperty =
    BindableProperty.Create<BiometricAuthenticationPopup, Color>(w => w.PopupTitleColor, Color.Blue);
        public Color PopupTitleColor
        {
            get { return (Color)GetValue(PopupTitleColorProperty); }
            set { SetValue(PopupTitleColorProperty, value); }
        }

        public static readonly BindableProperty PopupBackgroundColorProperty =
 BindableProperty.Create<BiometricAuthenticationPopup, Color>(w => w.PopupBackgroundColor, Color.White);
        public Color PopupBackgroundColor
        {
            get { return (Color)GetValue(PopupBackgroundColorProperty); }
            set { SetValue(PopupBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty PopupDescriptionColorProperty =
BindableProperty.Create<BiometricAuthenticationPopup, Color>(w => w.PopupDescriptionColor, Color.Black);
        public Color PopupDescriptionColor
        {
            get { return (Color)GetValue(PopupDescriptionColorProperty); }
            set { SetValue(PopupDescriptionColorProperty, value); }
        }

        public static readonly BindableProperty PopupNegativeTextColorProperty =
BindableProperty.Create<BiometricAuthenticationPopup, Color>(w => w.PopupNegativeTextColor, Color.Red);
        public Color PopupNegativeTextColor
        {
            get { return (Color)GetValue(PopupNegativeTextColorProperty); }
            set { SetValue(PopupNegativeTextColorProperty, value); }
        }

        public static readonly BindableProperty PopupPositiveTextColorProperty =
BindableProperty.Create<BiometricAuthenticationPopup, Color>(w => w.PopupPositiveTextColor, Color.Black);
        public Color PopupPositiveTextColor
        {
            get { return (Color)GetValue(PopupPositiveTextColorProperty); }
            set { SetValue(PopupPositiveTextColorProperty, value); }
        }

      
    }
}