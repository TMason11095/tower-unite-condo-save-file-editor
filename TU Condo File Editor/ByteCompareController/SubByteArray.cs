using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TU_Condo_File_Editor.ByteCompareController
{
	[Obsolete("Just use IndexRange instead.")]
	public class SubByteArray
	{
		public byte[] OriginalData { get; private set; }
		public IndexRange SubIndexRange { get; private set; }
		public byte[] SubData { get { return OriginalData[SubIndexRange.IndexStart..(SubIndexRange.IndexEnd + 1)]; } }
		public int SubIndexOffset { get { return SubIndexRange.IndexStart; } }

		public SubByteArray(ref byte[] originalBytes, IndexRange subIndexRange)
		{
			OriginalData = originalBytes;
			SubIndexRange = subIndexRange;
		}
	}
}
