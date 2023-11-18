using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TU_Condo_File_Editor.ByteCompareController
{
	public class IndexRange
	{
		public int IndexStart { get; set; }
		public int IndexEnd { get; set; }
		public int Length { get => IndexEnd - IndexStart + 1; }

		public IndexRange(int indexStart, int indexEnd)
		{
			IndexStart = indexStart;
			IndexEnd = indexEnd;
		}

		public byte[] GetSubArray(byte[] originalData)
		{
			return originalData[IndexStart..(IndexEnd + 1)];
		}

		public void AddOffset(int offset)
		{
			IndexStart += offset;
			IndexEnd += offset;
		}

		public static IndexRange CreateInbetweenIndexRange(IndexRange firstRange, IndexRange secondRange)
		{
			//Get indexes
			int indexStart = firstRange.IndexEnd + 1;
			int indexEnd = secondRange.IndexStart - 1;

			//Return null if indexes overlap (nothing inbetween given ranges)
			if (indexStart > indexEnd)
			{
				return null;
			}

			//Return inbetween range
			return new IndexRange(indexStart, indexEnd);
		}

		//Longest Common Subarray
		public static (IndexRange originalIndexes, IndexRange changedIndexes) GetLcsIndexes(byte[] originalBytes, byte[] changedBytes, int minMatchLength = 8)
		{
			//Get lengths of arrays
			int originalLength = originalBytes.Length;
			int changedLength = changedBytes.Length;

			//Create table to store common sub array lengths
			int[,] commonSubarray = new int[originalLength + 1, changedLength + 1];

			//Setup result length indexes
			int maxLengthI = 0;
			int maxLengthJ = 0;
			//int resultLength = 0;

			//Build table
			for (int i = 0; i <= originalLength; i++)
			{
				for (int j = 0; j < changedLength; j++)
				{
					//First indexes are set to 0
					if (i == 0 || j == 0)
					{
						commonSubarray[i, j] = 0;
					}
					else if (originalBytes[i - 1] == changedBytes[j - 1])//Current byte match (-1 because the table is offset by 1 at the start)
					{
						//Set current index as prev + 1
						commonSubarray[i, j] = commonSubarray[i - 1, j - 1] + 1;

						//Check for new max length
						if (commonSubarray[i, j] > commonSubarray[maxLengthI, maxLengthJ])
						{
							//Set new max length indexes
							maxLengthI = i;
							maxLengthJ = j;
						}
					}
					else//Not a match
					{
						commonSubarray[i, j] = 0;
					}
				}
			}


			//Longest common subarray length
			int longestLength = commonSubarray[maxLengthI, maxLengthJ];

			//Return null if the longest length is less than the min needed
			if (longestLength < minMatchLength)
			{
				return (null, null);
			}

			//Get result values from the table
			//Original start index (-1 to offset table index, +1 to offset longest length to get to the first match (ie 1))
			int originalStartIndex = maxLengthI - longestLength;
			//Original end index
			int originalEndIndex = originalStartIndex + longestLength - 1;
			//Changed start index
			int changedStartIndex = maxLengthJ - longestLength;
			//Change end index
			int changedEndIndex = changedStartIndex + longestLength - 1;

			//Create index range objects
			IndexRange originalLcsIndexes = new IndexRange(originalStartIndex, originalEndIndex);
			IndexRange changedLcsIndexes = new IndexRange(changedStartIndex, changedEndIndex);

			//Return both ranges as a tuple
			return (originalLcsIndexes, changedLcsIndexes);
		}
	}
}
