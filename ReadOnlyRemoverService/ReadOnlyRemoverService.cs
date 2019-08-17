using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ReadOnlyRemoverService
{
	public partial class ReadOnlyRemoverService : ServiceBase
	{
		//protected FileSystemWatcher Watcher;
		protected List<ReadOnlyRemover> ReadOnlyRemovers = new List<ReadOnlyRemover>();
		private List<string> _paths;

		// Directory must already exist unless you want to add your own code to create it.
		//string PathToFolder = @"C:\_tfs\mg\DT2-StarnetFIT\Shubert.Libraries";

		public ReadOnlyRemoverService()
		{
			SetShouldLogFlags();
			SetLogFileName();
			SetLogPath();

			//Watcher = new DirectoryWatcher(PathToFolder);
			
			_paths = (ConfigurationManager.AppSettings["Paths"] ?? "")
							.Split(new string[] { Environment.NewLine, " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)
							.Select(s => s.Trim())
							.Where(s => !string.IsNullOrWhiteSpace(s))
							.ToList();
			

			InitializeComponent();
		}

		private void SetShouldLogFlags()
		{
			var shouldLogActivity = (ConfigurationManager.AppSettings["ShouldLogActivity"] ?? "").Trim();
			if (string.Equals(shouldLogActivity, "true", StringComparison.OrdinalIgnoreCase))
				Log.Instance.ShouldLogActivity = true;
			else
				Log.Instance.ShouldLogActivity = false;

			var shouldLogErrors = (ConfigurationManager.AppSettings["ShouldLogErrors"] ?? "").Trim();
			if (string.Equals(shouldLogErrors, "true", StringComparison.OrdinalIgnoreCase))
				Log.Instance.ShouldLogErrors = true;
			else
				Log.Instance.ShouldLogErrors = false;
		}

		private void SetLogFileName()
		{
			var logFileNameFromConfig = ConfigurationManager.AppSettings["LogFileName"];
			if (!string.IsNullOrWhiteSpace(logFileNameFromConfig))
				Log.Instance.LogFileName = logFileNameFromConfig;
			else
				Log.Instance.LogFileName = "ReadOnlyRemover"; // The default log name.
		}

		private void SetLogPath()
		{
			//Log.Instance.LogPath = @"C:\__TestLog";
			try
			{
				var logPathFromConfig = ConfigurationManager.AppSettings["LogOutputDirectory"];
				if (!string.IsNullOrWhiteSpace(logPathFromConfig))
				{
					if (!Directory.Exists(logPathFromConfig))
					{
						Directory.CreateDirectory(logPathFromConfig);
					}
					Log.Instance.LogPath = logPathFromConfig;
				}
			}
			catch (Exception ex)
			{
				Log.WriteLineForError("======================EXCEPTION_THROWN=========================");
				Log.WriteLineForError("Error Setting Up Log Path from config: " + ex.ToString());
				Log.WriteLineForError("======================END_EXCEPTION=========================");
				// This is fine, the Log class automatically falls back to the folder
				// where the executable for the service is.

				// TODO: move Log class setup to here, and out of Log class.
				// change Log class from singleton to regular class and use dependency injection instead.
			}
		}

		

		private void StartRemovingReadOnlyFromPath(string path)
		{
			try
			{
				var fileAttributes = File.GetAttributes(path);
				if (fileAttributes.HasFlag(FileAttributes.Directory))
					StartRemovingReadOnlyFromDirectory(path);
				else
					StartRemovingReadOnlyFromFile(path);
			}
			catch (Exception ex)
			{
				Log.WriteLineForError("======================EXCEPTION_THROWN=========================");
				Log.WriteLineForError("Error in StartRemovingReadOnlyFromPath:");
				Log.WriteLineForError(ex.ToString());
				Log.WriteLineForError("======================END_EXCEPTION=========================");
			}
		}

		private void StartRemovingReadOnlyFromDirectory(string directory)
		{
			Log.WriteLineForActivity($"StartRemovingReadOnlyFromDirectory {directory}");
			var Watcher = new ReadOnlyRemover(directory);
			ReadOnlyRemovers.Add(Watcher);
		}

		private void StartRemovingReadOnlyFromFile(string filepath)
		{
			Log.WriteLineForActivity($"StartRemovingReadOnlyFromFile {filepath}");
			var directory = Path.GetDirectoryName(filepath);
			var filename = Path.GetFileName(filepath);
			ReadOnlyRemovers.Add(new ReadOnlyRemover(directory, filename, false));
		}

		public void Start()
		{
			OnStart(new string[1]);
			Console.ReadLine();
		}

		protected override void OnStart(string[] args)
		{
			Log.WriteLineForActivity(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
			Log.WriteLineForActivity($"{ServiceName} has started.");
			Log.WriteLineForActivity(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

			foreach (var path in _paths) //TODO: if a duplicate directory or file, ignore it (maybe use a HashSet)
			{
				Log.WriteLineForActivity($"Path={path}");
				StartRemovingReadOnlyFromPath(path);
			}

			var test = ConfigurationManager.AppSettings["Paths"] ?? "sample2";
			Console.WriteLine(test);
		}

		protected override void OnStop()
		{
			Log.WriteLineForActivity("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
			Log.WriteLineForActivity($"{ServiceName} has stopped.");
			Log.WriteLineForActivity("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
		}
	}
}
