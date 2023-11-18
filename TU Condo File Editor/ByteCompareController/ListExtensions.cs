using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TU_Condo_File_Editor.ByteCompareController
{
	public static class ListExtensions
	{
		public static T Pop<T>(this List<T> indexes)
		{
			//Return null if list is empty
			if (indexes.Count == 0)
			{
				return default;
			}

			//Save the first object to return
			T result = indexes[0];

			//Remove the first object from list
			indexes.RemoveAt(0);

			//Return the object
			return result;
		}
	}
}
