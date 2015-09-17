using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;

namespace APConsole
{
	static class Program
	{
		#region Some internal needs
		[DllImport("user32.dll")]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		enum ExitCode
		{
			Success = 0, 
			InvalidLoginPasswordOrWebApiKey = 10,
		}

		#endregion

		[MTAThread]
		static int Main(string[] args)
		{
			//hiding console window
			//var hWnd = FindWindow(null, Console.Title);

			//if (hWnd != IntPtr.Zero)
			//{
			//    ShowWindow(hWnd, 0); // 0 - SW_HIDE, 1 - SW_SHOWNORMA
			//}

			var checkLoginOption = false;
			var password         = string.Empty;
			var user             = string.Empty;
			var webapikey        = string.Empty;
			var searchFile       = string.Empty;
			var resultFile       = string.Empty;
			var email            = string.Empty;

			for (var i = 0; i < args.Length; i++)
			{
				switch (args[i])
				{
					case "-check":
					case "--checkLogin":
						checkLoginOption = true;
						break;

					case "-p":
					case "--pass":
						password = args[i + 1];
						break;

					case "-u":
					case "--user":
						user = args[i + 1];
						break;

					case "-api":
					case "--webapi":
						webapikey = args[i + 1];
						break;

					case "-sF":
					case "--searchFile":
						searchFile = args[i + 1];
						break;

					case "-rF":
					case "--resultFile":
						resultFile = args[i + 1];
						break;

					case "-em":
					case "--email":
						email = args[i + 1];
						break;
				}
			}

			if (args.Length == 0)
			{
				return 0;
			}

			AllegroClient allegroClient;
			if (checkLoginOption)
			{
				try
				{
					AllegroClient.WebApiKey = webapikey;
					allegroClient           = new AllegroClient(user, password, true);
				}
				catch(Exception)
				{
					Console.WriteLine("Exit code: invalid...");
					return (int) ExitCode.InvalidLoginPasswordOrWebApiKey;
				}
			}
			else
			{
				var allegroFile     = new AllegroFile(searchFile, resultFile);
				var allegroDatabase = allegroFile.GetDatabase();
				var allegroResults  = allegroFile.GetResults();

				AllegroClient.WebApiKey = webapikey;
				allegroClient = new AllegroClient(user, password, false);

				var cleanedResults = new List<AllegroFileResult>();

				foreach (var search in allegroDatabase)
				{
					var tempSearchResults = allegroClient.Search(search.Title, search.Category, search.Price);
					cleanedResults.AddRange(AllegroChecker.FindDuplicatesAndCreateLinks(tempSearchResults, allegroResults, search));
				}

				for (var i = 0; i < cleanedResults.Count; i++)
				{
					cleanedResults[i].ID = i + 1;
				}

				allegroFile.WriteResults(cleanedResults);

				if (email != string.Empty)
				{
					SendEmail(email);
				}
			}

			Console.WriteLine("Exit code: success");
			return (int) ExitCode.Success;
		}

		private static void SendEmail(string emailTo)
		{
			try
			{
				var mail = new MailMessage("allegroparser@gmail.com", emailTo);
				var smtp = new SmtpClient("smtp.gmail.com");

				mail.Subject = "Znaleziono nowe aukcje spełniające podane kryteria";
				mail.Body = "Sprawdź aukcje w miejscu gdzie zapisujesz wybrane e-mail'e\r\n\r\nPozdrawiam\r\ndev4s";

				smtp.Port = 587;
				smtp.Credentials = new NetworkCredential("allegroparser@gmail.com", "allegroparser!@#");
				smtp.EnableSsl = true;
				smtp.Send(mail);
			}
			catch (Exception e)
			{
				//do nothing with it...
			}
		}
	}
}
