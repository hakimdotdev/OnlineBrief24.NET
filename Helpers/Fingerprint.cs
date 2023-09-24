
using System.Globalization;

namespace OnlineBrief24.Helpers
{
	public class Fingerprint
	{
		public static byte[] ToBytes(string thumbprintStr)
		{
			byte[] thumbprintBytes = thumbprintStr.Split(':').Select(s => Convert.ToByte(s, 16)).ToArray();

			return thumbprintBytes;
		
		}
	}
}
