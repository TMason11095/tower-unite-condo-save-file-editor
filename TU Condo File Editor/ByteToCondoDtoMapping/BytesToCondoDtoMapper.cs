using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TU_Condo_File_Editor.ByteController;
using TU_Condo_File_Editor.CondoBytesDtoModel;
using TU_Condo_File_Editor.CondoDtoModel;

namespace TU_Condo_File_Editor.ByteToCondoDtoMapping
{
    public static class BytesToCondoDtoMapper
	{
		public static CondoDTO MapBytesToCondoDTO(byte[] bytes)
		{
			//Get condo bytes
			CondoBytesDTO condoBytesDTO = MapBytesToCondoBytesDTO(bytes);
			//Convert to condo
			CondoDTO condoDTO = CondoBytesDtoToCondoDTO(condoBytesDTO);

			//Return
			return condoDTO;
		}

		private static CondoBytesDTO MapBytesToCondoBytesDTO(byte[] bytes)
		{
			//Create condo byte array object
			CondoBytesDTO condoBytesDTO = new CondoBytesDTO();

			//Save full byte array
			condoBytesDTO.FullFileBytes = bytes;

			//Get header (endIndex = startIndex + keywordLength + headerLength + 1(..] is non inclusive))
			byte[] headerKeyword = ByteToCondoDtoMapping.FileKeywordBytePairs[CondoFileKeyword.Header];
			int headerIndex = bytes.FindIndex(headerKeyword);
			int keywordLength = headerKeyword.Length;
			int headerLength = 8;
			int nonInclusiveEndIndex = headerIndex + keywordLength + headerLength;
			condoBytesDTO.HeaderBytes = bytes[0..nonInclusiveEndIndex];
			int startIndex = nonInclusiveEndIndex;

			//Get condo settings
			int condoSettingStartIndex = bytes.FindIndex(ByteToCondoDtoMapping.FileKeywordBytePairs[CondoFileKeyword.CondoSettingsManagerStart], startIndex);
			byte[] condoSettingEndKeyword = ByteToCondoDtoMapping.FileKeywordBytePairs[CondoFileKeyword.CondoSettingsManagerEnd];
			int condoSettingEndIndex = bytes.FindIndex(condoSettingEndKeyword, condoSettingStartIndex);
			int condoSettingEndLength = condoSettingEndKeyword.Length;
			nonInclusiveEndIndex = condoSettingEndIndex + condoSettingEndLength;
			condoBytesDTO.CondoSettingBytes = bytes[condoSettingStartIndex..nonInclusiveEndIndex];

			//Get items (between header and condo settings)
			condoBytesDTO.ItemsBytes = bytes[startIndex..condoSettingStartIndex];//End index is non inclusive so we can use the next start index as is

			//Get footer (remaining items at the end)
			condoBytesDTO.FooterBytes = bytes[nonInclusiveEndIndex..^0];

			return condoBytesDTO;
		}

		public static CondoDTO CondoBytesDtoToCondoDTO(CondoBytesDTO condoBytesDTO)
		{
			CondoDTO condoDTO = new CondoDTO();

			//Header
			condoDTO.HeaderBytes = condoBytesDTO.HeaderBytes;
			//Items
			condoDTO.ItemsBytesDTO = SplitItemsBytes(condoBytesDTO.ItemsBytes);
			//Condo Settings
			condoDTO.CondoSettingBytes = condoBytesDTO.CondoSettingBytes;
			//Footer
			condoDTO.FooterBytes = condoBytesDTO.FooterBytes;

			return condoDTO;
		}

		private static ItemsBytesDTO SplitItemsBytes(byte[] itemsBytes)
		{
			ItemsBytesDTO itemsBytesDTO = new ItemsBytesDTO();

			//Save full array
			itemsBytesDTO.FullItemBytes = itemsBytes;

			//first byte goes to item count
			itemsBytesDTO.ItemCountBytes = itemsBytes[0..1];

			//Get index of first item
			int firstItemStartIndex = itemsBytes.FindIndex(ByteToCondoDtoMapping.CanvasKeywordBytePairs[CanvasKeyword.CanvasStart], 1);

			//Finish with between bytes if there's no items found
			if (firstItemStartIndex < 0)
			{
				itemsBytesDTO.BetweenItemCountAndItemsBytes = itemsBytes[1..itemsBytes.Length];
				return itemsBytesDTO;
			}

			//Get inbetween bytes
			itemsBytesDTO.BetweenItemCountAndItemsBytes = itemsBytes[1..firstItemStartIndex];

			//Get end index of the last item
			byte[] canvasEndKeywordBytes = ByteToCondoDtoMapping.CanvasKeywordBytePairs[CanvasKeyword.CanvasEnd];
			int canvasEndKeywordLength = canvasEndKeywordBytes.Length;
			int currentEndIndex = itemsBytes.FindIndex(canvasEndKeywordBytes, firstItemStartIndex);
			int prevEndIndex = currentEndIndex;
			while (currentEndIndex >= 0)
			{
				//Set prev end index
				prevEndIndex = currentEndIndex;
				//Get next item's end index
				currentEndIndex = itemsBytes.FindIndex(canvasEndKeywordBytes, prevEndIndex + 1);
			}
			//End index is the last found index (prev) + keyword length
			int nonInclusiveEndIndex = prevEndIndex + canvasEndKeywordLength;

			//Get Items
			itemsBytesDTO.ItemsBytes = itemsBytes[firstItemStartIndex..nonInclusiveEndIndex];

			//Get remaining bytes
			itemsBytesDTO.AfterItemsBytes = itemsBytes[nonInclusiveEndIndex..^0];

			//Return split items
			return itemsBytesDTO;
		}
	}
}
