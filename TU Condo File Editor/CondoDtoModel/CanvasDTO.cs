using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TU_Condo_File_Editor.CondoDtoModel
{
	public class CanvasDTO
	{
		public byte[] HeaderBytes { get; set; }
		public PositionDTO Position { get; set; }
		public byte[] FooterBytes { get; set; }

		public byte[] ItemsBytes { get; set; }
	}
}
