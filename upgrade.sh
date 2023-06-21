#!/bin/bash
[[ $EUID -ne 0 ]] && echo "This script must be run as root." && exit 1

heading ()
{
    echo -e "\n\n+- $1\n"
}

echo -e '\n\n\n\n####################\n#'
echo -e '#  This will upgrade MilwaukeeMakerspaceApi on this system'
echo -e '#\n####################\n'

systemctl stop MilwaukeeMakerspaceApi

rm -rf /opt/MilwaukeeMakerspaceApiOldOld
mv /opt/MilwaukeeMakerspaceApiOld /opt/MilwaukeeMakerspaceApiOldOld
mv /opt/MilwaukeeMakerspaceApi /opt/MilwaukeeMakerspaceApiOld
mkdir -p /opt/MilwaukeeMakerspaceApi

# Update the OS
heading 'Updating Operating System'
apt-get -y update
apt-get -y dist-upgrade

# Install .Net Core
heading 'Installing .Net Core'
apt-get -y install dotnet-runtime-7.0 aspnetcore-runtime-7.0

# Install MmsPiFobReader
heading 'Installer MilwaukeeMakerspaceApi'
cd /root
curl -s https://api.github.com/repos/DanDude0/MilwaukeeMakerspaceApi/releases/latest | grep -P "(?<=browser_download_url\": \")https://.*zip" -o | wget -i -
unzip -o MmsApi.zip -d /opt/MilwaukeeMakerspaceApi
rm -f MmsApi.zip

cp -p /opt/MilwaukeeMakerspaceApiOld/appsettings.json /opt/MilwaukeeMakerspaceApi/appsettings.json

echo 'Upgrade completed, restarting service'
systemctl start MilwaukeeMakerspaceApi

systemctl status MilwaukeeMakerspaceApi
