using Android.Views.Animations;

using System;
using System.Collections.Generic;
using System.Text;
using static Android.Views.Animations.Animation;

namespace Plugin.XF.TouchID
{
    public class FadeAnimationListener : Java.Lang.Object, Android.Views.Animations.Animation.IAnimationListener
    {
        public Action ActionOnAnimationEnd;
       
        public void OnAnimationEnd(Animation animation)
        {
            ActionOnAnimationEnd?.Invoke();
        }

        public void OnAnimationRepeat(Animation animation)
        {
       
        }

        public void OnAnimationStart(Animation animation)
        {
        }
    }
}

