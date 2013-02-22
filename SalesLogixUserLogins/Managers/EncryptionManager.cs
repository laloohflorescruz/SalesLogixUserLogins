using System;
using System.Globalization;

namespace FX.SalesLogix.Utility.UserLogins.Managers
{
	public class EncryptionManager
	{
		public static string Decrypt(string srcValue)
		{
			return Decrypt(srcValue, null);
		}

		public static string Decrypt(string srcValue, string key)
		{
			string str = string.Empty;
			srcValue = srcValue.Trim();
			key = key.Trim();

			if ((key == null) || (key.Length == 0))
			{
				key = "SLSLGX";
			}
			int length = key.Length;
			int startIndex = 0;
			if ((srcValue != null) && (srcValue.Length > 2))
			{
				int num3;
				int.TryParse(srcValue.Substring(0, 2), NumberStyles.HexNumber, null, out num3);
				int num4 = srcValue.Length;
				int num5 = 2;
				while (num5 < num4)
				{
					int num6;
					int.TryParse(srcValue.Substring(num5, 2), NumberStyles.HexNumber, null, out num6);
					num5 += 2;
					int num7 = num6 ^ key.Substring(startIndex, 1).ToCharArray()[0];
					startIndex++;
					if (startIndex >= length)
					{
						startIndex = 0;
					}
					if (num7 <= num3)
					{
						num7 = (0xff + num7) - num3;
					}
					else
					{
						num7 -= num3;
					}
					str = str + ((char) num7);
					num3 = num6;
				}
			}
			return str.Trim();
		}

		public static string Encrypt(string srcValue)
		{
			return Encrypt(srcValue, null);
		}

		public static string Encrypt(string srcValue, string key)
		{
			string str = string.Empty;
			srcValue = srcValue.Trim();
			key = key.Trim();

			if ((srcValue != null) && (srcValue.Length > 0))
			{
				if ((key == null) || (key.Length == 0))
				{
					key = "SLSLGX";
				}
				int length = key.Length;
				int num2 = 0;
				int maxValue = 0x100;
				int num4 = new Random().Next(maxValue);
				str = string.Format("{0:X2}", num4);
				int num5 = srcValue.Length;
				for (int i = 0; i < num5; i++)
				{
					int num7 = (((byte) srcValue[i]) + num4) % 0xff;
					num7 ^= (byte) key[num2];
					num2++;
					if (num2 == length)
					{
						num2 = 0;
					}
					str = str + string.Format("{0:X2}", num7);
					num4 = num7;
				}
			}
			return str.Trim();
		}
	}
}
