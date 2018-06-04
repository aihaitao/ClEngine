using System;

namespace ClEngine.Core.Math
{
    public class NaNException : Exception
    {
        public string MemberName { get; set; }



        public NaNException()
            : base()
        {
            
        }

        public NaNException(string message, string memberName)
            : base()
        {
            MemberName = memberName;
        }
    }
}