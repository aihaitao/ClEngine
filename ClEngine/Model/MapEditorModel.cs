using GalaSoft.MvvmLight;

namespace ClEngine.Model
{
    public class MapEditorModel : ObservableObject
    {
        private string _eventId;
        public string EventId
        {
            get => _eventId;
            set
            {
                _eventId = value;
                RaisePropertyChanged(() => EventId);
            }
        }

        private string _sign;
        public string Sign
        {
            get => _sign;
            set
            {
                _sign = value;
                RaisePropertyChanged(() => Sign);
            }
        }

        private string _property;
        public string Property
        {
            get => _property;
            set
            {
                _property = value;
                RaisePropertyChanged(() => Property);
            }
        }

        private string _camp;
        public string Camp
        {
            get => _camp;
            set
            {
                _camp = value;
                RaisePropertyChanged(() => Camp);
            }
        }

        private int _gridX;
        public int GridX
        {
            get => _gridX;
            set
            {
                _gridX = value;
                RaisePropertyChanged(() => GridX);
            }
        }

        private int _gridY;
        public int GridY
        {
            get => _gridY;
            set
            {
                _gridY = value;
                RaisePropertyChanged(() => GridY);
            }
        }
    }
}