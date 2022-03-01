"D:\PuTTY\plink.exe" -i "V:\Data\File Vault\Dan PC.ppk" root@mvaccess.vfdworld.com systemctl stop MilwaukeeMakerspaceApi
del /Q /S bin\Release\publish
dotnet publish -c Release -o bin\Release\publish --self-contained -r linux-arm
del /Q /S bin\Release\publish\appsettings.json
"D:\PuTTY\pscp.exe" -batch -r -i "V:\Data\File Vault\Dan PC.ppk" "bin\Release\publish\*" root@mvaccess.vfdworld.com:/opt/MilwaukeeMakerspaceApi/
"D:\PuTTY\plink.exe" -i "V:\Data\File Vault\Dan PC.ppk" root@mvaccess.vfdworld.com systemctl start MilwaukeeMakerspaceApi
