using System;
using FX.SalesLogix.Utility.UserLogins.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SalesLogixUserLoginsTests
{
	[TestClass]
	public class EncryptionManagerTests
	{
		[TestMethod]
		public void Can_Encrypt_And_Decrypt_Values()
		{
			const string origValue = "Testing";

			string encValue = EncryptionManager.Encrypt(origValue, "KEY");
			string decValue = EncryptionManager.Decrypt(encValue, "KEY");

			Assert.IsTrue(origValue == decValue);
		}

		[TestMethod]
		public void Can_Decrypt_Password()
		{
			const string encryptedPassword = "072ED70ECF7AADFD2E7EAEFE592C086DC0AF8E9E8E9E8E9EF84C280D60CFAE";
			const string userId = "UDEMOA00000F";

			string password = EncryptionManager.Decrypt(encryptedPassword, userId);

			Assert.IsTrue(password == "tester");
		}
	}
}
