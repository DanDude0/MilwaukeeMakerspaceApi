"D:\PuTTY\plink.exe" -P 9586 -i "V:\Data\File Vault\Dan PC.ppk" root@tank systemctl stop MilwaukeeMakerspaceApi
del /Q /S bin\Release\publish
dotnet publish -c Release -o bin\Release\publish
"D:\PuTTY\pscp.exe" -P 9586 -batch -r -i "V:\Data\File Vault\Dan PC.ppk" "bin\Release\publish\*" root@tank:/opt/MilwaukeeMakerspaceApi/
"D:\PuTTY\plink.exe" -P 9586 -i "V:\Data\File Vault\Dan PC.ppk" root@tank systemctl start MilwaukeeMakerspaceApi
