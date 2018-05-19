using System.ComponentModel;
using GalaSoft.MvvmLight;

namespace ClEngine.Core
{
    public class MessageModel : ObservableObject
    {
        private object _message;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public object Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }
    }
}