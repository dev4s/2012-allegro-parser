using System.ComponentModel;
using System.Windows.Forms;

namespace APForm
{
	public partial class LoginBox : Form
	{
		public LoginBox()
		{
			InitializeComponent();
		}

		public string Password
		{
			get { return textBoxPassword.Text; }
		}

		public string Login
		{
			get { return textBoxLogin.Text; }
		}

		public ProgressBarStyle ProgressBarStyle
		{
			get { return progressBar.Style; }
			set { progressBar.Style = value; }
		}

		public BackgroundWorker BackgroundWorker
		{
			get { return backgroundWorker; }
		}
	}
}
