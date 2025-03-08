cd /d %~dp0

@REM This script is used to publish the project to a folder and compress it to a 7z file.
@REM You should have 7z installed and added to PATH.
@REM You should prepare the bin file ffmpeg.exe and ffplay.exe in publish folder.

for /f "usebackq tokens=*" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -property installationPath`) do set "path=%path%;%%i\MSBuild\Current\Bin;%%i\Common7\IDE"
msbuild ..\src\PickColor.csproj /t:Rebuild /p:Configuration=Release /p:DeployOnBuild=true /p:PublishProfile=FolderProfile /restore
del /s /q publish.7z
7z a publish.7z ..\src\bin\Release\net48\* -t7z -mx=5 -mf=BCJ2 -r -y
for /f "usebackq delims=" %%i in (`powershell -NoLogo -NoProfile -Command "Get-Content '..\src\PickColor.csproj' | Select-String -Pattern '<AssemblyVersion>(.*?)</AssemblyVersion>' | ForEach-Object { $_.Matches.Groups[1].Value }"`) do @set version=%%i
del /s /q PickColor_v%version%_win64.7z
makemica micasetup.json
rename publish.7z PickColor_v%version%_win64.7z

@pause
