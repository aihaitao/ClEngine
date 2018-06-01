using System.ComponentModel;

namespace ClEngine.Core
{
    public class MessageModel
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public object Message { get; set; }
    }
}