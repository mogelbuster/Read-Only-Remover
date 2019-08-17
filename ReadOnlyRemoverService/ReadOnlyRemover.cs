using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadOnlyRemoverService
{
	public class ReadOnlyRemover : FileSystemWatcher
	{

		public ReadOnlyRemover()
		{
			Init(true);
		}

		public ReadOnlyRemover(string inDirectoryPath)
			: base(inDirectoryPath)
		{
			Init(true);
		}

		public ReadOnlyRemover(string inDirectoryPath, string inFilter, bool includeSubdirectories)
			: base(inDirectoryPath, inFilter)
		{
			Init(includeSubdirectories);
		}

		private void Init(bool includeSubdirectories)
		{
			IncludeSubdirectories = includeSubdirectories;
			// Eliminate duplicates when timestamp doesn't change
			NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.Attributes | NotifyFilters.LastWrite; // The default also has NotifyFilters.LastWrite
			EnableRaisingEvents = true;
			Created += Watcher_Created;
			Changed += Watcher_Changed;
			Deleted += Watcher_Deleted;
			Renamed += Watcher_Renamed;
			Error += Watcher_Error;

			// Initially remove the readonly
			RemoveReadOnlyFromDirectory();
		}

		public void Watcher_Created(object source, FileSystemEventArgs inArgs)
		{
			//Log.WriteLine("File created or added: " + inArgs.FullPath);
			FileChanged(inArgs.FullPath);
		}

		public void Watcher_Changed(object sender, FileSystemEventArgs inArgs)
		{
			//Log.WriteLine("File changed: " + inArgs.FullPath);
			FileChanged(inArgs.FullPath);
		}

		public void Watcher_Deleted(object sender, FileSystemEventArgs inArgs)
		{
			//Log.WriteLine("File deleted: " + inArgs.FullPath);
			FileChanged(inArgs.FullPath);
		}

		public void Watcher_Renamed(object sender, RenamedEventArgs inArgs)
		{
			//Log.WriteLine("File renamed: " + inArgs.OldFullPath + ", New name: " + inArgs.FullPath);
			FileChanged(inArgs.FullPath);
		}

		private void FileChanged(string fullPath)
		{
			Log.WriteLineForActivity($"FileChanged {fullPath}");

			try
			{
				var file = new FileInfo(fullPath);
				if (file.IsReadOnly)
				{
					//EnableRaisingEvents = false;
					SetReadOnlyFlagForFile(file, false);
					//EnableRaisingEvents = true;
				}
			}
			catch (DirectoryNotFoundException ex)
			{
				Log.WriteLineForError("======================EXCEPTION_THROWN=========================");
				Log.WriteLineForError("File Not Found: " + fullPath);
				Log.WriteLineForError("======================END_EXCEPTION=========================");
				// throw ex;
			}
			catch (Exception ex)
			{
				Log.WriteLineForError("======================EXCEPTION_THROWN=========================");
				Log.WriteLineForError("There was an unexpected exception throw.  Here are the details:");
				Log.WriteLineForError(ex.ToString());
				Log.WriteLineForError("======================END_EXCEPTION=========================");
				// throw ex;
			}
		}

		private void Watcher_Error(object sender, ErrorEventArgs e)
		{
			Log.WriteLineForError("=======================================================");
			Log.WriteLineForError($"ERROR: FileWatcher Received an Error!  The directory {Path} is no longer removing the readonly flag.");
			Log.WriteLineForError("=======================================================");
		}

		void SetReadOnlyFlagForFile(FileInfo file, bool isReadOnly)
		{
			if (file.Exists) file.IsReadOnly = isReadOnly;
		}

		private void RemoveReadOnlyFromDirectory()
		{
			var dir = new DirectoryInfo(Path);
			SetReadOnlyFlagForAllFiles(dir, false);
		}

		void SetReadOnlyFlagForAllFiles(DirectoryInfo directory, bool isReadOnly)
		{
			// Iterate over ALL files using "*" wildcard and choosing to search all directories.
			var searchOption = IncludeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
			foreach (FileInfo file in directory.GetFiles(Filter, searchOption)) //The trick is directories can be cast to FileInfo
			{
				// Set flag
				if (file.Exists) file.IsReadOnly = isReadOnly;
			}
		}
	}
}
