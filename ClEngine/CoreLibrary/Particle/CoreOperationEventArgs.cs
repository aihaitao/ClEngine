using System;

namespace ClEngine.CoreLibrary.Particle
{
	public class CoreOperationEventArgs : EventArgs
	{
		protected CoreOperationEventArgs() { }

		public CoreOperationResult Result { get; set; }
	}
}