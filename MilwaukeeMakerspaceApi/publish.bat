"D:\PuTTY\plink.exe" -i "V:\Data\File Vault\Dan PC.ppk" root@mmsaccess.vfdworld.com systemctl stop MilwaukeeMakerspaceApi
del /Q /S bin\Release\publish
dotnet publish -c Release -o bin\Release\publish
del /Q /S bin\Release\publish\appsettings.json
"D:\PuTTY\pscp.exe" -batch -r -i "V:\Data\File Vault\Dan PC.ppk" "bin\Release\publish\*" root@mmsaccess.vfdworld.com:/opt/MilwaukeeMakerspaceApi/
"D:\PuTTY\plink.exe" -i "V:\Data\File Vault\Dan PC.ppk" root@mmsaccess.vfdworld.com systemctl start MilwaukeeMakerspaceApi
