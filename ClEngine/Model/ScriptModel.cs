using GalaSoft.MvvmLight;

namespace ClEngine.Model
{
    public class ScriptModel : ObservableObject
    {
        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }
    }
}