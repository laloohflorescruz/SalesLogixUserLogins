using System.Collections.Generic;
using System.IO;
using FX.SalesLogix.Utility.UserLogins.Model;

namespace FX.SalesLogix.Utility.UserLogins.Managers
{
	public class ExportManager
	{
		public static void Export(string fileName, List<UserLogin> userLogins)
		{
			using (var outFile = new StreamWriter(fileName, false))
			{
				outFile.WriteLine("\"User ID\",\"User\",\"Login Name\",\"Password\"");
				foreach (var login in userLogins)
				{
					outFile.WriteLine("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", login.ID, login.User, login.LoginName, login.Password);
				}
			}
		}
	}
}
