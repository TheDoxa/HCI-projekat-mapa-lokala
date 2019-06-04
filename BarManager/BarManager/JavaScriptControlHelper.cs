using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Windows;

namespace BarManager
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class JavaScriptControlHelper
    {
        Window window;
        public JavaScriptControlHelper(Window w)
        {
            window = w;
        }

        public void RunFromJavascript(string param)
        {
            
        }
    }
}
