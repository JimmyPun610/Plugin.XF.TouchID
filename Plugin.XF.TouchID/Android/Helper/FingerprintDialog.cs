using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.Hardware.Fingerprint;
using Android.Transitions;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace Plugin.XF.TouchID
{
    internal class FingerprintDialog : Android.Support.V4.App.DialogFragment
    {
        DialogConfiguration _dialogConfiguration;
        FingerprintManagerCompat.AuthenticationCallback _authenticationCallBack;
        Android.Support.V4.OS.CancellationSignal _fingerprintCancellationSignal;
        #region Views
        Android.Support.V7.Widget.AppCompatTextView _titleTextView;
        Android.Support.V7.Widget.AppCompatTextView _descTextView;
        Android.Support.V7.Widget.AppCompatTextView _statusTextView;
        Android.Widget.ImageView _fingerprintIconView;
        Android.Support.V7.Widget.AppCompatButton _alternativeActionButton;
        Android.Widget.LinearLayout _fingerprintStatusPanelLinearLayout;
        #endregion




        public FingerprintDialog(DialogConfiguration dialogConfiguration, FingerprintManagerCompat.AuthenticationCallback authenticationCallBack, Android.Support.V4.OS.CancellationSignal fingerprintCancellationSignal)
        {
 
            _dialogConfiguration = dialogConfiguration;
            _authenticationCallBack = authenticationCallBack;
            _fingerprintCancellationSignal = fingerprintCancellationSignal;
            ((AuthenticationCallBack)_authenticationCallBack).SetFingerprintDialog(this);
        }
        public virtual void DismissDialog()
        {
            Configuration.CurrentActivity.RunOnUiThread(() => this.Dismiss());
        }

        public virtual void SetAuthenticationFailed()
        {
            Configuration.CurrentActivity.RunOnUiThread(() =>
            {
        
                CustomAnimations.FadeOutAnimation.SetAnimationListener(new FadeAnimationListener
                {
                    ActionOnAnimationEnd = () =>
                    {
                        _statusTextView.Text = _dialogConfiguration.FingerprintDialogConfiguration.FingerprintNotRecoginzedString;
                        _statusTextView.SetTextColor(Android.Graphics.Color.Red);
                        _fingerprintIconView.SetImageResource(Resource.Drawable.ic_fingerprint_error);
                        _fingerprintIconView.StartAnimation(CustomAnimations.FadeInAnimation);
                    }
                });
                _fingerprintStatusPanelLinearLayout.StartAnimation(CustomAnimations.FadeOutAnimation);

            });
        }

      
        private void FindView(View view)
        {
            _titleTextView = view.FindViewById<Android.Support.V7.Widget.AppCompatTextView>(Resource.Id.fingerprint_dialog_title_default);
            if (string.IsNullOrWhiteSpace(_dialogConfiguration.DialogTitle))
                _titleTextView.Visibility = ViewStates.Gone;
            else _titleTextView.Text = _dialogConfiguration.DialogTitle;

            _descTextView = view.FindViewById<Android.Support.V7.Widget.AppCompatTextView>(Resource.Id.fingerprint_dialog_message_default);
            if (string.IsNullOrWhiteSpace(_dialogConfiguration.DialogDescription))
                _descTextView.Visibility = ViewStates.Gone;
            else _descTextView.Text = _dialogConfiguration.DialogDescription;

            _fingerprintIconView = view.FindViewById<Android.Widget.ImageView>(Resource.Id.fingerprint_icon);
            _statusTextView = view.FindViewById<Android.Support.V7.Widget.AppCompatTextView>(Resource.Id.fingerprint_dialog_status_default);
            _statusTextView.Text = _dialogConfiguration.FingerprintDialogConfiguration.FingerprintHintString;

            _alternativeActionButton = view.FindViewById<Android.Support.V7.Widget.AppCompatButton>(Resource.Id.fingerprint_dialog_alternative_default);
            _alternativeActionButton.Text = _dialogConfiguration.AlternativeActionMessage;
            _alternativeActionButton.Click += (s, e) =>
            {
                //_fingerprintCancellationSignal.Cancel();
                DismissDialog();
                _dialogConfiguration.AlternativeAction?.Invoke();
            };

            _fingerprintStatusPanelLinearLayout = view.FindViewById<Android.Widget.LinearLayout>(Resource.Id.fingerprint_status_panel_default);
        }


      

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.dialog_fingerprint_default, container, false);
            FindView(view);
            return view;
        }

    }
}
