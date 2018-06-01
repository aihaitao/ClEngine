/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ClEngine"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace ClEngine.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ScriptViewModel>();
            SimpleIoc.Default.Register<SystemDocumentViewModel>();
            SimpleIoc.Default.Register<ProjectViewModel>();
        }

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }
        
        public ScriptViewModel Script
        {
            get { return ServiceLocator.Current.GetInstance<ScriptViewModel>(); }
        }
        
        public SystemDocumentViewModel Document
        {
            get { return ServiceLocator.Current.GetInstance<SystemDocumentViewModel>(); }
        }

        public ProjectViewModel Project
        {
            get { return ServiceLocator.Current.GetInstance<ProjectViewModel>(); }
        }

        public static void Cleanup()
        {
            SimpleIoc.Default.Unregister<MainViewModel>();
            SimpleIoc.Default.Unregister<ScriptViewModel>();
            SimpleIoc.Default.Unregister<SystemDocumentViewModel>();
            SimpleIoc.Default.Unregister<ProjectViewModel>();
        }
    }
}