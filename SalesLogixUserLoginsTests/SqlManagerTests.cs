using System;
using FX.SalesLogix.Utility.UserLogins.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SalesLogixUserLoginsTests
{
	[TestClass]
	public class SqlManagerTests
	{
		[TestMethod]
		public void Can_Get_SQL_Servers()
		{
			var sqlManager = new SqlManager();
			var servers = sqlManager.GetServers();

			Assert.IsTrue(servers.Contains(Environment.MachineName));
		}

		[TestMethod]
		public void Can_Get_SQL_Databases()
		{
			var sqlManager = new SqlManager();
			var databases = sqlManager.GetDatabases(Environment.MachineName, "sysdba", "masterkey");

			Assert.IsTrue(databases.Contains("saleslogix_eval"));
		}

		[TestMethod]
		public void Can_Test_If_Database_Is_SalesLogix_Database()
		{
			var sqlManager = new SqlManager();
			Assert.IsTrue(sqlManager.IsSalesLogixDatabase("saleslogix_eval", Environment.MachineName, "sysdba", "masterkey"));
		}
}
}
