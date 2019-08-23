# Read-Only-Remover

A simple windows service to remove the readonly flag from files and directories.  Directories have all their files' readonly flags removed recursively.  The service removes the readonly flag from a list of paths in **LIVE TIME**.  So once the service is running, it is essentially impossible for a readonly flag for a file in the list of paths to be set to true.

## How to Use

This is a Windows service, and can only be installed on a computer running Windows.  [Download the ReadOnlyRemoverSetup.msi](https://github.com/mogelbuster/Read-Only-Remover/raw/master/ReadOnlyRemoverSetup.msi) installation file, and double click it to install.  Follow the directions in the installation wizard.  After install completes, open the installed location, which should be this folder path:
```
C:\Program Files (x86)\Mogelbuster\ReadOnlyRemoverSetup\
```
In that folder, open the file named `ReadOnlyRemover.config`.  I suggest opening the config file with a code editor that does syntax highlighting, like VSCode.  In the config file, you will find this bit:
```
<add key="Paths" value="C:\My\Path\To\A\Directory
                        C:\My\Path\To\A\File.txt" />
```
You will need to replace the part in quotes after `value=` with a list of directory paths and file paths.  This list is separated by newlines and spaces, so you can format them however you like.  I prefer putting each path on its' own line.

NOTE: If you include a directory, the readonly flag will be removed from all files in the directory recursively.

Next, open the services app.  You can do so by searching for 'services' in the Windows search bar.  This is where you can start and stop the service.  The name of the service in the list appears as `Read Only Remover`.  Find it, right click it, and select `Start`.  This will start the service, and the readonly flags for all the files and directories you specified in the config will be immediately removed, and will continue to be removed in live time whenever the flag is set to true.  When adding, deleting, or changing paths in the config file, you will need to restart the service for the changes to take effect.

## Getting Started with Development

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

You will need Visual Studio 2017 or later installed on you machine.  Earlier version might work, but I haven't tried them.

### Installing

Clone the project and open the solution.  You will see three projects.  `ReadOnlyRemoverService` is the service itself.  `RunServiceAsConsoleApp` is a wrapper around the service to enable it to be run as a console app, which makes debugging much easier.  `ReadOnlyRemoverSetup` is used to generate the end user .msi installation file.

Before trying to run the solution, right click the `RunServiceAsConsoleApp` project, and select `Set as StartUp Project`.  Then you should be good to go!  

### Building the Installation File

To build the .msi installation file yourself, set the `ReadOnlyRemoverService` project as the startup project (not 100% sure that is necessary), then right click the `ReadOnlyRemoverSetup` project and click Rebuild.  After successfully rebuilding, right click `ReadOnlyRemoverSetup` again and select `Open Folder in File Explorer`.  Depending on the build configuration you had selected in visual studios, navigate into either the `Debug` or `Release` folders.  In there you will find the recently built `ReadOnlyRemoverSetup.msi` file.  Double click it to install.

NOTE: The config file `ReadOnlyRemover.config` is set up to be used by all three projects, and will be copied to the correct folder depending on if you build and run as a console app, or if you generate and install the .msi installation file.  So you can put your paths in that file however you run the service.

## Todo

* Test running `RunServiceAsConsoleApp` as startup project works
* Clean up code
* Remove the ReadMe.txt in the project, and make a separate section here for how to install the .exe as a service
* Add ability to mark directory as recursive or not
* Add new features for other file/directory operations - If you have suggestions, let me know!

## Contributing

There is no formal contibuting guide yet.

## Authors

* **Kenneth Connors** - *Initial work* - [Mogelbuster](https://github.com/mogelbuster)

See also the list of [contributors](https://github.com/mogelbuster/Read-Only-Remover/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE) file for details

## Acknowledgments

* Shout out to any articles I might have read that inspired this project.
