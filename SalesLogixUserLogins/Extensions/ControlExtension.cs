using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* usage

SET 

lblProcent.SafeThreadAction(d => d.Text = "Written by the background thread");
progressBar1.SafeThreadAction(d => d.Value = i);

//or call a methode thread safe. That method is executed on the same thread as the form
this.SafeThreadAction(d => d.UpdateFormItems("test1", "test2"));

GET 

string textboxtext = textbox.SafeThreadGet(d=>d.textbox.Text);

*/

namespace System.Windows.Forms
{
	public static class ControlExtension
	{
		public static void SafeThreadAction<T>(this T control, Action<T> call) where T : Control
		{
			if (control.IsHandleCreated && control.InvokeRequired)
				control.Invoke(call, control);
			else
				call(control);
		}

		public static Y SafeThreadGet<Y, T>(this T control, Func<T, Y> call) where T : Control
		{
			IAsyncResult result = control.BeginInvoke(call, control);
			object result2 = control.EndInvoke(result);
			return (Y)result2;
		}
	}
}