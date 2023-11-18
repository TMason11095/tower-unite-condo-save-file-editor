using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TU_Condo_File_Editor.CondoBytesDtoModel
{
    public class ItemsBytesDTO
    {
        public byte[] FullItemBytes { get; set; }

        public byte[] ItemCountBytes { get; set; }
        public byte[] BetweenItemCountAndItemsBytes { get; set; }
        public byte[] ItemsBytes { get; set; }//Replace
        public byte[] AfterItemsBytes { get; set; }
    }
}
