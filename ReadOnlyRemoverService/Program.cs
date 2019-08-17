using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ReadOnlyRemoverService
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			//try {

			//var service = new ReadOnlyRemoverService();
			//var paths = System.Configuration.ConfigurationManager.AppSettings["Paths"];

			//var logPathFromConfig = System.Configuration.ConfigurationManager.AppSettings["LogOutputDirectory"];
			//var isnull1 = String.IsNullOrWhiteSpace(logPathFromConfig);
			//var isnull2 = string.IsNullOrWhiteSpace(logPathFromConfig);
			//if (string.IsNullOrWhiteSpace(logPathFromConfig))
			//{
			//	if (!Directory.Exists(logPathFromConfig))
			//	{
			//		Directory.CreateDirectory(logPathFromConfig);
			//	}
			//	Log.Instance.LogPath = logPathFromConfig;
			//}

			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[]
			{
				new ReadOnlyRemoverService()
			};
			ServiceBase.Run(ServicesToRun);

			//var file = new FileInfo(@"C:\_tfs\mg\DT2 - StarnetFIT\Shubert.Libraries\madeupfile.txt");
			//file.IsReadOnly = false;
			//var readonlyRemover = new ReadOnlyRemoverService();


			//}
			//catch (Exception ex)
			//{
			//	// IMPORTANT NOTE:  When the service is installed and running
			//	// This does NOT catch any exceptions!!!
			//	Log.WriteLine("======================TOP_LEVEL_EXCEPTION_THROWN=========================");
			//	Log.WriteLine("There was an unexpected exception caught in Program.Main()  Here are the details:");
			//	Log.WriteLine(ex.ToString());
			//	Log.WriteLine("======================END_EXCEPTION=========================");
			//}
		}
	}
}
