using IPlayer.IServices;

namespace IPlayer.Services;

public class DownloadFileService : IDownloadFileService
{
	public async Task<string> DownloadFileAsync(string fileUrl, string fileName, IProgress<double> progress, CancellationToken token)
	{
		try
		{
			var client = new HttpClient();
			var bufferSize = 4095;

			var response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead, token);

			response.EnsureSuccessStatusCode();

			var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

			var totalData = response.Content.Headers.ContentLength.GetValueOrDefault(-1L);

			using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize))
			{
				using var stream = await response.Content.ReadAsStreamAsync();
				var totalRead = 0L;
				var buffer = new byte[bufferSize];
				var isMoreDataToRead = true;

				do
				{
					token.ThrowIfCancellationRequested();

					var read = await stream.ReadAsync(buffer, 0, buffer.Length, token);

					if (read == 0)
					{
						isMoreDataToRead = false;
					}
					else
					{
						await fileStream.WriteAsync(buffer, 0, read);
						totalRead += read;
						progress.Report(totalRead * 1d / (totalData * 1d));
					}
				}
				while (isMoreDataToRead);
			}

			return filePath;
		}
		catch { }

		return string.Empty;
	}
}
