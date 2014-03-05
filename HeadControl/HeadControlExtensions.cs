using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeadControl
{
    public static class HeadControlExtensions
    {
        public static String GetError(this Exception ex)
        {
            String extra = "";

            return extra + " " + ex.Message + (ex.InnerException!= null ? ex.InnerException.Message : "");
        }
    }
}
