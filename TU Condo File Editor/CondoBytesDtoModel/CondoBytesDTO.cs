using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TU_Condo_File_Editor.CondoBytesDtoModel
{
    public class CondoBytesDTO
    {
        public byte[] FullFileBytes { get; set; }

        public byte[] HeaderBytes { get; set; }
        public byte[] ItemsBytes { get; set; }
        public byte[] CondoSettingBytes { get; set; }
        public byte[] FooterBytes { get; set; }
    }
}
