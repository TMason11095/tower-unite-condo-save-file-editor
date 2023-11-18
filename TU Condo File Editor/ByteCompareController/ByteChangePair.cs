using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TU_Condo_File_Editor.ByteCompareController
{
	public class ByteChangePair
	{
		public IndexRange OriginalSubIndexRange { get; set; }
		public IndexRange ChangedSubIndexRange { get; set; }
		public ByteChange ByteChange { get; set; }

		public ByteChangePair(IndexRange originalSubIndexRange, IndexRange changedSubIndexRange, ByteChange byteChange)
		{
			OriginalSubIndexRange = originalSubIndexRange;
			ChangedSubIndexRange = changedSubIndexRange;
			ByteChange = byteChange;
		}

		public static ByteChangePair CreateMatchingPair(IndexRange originalSubIndexRange, IndexRange changedSubIndexRange)
		{
			return new ByteChangePair(originalSubIndexRange, changedSubIndexRange, ByteChange.Match);
		}

		public static ByteChangePair CreateMatchingPair(int sameStartIndex, int sameEndIndex)
		{
			//Create the index object
			IndexRange matchingIndexRange = new IndexRange(sameStartIndex, sameEndIndex);
			//Return the pair object
			return CreateMatchingPair(matchingIndexRange, matchingIndexRange);
		}

		public static ByteChangePair CreateInbetweenPair(ByteChangePair firstPair, ByteChangePair secondPair)
		{
			//Get inbetween indexes
			IndexRange originalIndexRange = IndexRange.CreateInbetweenIndexRange(firstPair.OriginalSubIndexRange, secondPair.OriginalSubIndexRange);
			IndexRange changedIndexRange = IndexRange.CreateInbetweenIndexRange(firstPair.ChangedSubIndexRange, secondPair.ChangedSubIndexRange);

			//Decide byte change type
			ByteChange byteChange;
			if (originalIndexRange == null && changedIndexRange == null)//Return null if both ranges are null
			{
				return null;
			}
			else if (originalIndexRange == null)//Change isn't null, but original is (Added)
			{
				byteChange = ByteChange.Added;
			}
			else if (changedIndexRange == null)//Original isn't null, but changed isn't (Deleted)
			{
				byteChange = ByteChange.Deleted;
			}
			else//Both are not null (Changed)
			{
				byteChange = ByteChange.Changed;
			}

			//Return the inbetween pair
			return new ByteChangePair(originalIndexRange, changedIndexRange, byteChange);
		}
	}
}
