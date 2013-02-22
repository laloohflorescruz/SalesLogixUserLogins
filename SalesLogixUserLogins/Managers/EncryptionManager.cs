using System;
using System.Globalization;

namespace FX.SalesLogix.Utility.UserLogins.Managers
{
	public class EncryptionManager
	{
		public static string Decrypt(string sourceValue)
		{
			return Decrypt(sourceValue, null);
		}

		public static string Decrypt(string sourceValue, string key)
		{
			string newValue = string.Empty;
			sourceValue = (sourceValue ?? string.Empty).Trim();
			key = (key ?? string.Empty).Trim();

			if (string.IsNullOrEmpty(key))
				key = "SLSLGX";
			
			int length = key.Length;
			int startIndex = 0;

			if (sourceValue.Length > 2)
			{
				int num3;
				int.TryParse(sourceValue.Substring(0, 2), NumberStyles.HexNumber, null, out num3);
				int num4 = sourceValue.Length;
				int num5 = 2;

				while (num5 < num4)
				{
					int num6;
					int.TryParse(sourceValue.Substring(num5, 2), NumberStyles.HexNumber, null, out num6);
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

					newValue = newValue + ((char) num7);
					num3 = num6;
				}
			}
			return newValue.Trim();
		}

		public static string Encrypt(string sourceValue)
		{
			return Encrypt(sourceValue, null);
		}

		public static string Encrypt(string sourceValue, string key)
		{
			string newValue = string.Empty;
			sourceValue = sourceValue.Trim();
			key = key.Trim();

			if (sourceValue.Length > 0)
			{
				if (string.IsNullOrEmpty(key))
					key = "SLSLGX";
				
				int length = key.Length;
				int num2 = 0;
				int maxValue = 0x100;
				int num4 = new Random().Next(maxValue);

				newValue = string.Format("{0:X2}", num4);
				int num5 = sourceValue.Length;

				for (int i = 0; i < num5; i++)
				{
					int num7 = (((byte) sourceValue[i]) + num4) % 0xff;
					num7 ^= (byte) key[num2];
					num2++;

					if (num2 == length)
					{
						num2 = 0;
					}

					newValue = newValue + string.Format("{0:X2}", num7);
					num4 = num7;
				}
			}
			return newValue.Trim();
		}
	}
}
