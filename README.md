# MilwaukeeMakerspaceApi
An internal web service for Milwaukee Makerspace

## I want to test this on a desktop
This is pretty easy to do on any common desktop platform:

### Windows
#### Recommended - Install the full Visual Studio IDE for a nice development experince
Get the installer here:

<https://visualstudio.microsoft.com/>

Be sure to select the `.Net Core cross-platform development` workload.

#### Alternate - Just install the necessary SDKs
Get the installer for the .Net Core 7.0 SDK here:

<https://dotnet.microsoft.com/en-us/download/dotnet/7.0>

### Mac
Get the installer for the .Net Core 7.0 SDK here:

<https://dotnet.microsoft.com/en-us/download/dotnet/7.0>

### Linux
Instructions good for anything Debian/Ubuntu based. Translating for other package managers of your choice should be trivial:

Install .Net Core 3.1 SDK:

Directions from here:

<https://learn.microsoft.com/en-us/dotnet/core/install/linux-debian>

	wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
	sudo dpkg -i packages-microsoft-prod.deb
	rm packages-microsoft-prod.deb

	sudo apt-get update && sudo apt-get install -y dotnet-sdk-7.0
	
### Now lets run the damn thing already

#### Use the command line (good for any of the above platforms):

Checkout the repository to a folder on your machine:

	git clone https://github.com/DanDude0/MilwaukeeMakerspaceApi

Switch to the project directory:

	cd MilwaukeeMakerspaceApi/MilwaukeeMakerspaceApi
	
Build the project:

	dotnet build
	
And run the project:

	dotnet run
	
If it loaded correctly, you are ready to test. It should be publishing a website at:

<http://127.0.0.1/>

And it should be broadcasting an SSDP service on the local network for key readers to locate. One such implementation is over here:

<https://github.com/DanDude0/MilwaukeeMakerspacePiFobReader>

## Upgrade an existing install

Run this command, it will do an inplace upgrade and reboot the reader automatically

	wget https://raw.githubusercontent.com/DanDude0/MilwaukeeMakerspaceApi/master/upgrade.sh
	chmod +x upgrade.sh
	sudo ./upgrade.sh

### It runs but nothing works!

You probably don't have a database server setup that it can work with. Use the following scripts to create the necessary tables and databases on on MariaDB/MySQL server of your choice.

[SQL Scripts/access_control_schema.sql](SQL Scripts/access_control_schema.sql)

[SQL Scripts/area_funding_schema.sql](SQL Scripts/area_funding_schema.sql)

[SQL Scripts/billing_schema.sql](SQL Scripts/billing_schema.sql)

Then setup the necessary connection strings in

[MilwaukeeMakerspaceApi/appsettings.json](MilwaukeeMakerspaceApi/appsettings.json)
