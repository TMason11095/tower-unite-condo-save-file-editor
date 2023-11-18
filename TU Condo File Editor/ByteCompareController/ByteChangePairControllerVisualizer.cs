using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TU_Condo_File_Editor.ByteController;

namespace TU_Condo_File_Editor.ByteCompareController
{
	public class ByteChangePairControllerVisualizer
	{
		public ByteChangePairController ByteChangePairController { get; set; }
		public byte[] OriginalData { get { return ByteChangePairController.OriginalData; } }
		public string OriginalDataChar { get { return OriginalData.ToByteString(ByteStringFormat.Char); } }
		public string OriginalDataHex { get { return OriginalData.ToByteString(ByteStringFormat.Hex); } }
		public byte[] ChangedData { get { return ByteChangePairController.ChangedData; } }
		public string ChangedDataChar { get { return ChangedData.ToByteString(ByteStringFormat.Char); } }
		public string ChangedDataHex { get { return ChangedData.ToByteString(ByteStringFormat.Hex); } }
		public List<
			(
				ByteChange ByteChange,
				(IndexRange OriginalSubIndexRange, IndexRange ChangedSubIndexRange) ByteArrayPair,
				(string OriginalChar, string ChangedChar) CharStringPair,
				(string OriginalHex, string ChangedHex) HexStringPair
			)> ChangedPairs
		{
			get
			{
				var formattedPairs = ByteChangePairController.ChangedPairs
					.Select(p => new
					{
						ByteChange = p.ByteChange,
						ByteArrayPair = new
						{
							OriginalSubIndexRange = p.OriginalSubIndexRange,
							ChangedSubIndexRange = p.ChangedSubIndexRange
						},
						CharStringPair = new
						{
							OriginalChar = p.OriginalSubIndexRange?.GetSubArray(OriginalData).ToByteString(ByteStringFormat.Char) ?? "",
							ChangedChar = p.ChangedSubIndexRange?.GetSubArray(ChangedData).ToByteString(ByteStringFormat.Char) ?? ""
						},
						HexStringPair = new
						{
							OriginalHex = p.OriginalSubIndexRange?.GetSubArray(OriginalData).ToByteString(ByteStringFormat.Hex) ?? "",
							ChangedHex = p.ChangedSubIndexRange?.GetSubArray(ChangedData).ToByteString(ByteStringFormat.Hex) ?? ""
						}
					});

				return formattedPairs.Select(p =>
					(
						ByteChange: p.ByteChange,
						ByteArrayPair: (OriginalSubIndexRange: p.ByteArrayPair.OriginalSubIndexRange, ChangedSubIndexRange: p.ByteArrayPair.ChangedSubIndexRange),
						CharStringPair: (OriginalChar: p.CharStringPair.OriginalChar, ChangedChar: p.CharStringPair.ChangedChar),
						HexStringPair: (OriginalHex: p.HexStringPair.OriginalHex, ChangedHex: p.HexStringPair.ChangedHex)
					)).ToList();
			}
		}


		public ByteChangePairControllerVisualizer(ByteChangePairController byteChangePairController)
		{
			ByteChangePairController = byteChangePairController;
		}
	}
}
