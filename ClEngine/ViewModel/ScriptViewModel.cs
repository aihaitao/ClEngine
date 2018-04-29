using ClEngine.Model;
using GalaSoft.MvvmLight;
using ICSharpCode.AvalonEdit;

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