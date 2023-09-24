using System;
namespace OnlineBrief24.Helpers
{
	public class Environment
	{
		public static bool InDocker { get { return System.Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"; } }
	}
}
