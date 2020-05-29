using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.XF.TouchID
{
    public class CustomizedAction
    {
        public string ActionTitle = "Default Action Name";
        public Action Action { get; set; }

        public CustomizedAction(string actionTitle, Action action)
        {
            this.ActionTitle = actionTitle;
            this.Action = action;
        }
    }
}
