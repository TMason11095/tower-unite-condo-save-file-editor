using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TU_Condo_File_Editor.ByteToCondoDtoMapping;

namespace TU_Condo_File_Editor.ByteController
{
	public enum ByteStringFormat
	{
		Hex,
		Char
	}

	public static class ByteExtensions
	{
		public static List<int> FindIndexes(this byte[] bytes, CondoFileKeyword condoKeyword)
		{
			//Create return list
			List<int> returnIndexes = new List<int>();

			//Get byte value from keyword
			byte[] hexKeyword = ByteToCondoDtoMapping.ByteToCondoDtoMapping.FileKeywordBytePairs[condoKeyword];

			//Find indexes
			for (int index = bytes.FindIndex(hexKeyword); index >= 0; index = bytes.FindIndex(hexKeyword, index + 1))
			{
				returnIndexes.Add(index);
			}

			//Return list
			return returnIndexes;
		}

		public static byte[] ToByte(this string hexString)
		{
			//Split hex string
			string[] hexes = hexString.Split('-');
			//Create byte array of same length
			byte[] result = new byte[hexes.Length];
			//Convert each hex into the new array
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = Convert.ToByte(hexes[i], 16);
			}
			//Return converted array
			return result;
		}

		public static List<byte[]> ToByte(this List<string> hexStrings)
		{
			return hexStrings.Select(hs => hs.ToByte()).ToList();
		}

		public static string ToByteString(this byte[] bytes, ByteStringFormat format = ByteStringFormat.Char)
		{
			return format switch
			{
				ByteStringFormat.Char => new string(bytes.Select(b => (char)b).ToArray()),
				ByteStringFormat.Hex => BitConverter.ToString(bytes)
			};
		}

		public static List<string> ToByteString(this List<byte[]> bytes, ByteStringFormat format = ByteStringFormat.Char)
		{
			return bytes.Select(bs => bs.ToByteString(format)).ToList();
		}

		public static int FindIndex(this byte[] bytes, byte[] pattern, int startIndex = 0)
		{
			//Get length of both arrays
			int bytesLength = bytes.Length;
			int patternLength = pattern.Length;

			//Fail if pattern length is longer than bytes, if either array is empty, or starting index is pass the end of array
			if (patternLength > bytesLength || bytesLength == 0 || patternLength == 0 || startIndex >= bytesLength)
			{
				return -1;
			}

			//Loop through bytes and look for a match against pattern
			for (int byteIndex = startIndex; byteIndex < bytesLength; byteIndex++)
			{
				//Fail if we go pass pattern's length from the end as it's no longer possible to find the full array
				if (byteIndex > bytesLength - patternLength)
				{
					break;
				}

				//Loop through pattern
				int numOfMatchingIndexes = 0;
				for (int patternIndex = 0; patternIndex < patternLength; patternIndex++)
				{
					//Increase match count if they match
					if (bytes[byteIndex + patternIndex] == pattern[patternIndex])
					{
						numOfMatchingIndexes++;
					}
				}
				
				//Return index if the number of matching indexes equals the pattern length
				if (numOfMatchingIndexes == patternLength)
				{
					return byteIndex;
				}
			}

			//Couldn't find index
			return -1;
		}
	}
}
