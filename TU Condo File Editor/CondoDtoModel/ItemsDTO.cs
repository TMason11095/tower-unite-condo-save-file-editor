using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TU_Condo_File_Editor.CondoDtoModel
{
	public class ItemsDTO
	{
		public float ItemCount { get; set; }
		public byte[] BetweenItemCountAndItems { get; set; }
		public List<ItemsDTO> Items { get; set; }
		public byte[] AfterItemsBytes { get; set; }
	}
}
