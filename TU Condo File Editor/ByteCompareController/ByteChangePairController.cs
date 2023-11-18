using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TU_Condo_File_Editor.ByteController;

namespace TU_Condo_File_Editor.ByteCompareController
{
	public class ByteChangePairController
	{
		public byte[] OriginalData { get; set; }
		public byte[] ChangedData { get; set; }
		public List<ByteChangePair> ChangedPairs { get; set; } = new List<ByteChangePair>();

		public ByteChangePairController(byte[] originalData, byte[] changedData)
		{
			OriginalData = originalData;
			ChangedData = changedData;
		}

		public ByteChangePairController(string originalFilePath, string changedFilePath)
			: this(File.ReadAllBytes(originalFilePath), File.ReadAllBytes(changedFilePath))
		{

		}

		public List<ByteChangePair> FindAllPairs()
		{
			//Create return list
			List<ByteChangePair> pairs = new List<ByteChangePair>();

			//Get all matching pairs
			List<ByteChangePair> matchingPairs = GetMatchingPairs();
			pairs.AddRange(matchingPairs);
			//Get remaining non matching pairs
			List<ByteChangePair> nonMatchingPairs = GetNonMatchingPairs(matchingPairs);
			pairs.AddRange(nonMatchingPairs);

			//Set the new pair list
			ChangedPairs = pairs;

			//Reorganize pairs
			ReorganizePairs();

			//Return pairs
			return pairs;
		}

		/// <summary>Reorder ChangedPairs to be in index order.</summary>
		private void ReorganizePairs()
		{
			//Add counts fields to each pairing
			int row = 0;
			var originalOrderPairs = from pair in ChangedPairs
									 orderby pair.OriginalSubIndexRange?.IndexStart ?? pair.ChangedSubIndexRange.IndexStart
									 select new
									 {
									 	pair = pair,
										originalRow = ++row
									 };
			row = 0;
			var changedOrderPairs = from pair in originalOrderPairs
									orderby pair.pair.ChangedSubIndexRange?.IndexStart ?? pair.pair.OriginalSubIndexRange.IndexStart
									select new
									{
										pair = pair,
										originalRow = pair.originalRow,
										changedRow = ++row
									};
			var orderedPairs = from pair in changedOrderPairs
							   orderby pair.originalRow + pair.changedRow
							   select pair.pair.pair;

			//Set the new pair list
			ChangedPairs = orderedPairs.ToList();
		}

		public List<ByteChangePair> GetNonMatchingPairs(List<ByteChangePair> matchingPairs)
		{
			//Create return list
			List<ByteChangePair> nonMatchingPairs = new List<ByteChangePair>();

			//No matching so just return the rest as changed
			if (matchingPairs.Count == 0)
			{
				nonMatchingPairs.Add(new ByteChangePair(new IndexRange(0, OriginalData.Length - 1), new IndexRange(0, ChangedData.Length - 1), ByteChange.Changed));
				return nonMatchingPairs;
			}

			//Order pairs by indexes
			matchingPairs = matchingPairs.OrderBy(p => p.OriginalSubIndexRange.IndexStart).ToList();

			//TODO: Handle if we don't have matching prefix/suffix pairs (check for index start > 0/index end < length -1)
			//Pop the first pair before starting the loop
			ByteChangePair prevPair = matchingPairs.Pop();
			//Loop the remaining pairs and get their inbetween pairs
			for (ByteChangePair currentPair = matchingPairs.Pop(); currentPair != null; currentPair = matchingPairs.Pop())
			{
				nonMatchingPairs.Add(ByteChangePair.CreateInbetweenPair(prevPair, currentPair));
				//Set prevPair for next loop
				prevPair = currentPair;
			}

			//Return inbetween pairs
			return nonMatchingPairs;
		}

		public List<ByteChangePair> GetMatchingPairs()
		{
			//Create the return list
			List<ByteChangePair> pairs = new List<ByteChangePair>();
			//Create index ranges to remove already found matches
			IndexRange remainingOriginalSubArray = new IndexRange(0, OriginalData.Length - 1);
			IndexRange remainingChangedSubArray = new IndexRange(0, ChangedData.Length - 1);

			//Get matching prefix pair
			ByteChangePair matchingPrefixPair = GetMatchingPrefixPair(OriginalData, ChangedData);
			//Update list and sub array if not null
			if (matchingPrefixPair != null)
			{
				//Add to list
				pairs.Add(matchingPrefixPair);
				//Update sub arrays
				remainingOriginalSubArray.IndexStart = matchingPrefixPair.OriginalSubIndexRange.IndexEnd + 1;
				remainingChangedSubArray.IndexStart = matchingPrefixPair.ChangedSubIndexRange.IndexEnd + 1;
			}

			//Get matching suffix pair
			ByteChangePair matchingSuffixPair = GetMatchingSuffixPair(OriginalData, ChangedData);
			//Update list and sub array if not null
			if (matchingSuffixPair != null)
			{
				//Add to list
				pairs.Add(matchingSuffixPair);
				//Update sub arrays
				remainingOriginalSubArray.IndexEnd = matchingSuffixPair.OriginalSubIndexRange.IndexStart - 1;
				remainingChangedSubArray.IndexEnd = matchingSuffixPair.ChangedSubIndexRange.IndexStart - 1;
			}

			//Get remaining pairs using middle snake
			pairs.AddRange(GetLcsPairs(remainingOriginalSubArray.GetSubArray(OriginalData), remainingChangedSubArray.GetSubArray(ChangedData), remainingOriginalSubArray.IndexStart, remainingChangedSubArray.IndexStart));

			//Return matching pairs
			return pairs;
		}

		public List<ByteChangePair> GetLcsPairs(byte[] originalSubData, byte[] changedSubData, int originalIndexOffset, int changedIndexOffset)
		{
			//Create the return list
			List<ByteChangePair> pairs = new List<ByteChangePair>();

			//Get LCS pair
			(IndexRange originalIndexRange, IndexRange changedIndexRange) lcsPair = IndexRange.GetLcsIndexes(originalSubData, changedSubData);

			//Return the empty list if index ranges are null
			if (lcsPair.originalIndexRange == null || lcsPair.changedIndexRange == null)
			{
				return pairs;
			}

			//Offset the ranges
			lcsPair.originalIndexRange.AddOffset(originalIndexOffset);
			lcsPair.changedIndexRange.AddOffset(changedIndexOffset);

			//Add the matching pair to the list
			pairs.Add(ByteChangePair.CreateMatchingPair(lcsPair.originalIndexRange, lcsPair.changedIndexRange));

			//Get left an right side of the LCS pair
			IndexRange originalLeftNonLcs = new IndexRange(indexStart: originalIndexOffset, indexEnd: lcsPair.originalIndexRange.IndexStart - 1);
			IndexRange originalRightNonLcs = new IndexRange(indexStart: lcsPair.originalIndexRange.IndexEnd + 1, indexEnd: originalSubData.Length - 1 + originalIndexOffset);
			IndexRange changedLeftNonLcs = new IndexRange(indexStart: changedIndexOffset, indexEnd: lcsPair.changedIndexRange.IndexStart - 1);
			IndexRange changedRightNonLcs = new IndexRange(indexStart: lcsPair.changedIndexRange.IndexEnd + 1, indexEnd: changedSubData.Length - 1 + changedIndexOffset);

			//Get sub arrays
			byte[] originalLeftNonLcsSubData = originalLeftNonLcs.GetSubArray(OriginalData);
			byte[] originalRightNonLcsSubData = originalRightNonLcs.GetSubArray(OriginalData);
			byte[] changedLeftNonLcsSubData = changedLeftNonLcs.GetSubArray(ChangedData);
			byte[] changedRightNonLcsSubData = changedRightNonLcs.GetSubArray(ChangedData);

			//Recursive call the left and right sides
			List<ByteChangePair> leftNonLcsPair = GetLcsPairs(originalLeftNonLcsSubData, changedLeftNonLcsSubData, originalLeftNonLcs.IndexStart, changedLeftNonLcs.IndexStart);
			pairs.AddRange(leftNonLcsPair);
			List<ByteChangePair> rightNonLcsPair = GetLcsPairs(originalRightNonLcsSubData, changedRightNonLcsSubData, originalRightNonLcs.IndexStart, changedRightNonLcs.IndexStart);
			pairs.AddRange(rightNonLcsPair);

			//Return pairs
			return pairs;
		}

		private static ByteChangePair GetMatchingPrefixPair(byte[] originalFullData, byte[] changedFullData)
		{
			//Get the length of the matching prefix
			int matchingPrefixLength = GetMatchingPrefixLength(originalFullData, changedFullData);

			//Return null if there's no match
			if (matchingPrefixLength <= 0)
			{
				return null;
			}

			//Return pair
			return ByteChangePair.CreateMatchingPair(0, matchingPrefixLength - 1);
		}

		/// <returns>Number of matching bytes starting from [0].</returns>
		private static int GetMatchingPrefixLength(byte[] originalFullData, byte[] changedFullData)
		{
			//Get the min length of both arrays
			int minLength = Math.Min(originalFullData.Length, changedFullData.Length);

			//Loop through both arrays and get the length of matching bytes
			for (int i = 0; i < minLength; i++)
			{
				//Return value if they don't match
				if (originalFullData[i] != changedFullData[i])
				{
					return i;
				}
			}

			//Return min if all matched
			return minLength;
		}

		private static ByteChangePair GetMatchingSuffixPair(byte[] originalFullData, byte[] changedFullData)
		{
			//Get the length of the matching suffix
			int matchingSuffixLength = GetMatchingSuffixLength(originalFullData, changedFullData);

			//Return null if there's no match
			if (matchingSuffixLength <= 0)
			{
				return null;
			}

			//Get array lengths
			int originalLength = originalFullData.Length;
			int changedLength = changedFullData.Length;

			//Create index objects
			IndexRange originalIndexes = new IndexRange(indexStart: originalLength - matchingSuffixLength, indexEnd: originalLength - 1);
			IndexRange changedIndexes = new IndexRange(indexStart: changedLength - matchingSuffixLength, indexEnd: changedLength - 1);

			//Return pair
			return ByteChangePair.CreateMatchingPair(originalIndexes, changedIndexes);
		}

		/// <returns>Number of matching bytes starting from [^1].</returns>
		private static int GetMatchingSuffixLength(byte[] originalFullData, byte[] changedFullData)
		{
			//Get the min length of both arrays
			int minLength = Math.Min(originalFullData.Length, changedFullData.Length);

			//Loop through both arrays backwards and get the length of matching bytes
			for (int i = 1; i <= minLength; i++)//[^] starts at 1
			{
				//Return value if they don't match
				if (originalFullData[^i] != changedFullData[^i])
				{
					return i - 1;
				}
			}

			//Return min if all matched
			return minLength;
		}
	}
}
