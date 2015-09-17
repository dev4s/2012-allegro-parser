namespace APForm
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonSearch = new System.Windows.Forms.Button();
			this.buttonResult = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioButtonDaily = new System.Windows.Forms.RadioButton();
			this.comboBoxMinutesHoursDays = new System.Windows.Forms.ComboBox();
			this.radioButtonWeekly = new System.Windows.Forms.RadioButton();
			this.radioButtonMonthly = new System.Windows.Forms.RadioButton();
			this.comboBoxCounter = new System.Windows.Forms.ComboBox();
			this.radioButtonAsThe = new System.Windows.Forms.RadioButton();
			this.textBoxResultFile = new System.Windows.Forms.TextBox();
			this.textBoxSearchFile = new System.Windows.Forms.TextBox();
			this.buttonClearTask = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxEmail = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonSearch
			// 
			this.buttonSearch.Location = new System.Drawing.Point(490, 10);
			this.buttonSearch.Name = "buttonSearch";
			this.buttonSearch.Size = new System.Drawing.Size(165, 23);
			this.buttonSearch.TabIndex = 1;
			this.buttonSearch.Text = "Ścieżka do pliku wstępnego";
			this.buttonSearch.UseVisualStyleBackColor = true;
			this.buttonSearch.Click += new System.EventHandler(this.ButtonSearchClick);
			// 
			// buttonResult
			// 
			this.buttonResult.Location = new System.Drawing.Point(490, 39);
			this.buttonResult.Name = "buttonResult";
			this.buttonResult.Size = new System.Drawing.Size(165, 23);
			this.buttonResult.TabIndex = 3;
			this.buttonResult.Text = "Ścieżka do pliku wynikowego";
			this.buttonResult.UseVisualStyleBackColor = true;
			this.buttonResult.Click += new System.EventHandler(this.ButtonResultClick);
			// 
			// buttonOk
			// 
			this.buttonOk.Location = new System.Drawing.Point(499, 159);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 4;
			this.buttonOk.Text = "OK";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.ButtonOkClick);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(580, 159);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 5;
			this.buttonCancel.Text = "Anuluj";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
			// 
			// openFileDialog
			// 
			this.openFileDialog.FileName = "openFileDialog";
			this.openFileDialog.Filter = "plik CSV|*.csv";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioButtonDaily);
			this.groupBox1.Controls.Add(this.comboBoxMinutesHoursDays);
			this.groupBox1.Controls.Add(this.radioButtonWeekly);
			this.groupBox1.Controls.Add(this.radioButtonMonthly);
			this.groupBox1.Controls.Add(this.comboBoxCounter);
			this.groupBox1.Controls.Add(this.radioButtonAsThe);
			this.groupBox1.Location = new System.Drawing.Point(12, 67);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(244, 116);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Jak często ma być wykonywane zadanie";
			// 
			// radioButtonDaily
			// 
			this.radioButtonDaily.AutoSize = true;
			this.radioButtonDaily.Location = new System.Drawing.Point(13, 19);
			this.radioButtonDaily.Name = "radioButtonDaily";
			this.radioButtonDaily.Size = new System.Drawing.Size(134, 17);
			this.radioButtonDaily.TabIndex = 0;
			this.radioButtonDaily.Text = "Dziennie - o godz. 8.00";
			this.radioButtonDaily.UseVisualStyleBackColor = true;
			this.radioButtonDaily.CheckedChanged += new System.EventHandler(this.RadioButtonsCheckedChanged);
			// 
			// comboBoxMinutesHoursDays
			// 
			this.comboBoxMinutesHoursDays.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::APForm.Properties.Settings.Default, "RadioButtonAsThe", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.comboBoxMinutesHoursDays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxMinutesHoursDays.Enabled = global::APForm.Properties.Settings.Default.RadioButtonAsThe;
			this.comboBoxMinutesHoursDays.FormattingEnabled = true;
			this.comboBoxMinutesHoursDays.Items.AddRange(new object[] {
            "minut/ę",
            "godzin/ę",
            "dzień/dni"});
			this.comboBoxMinutesHoursDays.Location = new System.Drawing.Point(123, 87);
			this.comboBoxMinutesHoursDays.Name = "comboBoxMinutesHoursDays";
			this.comboBoxMinutesHoursDays.Size = new System.Drawing.Size(96, 21);
			this.comboBoxMinutesHoursDays.TabIndex = 5;
			this.comboBoxMinutesHoursDays.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSelectedIndexChanged);
			// 
			// radioButtonWeekly
			// 
			this.radioButtonWeekly.AutoSize = true;
			this.radioButtonWeekly.Location = new System.Drawing.Point(13, 42);
			this.radioButtonWeekly.Name = "radioButtonWeekly";
			this.radioButtonWeekly.Size = new System.Drawing.Size(226, 17);
			this.radioButtonWeekly.TabIndex = 1;
			this.radioButtonWeekly.Text = "Tygodniowo - w poniedziałek o godz. 8.00";
			this.radioButtonWeekly.UseVisualStyleBackColor = true;
			this.radioButtonWeekly.CheckedChanged += new System.EventHandler(this.RadioButtonsCheckedChanged);
			// 
			// radioButtonMonthly
			// 
			this.radioButtonMonthly.AutoSize = true;
			this.radioButtonMonthly.Location = new System.Drawing.Point(13, 65);
			this.radioButtonMonthly.Name = "radioButtonMonthly";
			this.radioButtonMonthly.Size = new System.Drawing.Size(206, 17);
			this.radioButtonMonthly.TabIndex = 2;
			this.radioButtonMonthly.Text = "Miesięcznie - 1 dnia każdego miesiąca";
			this.radioButtonMonthly.UseVisualStyleBackColor = true;
			this.radioButtonMonthly.CheckedChanged += new System.EventHandler(this.RadioButtonsCheckedChanged);
			// 
			// comboBoxCounter
			// 
			this.comboBoxCounter.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::APForm.Properties.Settings.Default, "RadioButtonAsThe", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.comboBoxCounter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCounter.Enabled = global::APForm.Properties.Settings.Default.RadioButtonAsThe;
			this.comboBoxCounter.FormattingEnabled = true;
			this.comboBoxCounter.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59"});
			this.comboBoxCounter.Location = new System.Drawing.Point(60, 87);
			this.comboBoxCounter.Name = "comboBoxCounter";
			this.comboBoxCounter.Size = new System.Drawing.Size(57, 21);
			this.comboBoxCounter.TabIndex = 4;
			this.comboBoxCounter.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSelectedIndexChanged);
			// 
			// radioButtonAsThe
			// 
			this.radioButtonAsThe.AutoSize = true;
			this.radioButtonAsThe.Location = new System.Drawing.Point(13, 88);
			this.radioButtonAsThe.Name = "radioButtonAsThe";
			this.radioButtonAsThe.Size = new System.Drawing.Size(41, 17);
			this.radioButtonAsThe.TabIndex = 3;
			this.radioButtonAsThe.Text = "Co:";
			this.radioButtonAsThe.UseVisualStyleBackColor = true;
			this.radioButtonAsThe.CheckedChanged += new System.EventHandler(this.RadioButtonsCheckedChanged);
			// 
			// textBoxResultFile
			// 
			this.textBoxResultFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APForm.Properties.Settings.Default, "ResultFullPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBoxResultFile.Location = new System.Drawing.Point(12, 41);
			this.textBoxResultFile.Name = "textBoxResultFile";
			this.textBoxResultFile.Size = new System.Drawing.Size(472, 20);
			this.textBoxResultFile.TabIndex = 2;
			this.textBoxResultFile.Text = global::APForm.Properties.Settings.Default.ResultFullPath;
			// 
			// textBoxSearchFile
			// 
			this.textBoxSearchFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APForm.Properties.Settings.Default, "SearchFullPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBoxSearchFile.Location = new System.Drawing.Point(12, 12);
			this.textBoxSearchFile.Name = "textBoxSearchFile";
			this.textBoxSearchFile.Size = new System.Drawing.Size(472, 20);
			this.textBoxSearchFile.TabIndex = 0;
			this.textBoxSearchFile.Text = global::APForm.Properties.Settings.Default.SearchFullPath;
			// 
			// buttonClearTask
			// 
			this.buttonClearTask.Location = new System.Drawing.Point(262, 159);
			this.buttonClearTask.Name = "buttonClearTask";
			this.buttonClearTask.Size = new System.Drawing.Size(110, 23);
			this.buttonClearTask.TabIndex = 7;
			this.buttonClearTask.Text = "Usuń zadanie";
			this.buttonClearTask.UseVisualStyleBackColor = true;
			this.buttonClearTask.Click += new System.EventHandler(this.ButtonClearTaskClick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(262, 74);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(353, 13);
			this.label1.TabIndex = 8;
			this.label1.Text = "Adres e-mail, na który zostanie wysłane potwierdzenie wyszukania aukcji:";
			// 
			// textBoxEmail
			// 
			this.textBoxEmail.Location = new System.Drawing.Point(264, 92);
			this.textBoxEmail.Name = "textBoxEmail";
			this.textBoxEmail.Size = new System.Drawing.Size(391, 20);
			this.textBoxEmail.TabIndex = 9;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.ClientSize = new System.Drawing.Size(667, 193);
			this.Controls.Add(this.textBoxEmail);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonClearTask);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.buttonResult);
			this.Controls.Add(this.textBoxResultFile);
			this.Controls.Add(this.buttonSearch);
			this.Controls.Add(this.textBoxSearchFile);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Ustawienia zadania wyszukiwania";
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxSearchFile;
		private System.Windows.Forms.Button buttonSearch;
		private System.Windows.Forms.TextBox textBoxResultFile;
		private System.Windows.Forms.Button buttonResult;
		private System.Windows.Forms.RadioButton radioButtonDaily;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.RadioButton radioButtonMonthly;
		private System.Windows.Forms.RadioButton radioButtonWeekly;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.ComboBox comboBoxMinutesHoursDays;
		private System.Windows.Forms.ComboBox comboBoxCounter;
		private System.Windows.Forms.RadioButton radioButtonAsThe;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonClearTask;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxEmail;
	}
}

