using System;
using TU_Condo_File_Editor.ByteCompareController;
using TU_Condo_File_Editor.ByteController;
using TU_Condo_File_Editor.ByteToCondoDtoMapping;
using TU_Condo_File_Editor.CondoDtoModel;

namespace TU_Condo_File_Editor
{
    public class Program
	{
		public static readonly string CONDO_BASE_FOLDER = @"C:\Users\Tyler\Desktop\TU Condo Files\";
		public static readonly string CONDO_BACKUP_FOLDER = CONDO_BASE_FOLDER + @"Backup Files\";
		public static readonly string CONDO_FILE_NAME = @"\CondoData";

		public static readonly string BLANK_CONDO = CONDO_BACKUP_FOLDER + @"Blank Condo" + CONDO_FILE_NAME;
		public static readonly string BLANK_CANVAS_DEFAULT_POS = CONDO_BACKUP_FOLDER + @"Blank Canvas X0 Y0 Z100" + CONDO_FILE_NAME;
		public static readonly string BLANK_CANVAS_POS_X_MAX = CONDO_BACKUP_FOLDER + @"Blank Canvas XMax Y0 Z100" + CONDO_FILE_NAME;
		public static readonly string BLANK_CANVAS_AFTER_DELETING_1ST_ONE = CONDO_BACKUP_FOLDER + @"1 Blank Canvas after Deleting 1st canvas" + CONDO_FILE_NAME;
		public static readonly string TWO_BLANK_CANVASES = CONDO_BACKUP_FOLDER + @"2 Blank Canvas X0 Y0 Z100 - X50 Y50 Z250" + CONDO_FILE_NAME;

		public static readonly string BLANK_CANVAS_NAME_TEST = CONDO_BACKUP_FOLDER + @"Blank Canvas NameTest" + CONDO_FILE_NAME;

		public static readonly string CANVAS_OPTION_DEFAULT = CONDO_BACKUP_FOLDER + @"Canvas Options Default" + CONDO_FILE_NAME;
		public static readonly string CANVAS_OPTION_NAME = CONDO_BACKUP_FOLDER + @"Canvas Options Name Test" + CONDO_FILE_NAME;

		static void Main(string[] args)
		{
			//Create pair controller
			ByteChangePairController test = new ByteChangePairController(CANVAS_OPTION_DEFAULT, CANVAS_OPTION_NAME);
			test.FindAllPairs();
			ByteChangePairControllerVisualizer visualizer = new ByteChangePairControllerVisualizer(test);



			//ByteFileController inputFile = new ByteFileController(BLANK_CANVAS_AFTER_DELETING_1ST_ONE, FileAccessType.Read);
			//CondoDTO mapTest = BytesToCondoDtoMapper.MapBytesToCondoDTO(inputFile.FileByteData);



			//Update condo's 1 canvas to have pos 500,500,500
			//Get bytes from file
			//ByteFileController condoFile = new ByteFileController(BLANK_CANVAS_DEFAULT_POS, FileAccessType.Read);

			//Convert bytes to a DTO object for editing
			//BytesToCondoDtoMapper.MapBytesToCondoDTO(condoFile.FileByteData);

			//var n = visualizer.ChangedPairs;

			Console.ReadLine();
		}
	}
}