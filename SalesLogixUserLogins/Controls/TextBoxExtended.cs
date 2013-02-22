using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace FX.SalesLogix.Utility.UserLogins.Controls
{
	public partial class TextBoxExtended : TextBox
	{
		private string _watermark;
		private Color _watermarkColor = Color.Silver;
		private Color _foreColor = SystemColors.ControlText;
		private bool _ispassword = false;
		private bool _empty;

		public TextBoxExtended()
		{
			_empty = true;
			_foreColor = ForeColor;
		}

		public Color WatermarkColor
		{
			get { return _watermarkColor; }
			set
			{
				_watermarkColor = value;
				if (_empty)
				{
					base.ForeColor = _watermarkColor;
				}
			}
		}

		public string WatermarkText
		{
			get { return _watermark; }
			set
			{
				_watermark = value;
				if (_empty)
				{
					base.Text = _watermark;
					base.ForeColor = _watermarkColor;
				}
			}
		}

		public new Color ForeColor
		{
			get { return _foreColor; }
			set
			{
				_foreColor = value;
				if (!_empty)
					base.ForeColor = value;
			}
		}

		public override string Text
		{
			get
			{
				if (_empty) return "";
				return base.Text;
			}
			set
			{
				if (value == "")
				{
					_empty = true;
					base.ForeColor = _watermarkColor;
					base.Text = _watermark;
					//if (this.Handle != IntPtr.Zero) base.UseSystemPasswordChar = false;
				}
				else
				{
					_empty = false;
					base.ForeColor = _foreColor;
					base.Text = value;
					//if (this.Handle != IntPtr.Zero) base.UseSystemPasswordChar = ispassword;
				}
			}
		}

		public bool IsPassword
		{
			get { return this._ispassword; }
			set
			{
				this._ispassword = value;
				//if (this.Handle != IntPtr.Zero) base.UseSystemPasswordChar = (ispassword && !empty);
			}
		}

		protected override void OnGotFocus(EventArgs e)
		{
			if (_empty)
			{
				_empty = false;
				base.ForeColor = _foreColor;
				base.Text = "";
				//if (this.Handle != IntPtr.Zero) base.UseSystemPasswordChar = ispassword;
			}
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if (base.Text == "")
			{
				_empty = true;
				base.ForeColor = _watermarkColor;
				base.Text = _watermark;
				//if (this.Handle != IntPtr.Zero) base.UseSystemPasswordChar = false;
			}
			else
				_empty = false;
		}
	}
}
