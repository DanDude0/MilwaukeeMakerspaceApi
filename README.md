# MilwaukeeMakerspaceApi
An internal web service for Milwaukee Makerspace

## I want to test this on a desktop
This is pretty easy to do on any common desktop platform:

### Windows
#### Recommended - Install the full Visual Studio IDE for a nice development experince
Get the installer here:

<https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&rel=15>

Be sure to select the `.Net Core cross-platform development` workload.

#### Alternate - Just install the necessary SDKs
Get the installer for the .Net Core 2.1 SDK here:

<https://www.microsoft.com/net/download/thank-you/dotnet-sdk-2.1.402-windows-x64-installer>

### Mac
Get the installer for the .Net Core 2.1 SDK here:

<https://www.microsoft.com/net/download/thank-you/dotnet-sdk-2.1.402-windows-x64-installer>

### Linux
Instructions good for anything Debian/Ubuntu based. Translating for other package managers of your choice should be trivial:

Install .Net Core 2.1 SDK:

Directions from here:

<https://www.microsoft.com/net/download/linux-package-manager/debian9/sdk-current>

	wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg
	mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/
	wget -q https://packages.microsoft.com/config/debian/9/prod.list
	mv prod.list /etc/apt/sources.list.d/microsoft-prod.list
	chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg
	chown root:root /etc/apt/sources.list.d/microsoft-prod.list

	apt-get update
	apt-get install dotnet-sdk-2.1

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