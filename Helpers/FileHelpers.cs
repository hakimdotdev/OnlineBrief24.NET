namespace OnlineBrief24.Helpers
{
	public class FileHelpers
	{
		public static async Task<byte[]> ToByteArray(IFormFile file)
		{
			using (var memoryStream = new MemoryStream())
			{
				await file.CopyToAsync(memoryStream);
				return memoryStream.ToArray();
			}
		}
	}
}
