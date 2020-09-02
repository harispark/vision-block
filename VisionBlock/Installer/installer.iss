; -- installer.iss --
[Setup]
AppName=VisionBlockInstaller
AppVersion=1.0
DefaultDirName={pf}\VisionBlock
DefaultGroupName=VisionBlock
Compression=lzma2
SolidCompression=yes
OutputBaseFilename=VisionBlockInstaller
OutputDir=.
UninstallDisplayIcon={app}\VisionBlockApplication.exe
UninstallDisplayName=InnoExample

[Files]
Source: "C:\Users\Arnaud-Perso\source\repos\vision-block\VisionBlock\VisionBlockApplication\bin\Debug\VisionBlockApplication.exe"; DestDir: "{app}"
Source: "C:\Users\Arnaud-Perso\source\repos\vision-block\VisionBlock\VisionBlockApplication\bin\Debug\ImageProcessing.dll"; DestDir: "{app}"
Source: "C:\Users\Arnaud-Perso\source\repos\vision-block\VisionBlock\VisionBlockApplication\bin\Debug\LiveCharts.dll"; DestDir: "{app}"
Source: "C:\Users\Arnaud-Perso\source\repos\vision-block\VisionBlock\VisionBlockApplication\bin\Debug\LiveCharts.Wpf.dll"; DestDir: "{app}"
Source: "C:\Users\Arnaud-Perso\source\repos\vision-block\VisionBlock\VisionBlockApplication\bin\Debug\MaterialDesignColors.dll"; DestDir: "{app}"
Source: "C:\Users\Arnaud-Perso\source\repos\vision-block\VisionBlock\VisionBlockApplication\bin\Debug\MaterialDesignThemes.Wpf.dll"; DestDir: "{app}"
Source: "C:\Users\Arnaud-Perso\source\repos\vision-block\VisionBlock\VisionBlockApplication\bin\Debug\Newtonsoft.Json.dll"; DestDir: "{app}"
Source: "C:\Users\Arnaud-Perso\source\repos\vision-block\VisionBlock\VisionBlockApplication\bin\Debug\Ookii.Dialogs.Wpf.dll"; DestDir: "{app}"
Source: "C:\Users\Arnaud-Perso\source\repos\vision-block\VisionBlock\VisionBlockApplication\bin\Debug\RazorEngine.dll"; DestDir: "{app}"
Source: "C:\Users\Arnaud-Perso\source\repos\vision-block\VisionBlock\VisionBlockApplication\bin\Debug\System.Collections.Immutable.dll"; DestDir: "{app}"
Source: "C:\Users\Arnaud-Perso\source\repos\vision-block\VisionBlock\VisionBlockApplication\bin\Debug\System.Diagnostics.StackTrace.dll"; DestDir: "{app}"
Source: "C:\Users\Arnaud-Perso\source\repos\vision-block\VisionBlock\VisionBlockApplication\bin\Debug\System.IO.Compression.dll"; DestDir: "{app}"
Source: "C:\Users\Arnaud-Perso\source\repos\vision-block\VisionBlock\VisionBlockApplication\bin\Debug\System.Net.Sockets.dll"; DestDir: "{app}"
Source: "C:\Users\Arnaud-Perso\source\repos\vision-block\VisionBlock\VisionBlockApplication\bin\Debug\System.Web.Razor.dll"; DestDir: "{app}"

[Icons]
Name: "{group}\VisionBlock"; Filename: "{app}\VisionBlockApplication.exe"
Name: "{group}\Uninstall"; Filename: "{uninstallexe}"