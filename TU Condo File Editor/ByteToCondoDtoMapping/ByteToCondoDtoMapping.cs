using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TU_Condo_File_Editor.ByteController;

namespace TU_Condo_File_Editor.ByteToCondoDtoMapping
{
	public static class ByteToCondoDtoMapping
	{
		//File mapping
		public static Dictionary<CondoFileKeyword, string> FileKeywordHexPairs = new Dictionary<CondoFileKeyword, string>()
		{
			{ CondoFileKeyword.Header, "73-75-69-74-65-62-72-6F" },//8 bytes after (canvas count? next 1-4 bytes)
			{ CondoFileKeyword.CondoSettingsManagerStart, "00-00-00-17-00-00-00-43-6F-6E-64-6F-53-65-74-74-69-6E-67-73-4D-61-6E-61-67-65-72-5F-32" },
			{ CondoFileKeyword.CondoSettingsManagerEnd, "96-44" }
		};
		public static Dictionary<CondoFileKeyword, byte[]> FileKeywordBytePairs = FileKeywordHexPairs.Select(p => new { Key = p.Key, Value = p.Value.ToByte() }).ToDictionary(p => p.Key, p => p.Value);

		//Canvas mapping
		public static Dictionary<CanvasKeyword, string> CanvasKeywordHexPairs = new Dictionary<CanvasKeyword, string>()
		{
			{ CanvasKeyword.CanvasStart, "0F-00-00-00" },
			{ CanvasKeyword.Position, "80-3F" },
			{ CanvasKeyword.CanvasEnd, "00-00-00-40-00-00-00-40" }//Next byte of last canvas is some counter?
		};
		public static Dictionary<CanvasKeyword, byte[]> CanvasKeywordBytePairs = CanvasKeywordHexPairs.Select(p => new { Key = p.Key, Value = p.Value.ToByte() }).ToDictionary(p => p.Key, p => p.Value);
	}

	public enum CondoFileKeyword
	{
		Header,//8 bytes
		CondoSettingsManagerStart,
		CondoSettingsManagerEnd
	}

	public enum CanvasKeyword
	{
		CanvasStart,
		CanvasEnd,
		Position
	}
}
