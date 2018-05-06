using ClEngine.Model;
using GalaSoft.MvvmLight;

namespace ClEngine.ViewModel
{
    public class ScriptViewModel : ViewModelBase
    {
        private ScriptModel _script;
        public ScriptModel Script
        {
            get => _script;
            set
            {
                _script = value;
                RaisePropertyChanged(()=> Script);
            }
        }
        
        public ScriptViewModel()
        {
            _script = new ScriptModel();
        }
    }
}