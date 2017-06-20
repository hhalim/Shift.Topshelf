# Shift.Topshelf
Run Shift server inside a windows service app container. Use [Topshelf](https://github.com/Topshelf/Topshelf) to configure Windows Service.

To setup this app:
- Setup storage media of your choice, install Redis server or MS SQL server. Check out the [Shift Quick Start - Infrastructure Setup](https://github.com/hhalim/Shift/wiki/Quick-Start#infrastructure-setup) section in the wiki for more information.
- Open and compile project in Visual Studio 2015 or later. 
- Update storage configuration in App.config file. Refer to [Shift Server wiki](https://github.com/hhalim/Shift/wiki/Shift-Server) for more configuration options. 
- Open a Windows command prompt window with Administrator privilege.
- Go to the folder where Shift.Topshelf.exe is located.
- Run `Shift.Topshelf.exe install` command to install the windows service. Use `uninstall` to uninstall the service.
- Open Windows Services management console, locate the service based on the `ServiceName` in App.config. Start the service.

You can use the [Shift.Demo.Client](https://github.com/hhalim/Shift.Demo.Client) console app to send jobs to the Shift Windows service server.

## Stopping Windows Service
Please note that stopping the windows service is similar to pushing the off power switch, which means that all running jobs may get stuck in **running** status without actually running in the server. The zombie jobs status will change into an **error** status when the original windows service process is running again.

My recommendation is to send **STOP** command to all the running jobs and also wait for jobs without cancelation handle to complete successfully first before turning off the windows service process. 
