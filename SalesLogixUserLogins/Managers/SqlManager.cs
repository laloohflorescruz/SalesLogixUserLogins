using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using FX.SalesLogix.Utility.UserLogins.Model;

namespace FX.SalesLogix.Utility.UserLogins.Managers
{
	public class SqlManager
	{
		public List<string> GetServers()
		{
			var list = new List<string>();

			var servers = SqlDataSourceEnumerator.Instance.GetDataSources();
			foreach (DataRow server in servers.Rows)
			{
				if (!string.IsNullOrEmpty(server["InstanceName"] as string))
					list.Add(server["ServerName"] + "\\" + server["InstanceName"]);
				else
					list.Add(server["ServerName"].ToString());
			}

			return list;
		}

		public List<string> GetDatabases(string server, string username, string password)
		{
			var list = new List<string>();

			using (var conn = new SqlConnection(string.Format("server={0};uid={1};pwd={2}", server, username, password)))
			{
				conn.Open();
				
				var databases = conn.GetSchema("Databases");
				list.AddRange(from DataRow row in databases.Rows select row["database_name"].ToString());
			}

			return list;
		}

		public bool IsSalesLogixDatabase(string database, string server, string username, string password)
		{
			try
			{
				using (var conn = new SqlConnection(string.Format("server={0};database={1};uid={2};pwd={3}", server, database, username, password)))
				{
					conn.Open();

					var tables = conn.GetSchema("Tables");
					return tables.Rows.Cast<DataRow>().Any(row => row[2].ToString().ToUpper().Equals("CALCULATEDFIELDDATA"));
				}
			}
			catch
			{
				return false;
			}
		}

		public List<UserLogin> GetLogins(string server, string database, string username, string password)
		{
			var list = new List<UserLogin>();

			using (var conn =new SqlConnection(string.Format("server={0};database={1};uid={2};pwd={3}", server, database, username, password)))
			{
				conn.Open();

				const string sql = "SELECT usersec.USERID, usersec.USERCODE, usersec.USERPW, userinf.LASTNAME, userinf.FIRSTNAME " +
				                   "FROM sysdba.USERSECURITY usersec " +
				                   "INNER JOIN sysdba.USERINFO userinf ON usersec.USERID = userinf.USERID " +
				                   "WHERE usersec.TYPE NOT IN ('P','R') " +
				                   "ORDER BY USERCODE";

				using (var cmd = new SqlCommand(sql, conn))
				{
					using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
					{
						while (reader.Read())
						{
							var login = new UserLogin()
								{
									ID = reader["USERID"].ToString(),
									User = GetFullName(reader["FIRSTNAME"].ToString(), reader["LASTNAME"].ToString()),
									LoginName = reader["USERCODE"].ToString(),
									Password = EncryptionManager.Decrypt(reader["USERPW"].ToString(), reader["USERID"].ToString())
								};
							list.Add(login);
						}
					}
				}
			}

			return list;
		}

		private static string GetFullName(string firstName, string lastName)
		{
			var fullName = string.Empty;

			fullName = lastName.Trim();
			if (fullName != string.Empty && firstName != string.Empty)
				fullName += ", ";
			
			fullName += firstName;

			return fullName.Trim();
		}
	}
}
