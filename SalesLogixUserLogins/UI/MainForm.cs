using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using FX.SalesLogix.Utility.UserLogins.Managers;
using FX.SalesLogix.Utility.UserLogins.Model;

namespace FX.SalesLogix.Utility.UserLogins.UI
{
	public partial class MainForm : Form
	{
		public const string _DATABASENOTSALESLOGIX = "Not a SalesLogix database. Select another.";
		public const string _NOPASSWORD = "Enter a SQL password to continue.";
		public const string _GENERALERROR = "Error: {0}";

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			this.Cursor = Cursors.AppStarting;
			new Thread(() =>
			{
				var sqlManager = new SqlManager();
				foreach (var server in sqlManager.GetServers())
				{
					ServerCombo.SafeThreadAction(x => x.Items.Add(server));
				}
				this.SafeThreadAction(x => x.Cursor = Cursors.Default);
			}).Start();
		}

		private void ServerCombo_TextChanged(object sender, EventArgs e)
		{
			DatabaseCombo.Items.Clear();
			StartButton.Enabled = false;
			ErrorLabel.Text = string.Empty;
		}

		private void DatabaseCombo_DropDown(object sender, EventArgs e)
		{
			ErrorLabel.Text = string.Empty;

			if (ServerCombo.Text.Trim() == string.Empty 
				|| UserTextBox.Text.Trim() == string.Empty
				) return;

			if (PasswordTextBox.Text.Trim() == string.Empty)
			{
				ErrorLabel.Text = _NOPASSWORD;
				DatabaseCombo.DroppedDown = false;
				return;
			}

			this.Cursor = Cursors.WaitCursor;

			try
			{
				var sqlManager = new SqlManager();
				var databases = sqlManager.GetDatabases(ServerCombo.Text, UserTextBox.Text, PasswordTextBox.Text);
				foreach (var database in databases)
				{
					DatabaseCombo.Items.Add(database);
				}
			}
			catch (Exception exception)
			{
				ErrorLabel.Text = string.Format(_GENERALERROR, exception.Message);
			}

			this.Cursor = Cursors.Default;
		}

		private void DatabaseCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (DatabaseCombo.SelectedIndex == -1) return;

			var sqlManager = new SqlManager();
			if (sqlManager.IsSalesLogixDatabase(DatabaseCombo.Text, ServerCombo.Text, UserTextBox.Text, PasswordTextBox.Text))
			{
				StartButton.Enabled = true;
				ErrorLabel.Text = string.Empty;
			}
			else
			{
				StartButton.Enabled = false;
				ErrorLabel.Text = _DATABASENOTSALESLOGIX;
			}
		}

		private void StartButton_Click(object sender, EventArgs e)
		{
			DataListView.Items.Clear();

			try
			{
				Cursor = Cursors.WaitCursor;

				var sqlManager = new SqlManager();
				var logins = sqlManager.GetLogins(ServerCombo.Text, DatabaseCombo.Text, UserTextBox.Text, PasswordTextBox.Text);
				ResultsLabel.Text = string.Format("{0} user logins retrieved.", logins.Count);

				foreach (var userLogin in logins)
				{
					var listItem = new ListViewItem();
					listItem.Text = userLogin.User;
					listItem.Tag = userLogin.ID;
					listItem.SubItems.Add(userLogin.LoginName);
					listItem.SubItems.Add(userLogin.Password);

					DataListView.Items.Add(listItem);
				}
				DataListView.Tag = logins;
			}
			catch (Exception exception)
			{
				ErrorLabel.Text = string.Format(_GENERALERROR, exception.Message);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void ExportButton_Click(object sender, EventArgs e)
		{
			if (DataListView.Items.Count == 0)
				return;

			using (var dlg = new SaveFileDialog())
			{
				dlg.Title = "Export the user login list";
				dlg.Filter = "Comma-Delimeted File (*.CSV)|*.csv";
				dlg.FileName = DatabaseCombo.Text + " User Logins.csv";
				dlg.OverwritePrompt = true;

				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					try
					{
						ExportManager.Export(dlg.FileName, (List<UserLogin>) DataListView.Tag);
						MessageBox.Show(this, "User login list exported successfully.", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					catch (Exception exception)
					{
						MessageBox.Show(this, exception.Message, "Error Exporting File", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private void WebLinkLabel_Click(object sender, EventArgs e)
		{
			try
			{
				Process.Start("http://customerfx.com");
			}
			catch { }
		}
	}
}
