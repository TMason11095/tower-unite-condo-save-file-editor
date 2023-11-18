using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TU_Condo_File_Editor.ByteController
{
    public class ByteFileController
    {
        public string FilePath { get; private set; } = "";
        public byte[] FileByteData { get; private set; } = new byte[0];

        public ByteFileController(string filePath, FileAccessType fileAccess = FileAccessType.None)
        {
            SetFilePath(filePath);

            //Read if access is read
            if (fileAccess == FileAccessType.Read)
            {
                ReadFile();
			}
        }

        public void SetFilePath(string filePath)
        {
            FilePath = filePath;
        }

        public void ReadFile(string filePath)
        {
            SetFilePath(filePath);
            ReadFile();
		}
        public void ReadFile()
        {
			FileByteData = File.ReadAllBytes(FilePath);
		}

        /// <summary>Save current bytes to given file.</summary>
        /// <param name="filePath">Output file path</param>
        public void SaveToFile(string filePath)
        {
            //Just set the file path and call the parameterless method
            SetFilePath(filePath);
            SaveToFile();
        }
        /// <summary>Overwrite original file with the current bytes.</summary>
        public void SaveToFile()
        {
            //Write to file
            File.WriteAllBytes(FilePath, FileByteData);
        }
    }

    public enum FileAccessType
    {
        None,
        Read,
        Write
    }
}
