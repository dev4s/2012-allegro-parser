using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using APForm.Properties;
using Microsoft.Win32.TaskScheduler;

namespace APForm
{
	public partial class MainForm : Form
	{
		enum ExitCode
		{
			Success = 0,
			InvalidLoginPasswordOrWebApiKey = 10
		}

		private const string WebApiKey = "76f7d4722f";

		private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			MessageBox.Show(((Exception) e.ExceptionObject).Message, "Coś się... popsuło ;)");
		}

		private static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.Message, "Coś się... popsuło ;)");
		}

		public MainForm()
		{
			AppDomain.CurrentDomain.UnhandledException += UnhandledException;
			Application.ThreadException += ApplicationThreadException;

			InitializeComponent();

			#region Show loginbox on first time use
			//Checking if this application is first runned
			if (!Settings.Default.FirstRun) return;

			while (Settings.Default.FirstRun)
			{
				var loginBox = new LoginBox();
				//loginBox.BackgroundWorker.DoWork += BackgroundWorkerDoWork;
				//loginBox.BackgroundWorker.RunWorkerCompleted += BackgroundWorkerRunWorkerCompleted;

				var loginBoxResult = loginBox.ShowDialog();
				if (loginBoxResult == DialogResult.OK)
				{
					var credentials = string.Format("-check -u {0} -p {1} -api {2}",
													loginBox.Login,
													loginBox.Password,
													WebApiKey);

					switch (RunApConsoleProcess(credentials))
					{
						case (int)ExitCode.InvalidLoginPasswordOrWebApiKey:
							ShowMessage("Podane dane logowania są nieprawidłowe (Nazwa użytkownika, hasło lub klucz WebApi)");
							break;

						case (int)ExitCode.Success:
							//loginCredentialsAreGood = true;
							Settings.Default.FirstRun = false;
							Settings.Default.Password = Password.Encrypt(loginBox.Password);
							Settings.Default.Login = Password.Encrypt(loginBox.Login);
							Settings.Default.WebApiKey = Password.Encrypt(WebApiKey);
							Settings.Default.Save();

							break;
					}
				}
				else
				{
					ShowMessage("Nie można uruchomić aplikacji bez podania danych logowania.");
					Close();
					return;
				}


				//loginBox.ProgressBarStyle = ProgressBarStyle.Marquee;
				//loginBox.BackgroundWorker.RunWorkerAsync(_credentials);

				
			}
			#endregion
		}

		private void BackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			//How to loginbox?
			throw new NotImplementedException();
		}

		private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
		{
			var credentials = (string) e.Argument;

			var checkProc = new ProcessStartInfo("APConsole.exe", credentials)
			{
				UseShellExecute = false,
				CreateNoWindow = true
			};

			var checkProcRun = Process.Start(checkProc);

			while (!checkProcRun.HasExited)
			{
				Thread.Sleep(500);
			}
		}

		private static void ShowMessage(string message)
		{
			MessageBox.Show(message,
// ReSharper disable LocalizableElement
			                "Informacja",
// ReSharper restore LocalizableElement
			                MessageBoxButtons.OK,
			                MessageBoxIcon.Information);
		}

		private static int RunApConsoleProcess(string arguments)
		{
			var checkProc = new ProcessStartInfo("APConsole.exe", arguments)
								{
									UseShellExecute = false,
									CreateNoWindow = true
								};

			var checkProcRun = Process.Start(checkProc);

			while (!checkProcRun.HasExited)
			{
				Thread.Sleep(100);
			}

			return checkProcRun.ExitCode;
		}

		private void ButtonSearchClick(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog() != DialogResult.OK) return;

			Settings.Default.SearchFullPath = openFileDialog.FileName;
			Settings.Default.Save();

			textBoxSearchFile.Text = Settings.Default.SearchFullPath;
		}

		private void ButtonResultClick(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog() != DialogResult.OK) return;

			Settings.Default.ResultFullPath = openFileDialog.FileName;
			Settings.Default.Save();

			textBoxResultFile.Text = Settings.Default.ResultFullPath;
		}

		private void ButtonOkClick(object sender, EventArgs e)
		{
			Settings.Default.Email = textBoxEmail.Text;
			Settings.Default.Save();

			using (var task = new TaskService())
			{
				var foundTask = task.FindTask("AllegroParser");

				if (foundTask != null)
				{
					task.RootFolder.DeleteTask(foundTask.Name);
				}

				var taskDef = task.NewTask();
				taskDef.RegistrationInfo.Author = "dev4s";
				taskDef.RegistrationInfo.Description = "This is a task for parsing allegro data.";
				taskDef.RegistrationInfo.Date = DateTime.Now;
				taskDef.Principal.LogonType = TaskLogonType.InteractiveToken;

				var file = new FileInfo("APConsole.exe");
				var args = string.Format("-u {0} -p {1} -api {2} -sF {3} -rF {4} -em {5}",
				                         Password.Decrypt(Settings.Default.Login),
				                         Password.Decrypt(Settings.Default.Password),
				                         Password.Decrypt(Settings.Default.WebApiKey),
				                         Settings.Default.SearchFullPath,
				                         Settings.Default.ResultFullPath,
										 Settings.Default.Email
					);


				taskDef.Actions.Add(new ExecAction(file.FullName, args));

				if (Settings.Default.RadioButtonDaily)
				{
					var dailyTrigger = new DailyTrigger
										{
											StartBoundary = DateTime.Today + TimeSpan.FromHours(8)
										};

					taskDef.Triggers.Add(dailyTrigger);
				}
				else if (Settings.Default.RadioButtonWeekly)
				{
					var weeklyTrigger = new WeeklyTrigger(DaysOfTheWeek.Monday)
					                    {
					                    	StartBoundary = DateTime.Today + TimeSpan.FromHours(8)
					                    };

					taskDef.Triggers.Add(weeklyTrigger);
				}
				else if (Settings.Default.RadioButtonMonthly)
				{
					var monthlyTrigger = new MonthlyTrigger()
					                    {
											StartBoundary = DateTime.Today + TimeSpan.FromHours(8)
					                    };

					taskDef.Triggers.Add(monthlyTrigger);
				}
				else if (Settings.Default.RadioButtonAsThe)
				{
					var counter = int.Parse(comboBoxCounter.SelectedItem.ToString());
					var daysHoursCounter = comboBoxMinutesHoursDays.SelectedIndex;

					var asTrigger = new DailyTrigger
									{
										StartBoundary = DateTime.Now + TimeSpan.FromMinutes(5)
									};

					switch (daysHoursCounter)
					{
						//co minutę
						case 0:
							asTrigger.Repetition.Interval = TimeSpan.FromMinutes(counter <= 10 ? 10 : counter);
							break;

						//co godzinę
						case 1:
							asTrigger.Repetition.Interval = TimeSpan.FromHours(counter);
							break;

						//co ileś tam dni
						case 2:
							asTrigger.Repetition.Interval = TimeSpan.FromDays(counter);
							break;
					}

					taskDef.Triggers.Add(asTrigger);
				}

				task.RootFolder.RegisterTaskDefinition("AllegroParser", taskDef);
			}

			MessageBox.Show("Utworzono zadanie w harmonogramie zadań!");
		}

		private void ButtonCancelClick(object sender, EventArgs e)
		{
			Close();
		}

		private void RadioButtonsCheckedChanged(object sender, EventArgs e)
		{
			var radioButton = (RadioButton) sender;

			switch (radioButton.Name)
			{
				case "radioButtonDaily":
					Settings.Default.RadioButtonDaily = true;
					Settings.Default.RadioButtonAsThe = false;
					comboBoxCounter.Enabled = false;
					comboBoxMinutesHoursDays.Enabled = false;
					Settings.Default.RadioButtonMonthly = false;
					Settings.Default.RadioButtonWeekly = false;
					break;

				case "radioButtonMonthly":
					Settings.Default.RadioButtonDaily = false;
					Settings.Default.RadioButtonAsThe = false;
					comboBoxCounter.Enabled = false;
					comboBoxMinutesHoursDays.Enabled = false;
					Settings.Default.RadioButtonMonthly = true;
					Settings.Default.RadioButtonWeekly = false;
					break;

				case "radioButtonAsThe":
					Settings.Default.RadioButtonDaily = false;
					Settings.Default.RadioButtonAsThe = true;
					comboBoxCounter.Enabled = true;
					comboBoxMinutesHoursDays.Enabled = true;
					Settings.Default.RadioButtonMonthly = false;
					Settings.Default.RadioButtonWeekly = false;
					break;

				case "radioButtonWeekly":
					Settings.Default.RadioButtonDaily = false;
					Settings.Default.RadioButtonAsThe = false;
					comboBoxCounter.Enabled = false;
					comboBoxMinutesHoursDays.Enabled = false;
					Settings.Default.RadioButtonMonthly = false;
					Settings.Default.RadioButtonWeekly = true;
					break;
			}

			Settings.Default.Save();
		}

		private void MainFormLoad(object sender, EventArgs e)
		{
			radioButtonDaily.Checked = Settings.Default.RadioButtonDaily;
			radioButtonAsThe.Checked = Settings.Default.RadioButtonAsThe;
			if (Settings.Default.RadioButtonAsThe)
			{
				comboBoxCounter.Enabled = true;
				comboBoxMinutesHoursDays.Enabled = true;
				comboBoxCounter.SelectedIndex = Settings.Default.ComboBoxCounter;
				comboBoxMinutesHoursDays.SelectedIndex = Settings.Default.ComboBoxHourDay;
			}
			else
			{
				comboBoxCounter.Enabled = false;
				comboBoxMinutesHoursDays.Enabled = false;
			}
			radioButtonMonthly.Checked = Settings.Default.RadioButtonMonthly;
			radioButtonWeekly.Checked = Settings.Default.RadioButtonWeekly;
			textBoxEmail.Text = Settings.Default.Email;
		}

		private void ComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			var comboBox = (ComboBox) sender;

			switch (comboBox.Name)
			{
				case "comboBoxCounter":
					Settings.Default.ComboBoxCounter = comboBoxCounter.SelectedIndex;
					break;

				case "comboBoxMinutesHoursDays":
					Settings.Default.ComboBoxHourDay = comboBoxMinutesHoursDays.SelectedIndex;
					break;
			}

			Settings.Default.Save();
		}

		private void ButtonClearTaskClick(object sender, EventArgs e)
		{
			using (var task = new TaskService())
			{
				var foundTask = task.FindTask("AllegroParser");

				if (foundTask != null)
				{
					task.RootFolder.DeleteTask(foundTask.Name);
				}
			}

			MessageBox.Show("Usunięto zadanie!");
		}
	}
}
