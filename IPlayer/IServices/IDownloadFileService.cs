namespace IPlayer.IServices;

public interface IDownloadFileService
{
	public Task<string> DownloadFileAsync(string fileUrl, string fileName, IProgress<double> progress, CancellationToken token);
}
