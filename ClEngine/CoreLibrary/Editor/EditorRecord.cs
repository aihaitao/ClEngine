using System;
using System.Windows.Forms;
using ClEngine.ViewModel;
using CommonServiceLocator;

namespace ClEngine.CoreLibrary.Editor
{
	public static class EditorRecord
	{
	    public static MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
	}
}