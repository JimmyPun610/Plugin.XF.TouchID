using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Plugin.XF.TouchID
{
    public class DialogConfiguration
    {
        internal string AlternativeActionMessage;
        internal Action AlternativeAction;

        /// <summary>
        /// Title of the dialog in Android
        /// Not showing in iOS
        /// </summary>
        internal string DialogTitle;
        /// <summary>
        /// Description message of the dialog in Android
        /// Title of the dialog in iOS
        /// </summary>
        internal string DialogDescription;

        /// <summary>
        /// Action will be taken if authentication success
        /// </summary>
        internal Action SuccessAction;

        /// <summary>
        /// Action will be taken if too many unsuccessful attempt
        /// Android : Available
        /// iOS : Unavailable
        /// </summary>
        internal Action FailedAction;


        internal FingerprintDialogConfiguration FingerprintDialogConfiguration;

        /// <summary>
        /// Allow use passcode / pin as alternative, for Android only
        /// </summary>
        internal bool IsUseAlternativeAuthentication = false;

        public const string DEFAULT_ALTERNATIVE_ACTION_MESSAGE = "Use PIN";
        public const string DEFAULT_DIALOG_TITLE = "Biometric Authentication";
        public const string DEFAULT_DIALOG_DESCRIPTION = "Please do the authentication for further action";

        /// <summary>
        /// Default constructor
        /// </summary>
        public DialogConfiguration()
        {
            this.DialogTitle = DEFAULT_DIALOG_TITLE;
            this.DialogDescription = DEFAULT_DIALOG_DESCRIPTION;

            this.AlternativeActionMessage = DEFAULT_ALTERNATIVE_ACTION_MESSAGE;
            this.AlternativeAction = null;

            this.SuccessAction = null;
            this.FailedAction = null;

            this.IsUseAlternativeAuthentication = false;
            this.FingerprintDialogConfiguration = new FingerprintDialogConfiguration();
        }

        /// <summary>
        /// Biometric authentication dialog configuration
        /// Init the dialog configuration and have customized method as alternative button
        /// Android : Available, customized action will be triggerred when user click alternative button
        /// iOS : No customized action is allowed.
        /// </summary>
        /// <param name="dialogTitle">Dialog title, should not be empty</param>
        /// <param name="dialogDescritpion">Dialog description, can be empty if no needed</param>
        /// <param name="successAction">Action will be taken if authentication success</param>
        /// <param name="customizedAction">Customized action when user clicked the button</param>
        /// <param name="fingerprintDialogConfiguration">For Android 6 - 8 only</param>
        public DialogConfiguration(string dialogTitle, string dialogDescritpion, Action successAction, CustomizedAction customizedAction, Action failedAction = null, FingerprintDialogConfiguration fingerprintDialogConfiguration = null)
        {
            this.DialogDescription = dialogDescritpion;
            this.DialogTitle = dialogTitle;
            this.SuccessAction = successAction;
            this.FailedAction = failedAction;
            this.AlternativeActionMessage = customizedAction.ActionTitle;
            this.AlternativeAction = customizedAction.Action;
            this.IsUseAlternativeAuthentication = false;
            this.FingerprintDialogConfiguration = fingerprintDialogConfiguration;
        }
        /// <summary>
        /// Biometric authentication dialog configuration
        /// Init the dialog configuration to use alternative authentication method
        /// Android : Available
        /// iOS : Available
        /// </summary>
        /// <param name="dialogTitle">Dialog title, should not be empty</param>
        /// <param name="dialogDescritpion">Dialog description, can be empty if no needed</param>
        /// <param name="successAction">Action will be taken if authentication success</param>
        /// <param name="alterAuthButtonText">Button text on dialog</param>
        /// <param name="fingerprintDialogConfiguration">For Android 6 - 8 only</param>
        public DialogConfiguration(string dialogTitle, string dialogDescritpion, Action successAction, Action failedAction = null, string alterAuthButtonText = "Use PIN", FingerprintDialogConfiguration fingerprintDialogConfiguration = null)
        {
            this.DialogDescription = dialogDescritpion;
            this.DialogTitle = dialogTitle;
            this.SuccessAction = successAction;
            this.FailedAction = failedAction;
            this.AlternativeActionMessage = alterAuthButtonText;
            this.AlternativeAction = new Action(() => TouchID.PromptKeyguardManagerAuth(dialogTitle, dialogDescritpion));
            this.IsUseAlternativeAuthentication = true;
            this.FingerprintDialogConfiguration = fingerprintDialogConfiguration;
        }
    }
}
