using System;

namespace ClEngine.CoreLibrary.Particle
{
	public class CoreOperationResult
	{
		public static CoreOperationResult OK { get; private set; }

		static CoreOperationResult()
		{
			OK = new CoreOperationResult(null);
		}

		public CoreOperationResult(Exception exception)
		{
			Exception = exception;
		}

		public Exception Exception { get; private set; }
	}
}