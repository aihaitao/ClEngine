using System;
using System.Windows.Forms;

namespace ClEngine.CoreLibrary.Particle
{
	internal sealed class HourglassCursor : IDisposable
	{
		public HourglassCursor()
		{
			this.PreviousCursor = Cursor.Current;

			Cursor.Current = Cursors.WaitCursor;
		}

		private Cursor PreviousCursor { get; set; }

		public void Dispose()
		{
			Cursor.Current = this.PreviousCursor;
		}
	}
}