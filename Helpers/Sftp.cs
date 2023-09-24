using System.Net.Mail;
using OnlineBrief24.Models;
using Renci.SshNet;
using File = OnlineBrief24.Models.File;

namespace OnlineBrief24.Helpers
{
	public class Sftp
	{
		private string _host = "api.onlinebrief24.de";
		private string _uploadPath = "/upload/api";
		private readonly string _user;
		private readonly string _password;
		private string _thumbprint = "5b:d1:29:17:cb:5e:17:b9:e2:29:4e:1e:aa:c7:d9:b2";
		private SshClient _sshClient;
		private SftpClient _sftpClient;
		private bool _verify = false;
		public Sftp(string user, string password, bool verify)
		{
			var envHost = System.Environment.GetEnvironmentVariable("SFTPHOST");
			var envUser = System.Environment.GetEnvironmentVariable("SFTPUSER");
			var envPassword = System.Environment.GetEnvironmentVariable("SFTPPASSWORD");
			var envUploadPath = System.Environment.GetEnvironmentVariable("UPLOADPATH");
			var envThumbprint = System.Environment.GetEnvironmentVariable("THUMBPRINT");
			var envVerifyThumbprint = System.Environment.GetEnvironmentVariable("VERIFYTHUMBPRINT");
			_host = !Helpers.Environment.InDocker ? _host : envHost;
			_user = Helpers.Environment.InDocker ? envUser.ToLower() : user.ToLower();
			_password = Helpers.Environment.InDocker ? envPassword : user;
			_uploadPath = !Helpers.Environment.InDocker && String.IsNullOrWhiteSpace(envUploadPath) ? _uploadPath : envUploadPath;
			_thumbprint = !Helpers.Environment.InDocker && String.IsNullOrWhiteSpace(envThumbprint) ? _thumbprint : envThumbprint;
			_verify =  String.Equals(envVerifyThumbprint == "True", StringComparison.OrdinalIgnoreCase) ? true : false;
		}
		
		private async Task<bool> Connect()
		{
			if (_verify)
			{
				using (_sshClient = new SshClient(_host, _user, _password))
				{
					_sshClient.HostKeyReceived += (sender, e) =>
					{
						if (_thumbprint.Length == e.FingerPrint.Length)
						{
							for (var i = 0; i < _thumbprint.Length; i++)
							{
								if (_thumbprint[i] != e.FingerPrint[i])
								{
									e.CanTrust = false;
									break;
								}
							}
						}
						else
						{
							e.CanTrust = false;
						}
					};
					_sshClient.Connect();
					return _sshClient.IsConnected;
					
				}
			}
			var connectionInfo = new Renci.SshNet.ConnectionInfo(_host,
										_user,
										new PasswordAuthenticationMethod(_user, _password));
			using (_sftpClient = new SftpClient(connectionInfo))
			{
				_sftpClient.Connect();
				if (_sftpClient.IsConnected)
				{
					return true;
				}
				return false;
			}
		}

		internal async Task Upload(IFormFile file)
		{
			string uploadfile = file.FileName;
			Console.WriteLine(file.Name);
			Console.WriteLine("uploadfile" + uploadfile);

			if (await this.Connect())
			{
				if (_sftpClient.IsConnected)
				{
					var fileStream = new FileStream(uploadfile, FileMode.Open);
					if (fileStream != null)
					{
						_sftpClient.UploadFile(fileStream, _uploadPath + file.Name, null);
						_sftpClient.Disconnect();
						_sftpClient.Dispose();
					}
				}
			}
		}
		internal async Task<bool> Upload(List<IFormFile> files)
		{
			if (await this.Connect())
			{
				if (_sftpClient.IsConnected)
				{
					foreach (File file in files)
					{
						var fileStream = new FileStream(file.FileName, FileMode.Open);
						if (fileStream != null)
						{
							_sftpClient.UploadFile(fileStream, _uploadPath + file.Name, null);
							_sftpClient.Disconnect();
							_sftpClient.Dispose();
							return true;
							
						}
					}
				}
			}
			return false;

		}

	}
}

