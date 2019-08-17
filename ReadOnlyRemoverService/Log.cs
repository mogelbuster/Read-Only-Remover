using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadOnlyRemoverService
{
	public class Log
	{

		#region Singleton

		private static readonly Lazy<Log> Lazy = new Lazy<Log>(() => new Log());

		public static Log Instance { get { return Lazy.Value; } }

		internal Log()
		{
			LogFileName = "Example";
			LogFileExtension = ".log";
		}

		#endregion

		public StreamWriter Writer { get; set; }

		public string LogPath
		{
			get { return _LogPath ?? (_LogPath = AppDomain.CurrentDomain.BaseDirectory); }
			set { _LogPath = value; }
		}
		private string _LogPath;

		public bool ShouldLogActivity = false;
		public bool ShouldLogErrors = false;

		public string LogFileName { get; set; }

		public string LogFileExtension { get; set; }

		public string LogFile { get { return LogFileName + LogFileExtension; } }

		public string LogFullPath { get { return Path.Combine(LogPath, LogFile); } }

		public bool LogExists { get { return File.Exists(LogFullPath); } }

		public void WriteLineToLog(string inLogMessage)
		{
			WriteToLog(DateTime.Now.ToString() + " - " + inLogMessage + Environment.NewLine);
		}

		public void WriteToLog(string inLogMessage)
		{
			if (!Directory.Exists(LogPath))
			{
				Directory.CreateDirectory(LogPath);
			}
			if (Writer == null)
			{
				Writer = new StreamWriter(LogFullPath, true);
			}

			Writer.Write(inLogMessage);
			Writer.Flush();
		}

		public static void WriteLineForError(string inLogMessage)
		{
			if (Instance.ShouldLogErrors) Instance.WriteLineToLog(inLogMessage);
		}

		public static void WriteLineForActivity(string inLogMessage)
		{
			if (Instance.ShouldLogActivity) Instance.WriteLineToLog(inLogMessage);
		}

		public static void WriteLine(string inLogMessage)
		{
			Instance.WriteLineToLog(inLogMessage);
		}

		public static void Write(string inLogMessage)
		{
			Instance.WriteToLog(inLogMessage);
		}
	}
}
