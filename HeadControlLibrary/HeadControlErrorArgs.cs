using System;

namespace HeadControl
{
    public class HeadControlErrorArgs : EventArgs
    {
        public String ErrorMessage { get; set; }
        public HeadControlErrorArgs(Exception ex)
        {
            ErrorMessage = ex.GetError();
        }

        public HeadControlErrorArgs(String error)
        {
            ErrorMessage = error;
        }
    }
}
