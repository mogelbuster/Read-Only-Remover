using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace RunServiceAsConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var ServiceToRun = new ReadOnlyRemoverService.ReadOnlyRemoverService();
			if (Environment.UserInteractive) {
				ServiceToRun.Start();
			} else {
				ServiceBase.Run(ServiceToRun);
				Console.ReadLine();
			}

		}
	}
}
