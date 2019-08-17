How to use ReadOnlyRemover:

1. Open ReadOnlyRemoverService.exe.config and set your configs.
   (The config file contains its' own documentation on how to use.)

2. Open a command prompt (might need to use Visual Studio's Developer Command Prompt, and it might need to be run as admin)

3. cd into the folder containing ReadOnlyRemoverService.exe

4. Run these commands:
installutil.exe ReadOnlyRemoverService.exe
net start ReadOnlyRemover

5. You're done!  The files and directories will be watched, and if the read only flag is set to true for any file, it will be immediately set back to false.

6. To change the values in the config file, stop the service, make the changes, save the config, and start the service.  The file is locked while the service is running, and if it isn't locked, the service won't pick up the changes on the fly anyway.

7. To uninstall the service, run the following commands:
net stop ReadOnlyRemover
installutil.exe -u ReadOnlyRemoverService.exe