using Android.Views.Animations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.XF.TouchID
{
    public class CustomAnimations
    {
        public static Animation FadeInAnimation = AnimationUtils.LoadAnimation(Configuration.CurrentActivity, Android.Resource.Animation.FadeIn);
        public static Animation FadeOutAnimation = AnimationUtils.LoadAnimation(Configuration.CurrentActivity, Android.Resource.Animation.FadeOut);

    }
}
