using Plugin.XF.TouchID.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Plugin.XF.TouchID
{
    public class CrossTouchID
    {
        static Lazy<ITouchID> Implementation = new Lazy<ITouchID>(() => CreateTouchIDHelper(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static ITouchID Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static ITouchID CreateTouchIDHelper()
        {
#if PORTABLE
        return null;
#else
            return new XFTouchIDImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
