using System;
using System.Collections.Generic;
using System.IO;

namespace comp110_worksheet_7
{
	public static class DirectoryUtils
	{
        public static int GoDeeper(string[] CurrentDirectory, int CurrentDepth, int MostDepth)
        {
            if (CurrentDirectory.Length != 0)
            {
                for (int i = 0; i < CurrentDirectory.Length; i++)
                {
                    CurrentDepth += 1;
                    if (CurrentDepth > MostDepth)
                    {
                        MostDepth = CurrentDepth;
                    }
                    string[] NewDirectory = Directory.GetDirectories(CurrentDirectory[i], "*.*", SearchOption.TopDirectoryOnly);
                    GoDeeper(NewDirectory, CurrentDepth, MostDepth);
                }
            }
            return MostDepth;
        }

		// Return the size, in bytes, of the given file
		public static long GetFileSize(string filePath)
		{
			return new FileInfo(filePath).Length;
		}

		// Return true if the given path points to a directory, false if it points to a file
		public static bool IsDirectory(string path)
		{
			return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
		}

		// Return the total size, in bytes, of all the files below the given directory
		public static long GetTotalSize(string directory)
		{
            string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            long TotalSize = 0;
            for (int i = 0; i < files.Length; i++)
            {
                TotalSize += GetFileSize(files[i]);
            }
            return TotalSize;
		}

		// Return the number of files (not counting directories) below the given directory
		public static int CountFiles(string directory)
		{
            string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            int JustFiles = 0;
            for (int i = 0; i < files.Length; i++)
            {
                if (IsDirectory(files[i]) == false)
                {
                    JustFiles += 1;
                }
            }
            return JustFiles;
        }

		// Return the nesting depth of the given directory. A directory containing only files (no subdirectories) has a depth of 0.
		public static int GetDepth(string directory)
		{
            int MostDepth = 0;
            int CurrentDepth = 0;
            string[] FirstDirectory = Directory.GetDirectories(directory, "*.*", SearchOption.TopDirectoryOnly);
            MostDepth = GoDeeper(FirstDirectory, CurrentDepth, MostDepth);
            return MostDepth;
        }

		// Get the path and size (in bytes) of the smallest file below the given directory
		public static Tuple<string, long> GetSmallestFile(string directory)
		{
            string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            long CurrentSmallest = -1;
            string SmallestFile = "";
            for (int i = 0; i < files.Length; i++)
            {
                long Size = GetFileSize(files[i]);
                if (CurrentSmallest == -1)
                {
                    CurrentSmallest = Size;
                }
                if (Size < CurrentSmallest)
                {
                    CurrentSmallest = Size;
                    SmallestFile = files[i];
                }
            }
            Tuple<string, long> Smallest = new Tuple<string, long>(SmallestFile, CurrentSmallest);
            return Smallest;
		}

		// Get the path and size (in bytes) of the largest file below the given directory
		public static Tuple<string, long> GetLargestFile(string directory)
        {
            string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            long CurrentLargest = 0;
            string LargestFile = "";
            for (int i = 0; i < files.Length; i++)
            {
                long Size = GetFileSize(files[i]);
                if (Size > CurrentLargest)
                {
                    CurrentLargest = Size;
                    LargestFile = files[i];
                }
            }
            Tuple<string, long> Largest = new Tuple<string, long>(LargestFile, CurrentLargest);
            return Largest;
        }

        // Get all files whose size is equal to the given value (in bytes) below the given directory
        public static List<string> GetFilesOfSize(string directory, long size)
		{
			string[] allFiles = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            List<string> filesOfSize = new List<string>();
            for (int i = 0; i < allFiles.Length; i++)
            {
                if (size == GetFileSize(allFiles[i]))
                {
                    filesOfSize.Add(allFiles[i]);
                }
            }
            return filesOfSize;
        }
	}
}
