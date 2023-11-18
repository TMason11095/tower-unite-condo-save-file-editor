using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TU_Condo_File_Editor.CondoBytesDtoModel;

namespace TU_Condo_File_Editor.CondoDtoModel
{
    public class CondoDTO
	{
		public byte[] HeaderBytes { get; set; }
		public ItemsBytesDTO ItemsBytesDTO { get; set; }
		public byte[] CondoSettingBytes { get; set; }//Replace
		public byte[] FooterBytes { get; set; }
	}
}
