using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FX.SalesLogix.Utility.UserLogins.UI
{
    public partial class LoginDetail : Form
    {
        public LoginDetail(string Name, string User, string Password, string UserID)
        {
            InitializeComponent();

            LabelUserLogin.Text = "User login for " + Name;
            UserTextBox.Text = User;
            PasswordTextBox.Text = Password;
            UserIDLabel.Text = "User ID: " + UserID;
        }
    }
}
