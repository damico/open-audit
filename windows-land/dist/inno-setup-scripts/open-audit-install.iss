
#pragma parseroption -b- -u+
#pragma verboselevel 9
#define MyAppName "Open Audit"
#define MyAppVersion "1.0"
#define MyAppPublisher "PEDRO CHAGAS"
#define MyAppURL "https:\\github.com\damico\open-audit"

[Setup]
AppId=OpenAudit-windows-land
AppPublisher=EIG Mercados
AppPublisherURL=https:\\github.com\damico\open-audit
PrivilegesRequired=admin
AppName=OpenAudit
AppVersion=1.0
DefaultDirName={pf}\open-audit
DefaultGroupName=OpenAudit
Compression=lzma2
SolidCompression=yes
UninstallDisplayIcon={app}\open-audit-config.exe
SetupIconFile=pkg\Hopstarter-Scrap-Magnifying-Glass.ico
OutputBaseFilename=pkg\open-audit-setup-1.0
OutputDir=.\


[Files]
Source: "..\..\open-audit-config\bin\release\open-audit-config.exe"; DestDir: "{app}";
Source: "..\..\open-audit-config\bin\release\open-audit-lib.dll"; DestDir: "{app}";
Source: "pkg\conf\open-audit.conf"; DestDir: "{app}\conf";
Source: "pkg\conf\up.dat"; DestDir: "{app}\conf";
Source: "..\..\open-audit-check-service\bin\release\open-audit-check-service.exe"; DestDir: "{app}";
Source: "..\..\open-audit-update-service\bin\release\open-audit-update-service.exe"; DestDir: "{app}";
Source: "..\..\open-audit-service\bin\release\open-audit-service.exe"; DestDir: "{app}"; AfterInstall: AfterMyProgInstall()


[Run]
Filename: "{app}\open-audit-service.exe"; Parameters: "--install"; 
Filename: "{app}\open-audit-service.exe"; Parameters: "--start";
Filename: "{app}\open-audit-check-service.exe"; Parameters: "--install"; 
Filename: "{app}\open-audit-check-service.exe"; Parameters: "--start";

[UninstallRun]
Filename: "{app}\open-audit-service.exe"; Parameters: "--uninstall";
Filename: "{app}\open-audit-check-service.exe"; Parameters: "--uninstall";

[Code]
function GetCommandlineParam ():String; 
var
  ParamCnt : LongInt;
	ii : LongInt;
begin  
  Result := ''; 
  ParamCnt := ParamCount();
	for ii := 0 to ParamCnt do
	begin
		Result := ParamStr(ii)
	end; 
end; 

procedure AfterMyProgInstall();
var
 ResultCode: Integer;
 CmdParam: String;
begin
 CmdParam:= GetCommandlineParam ();
 Exec(ExpandConstant('{app}\open-audit-config.exe'), CmdParam, '',SW_SHOW, ewWaitUntilTerminated, ResultCode);
end;

const
  DELAY_MILLIS = 250;
  MAX_DELAY_MILLIS = 30000;


function GetUninstallString(): String;
var
  uninstallPath: String;
  uninstallStr: String;
begin
  uninstallPath := ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{#emit SetupSetting("AppId")}_is1');
  uninstallStr := '';
  if not RegQueryStringValue(HKLM, uninstallPath, 'UninstallString', uninstallStr) then
    RegQueryStringValue(HKCU, uninstallPath, 'UninstallString', uninstallStr);
  Result := RemoveQuotes(uninstallStr);
end;


function ForceUninstallApplication(): Boolean;
var
  ResultCode: Integer;
  uninstallStr: String;
  delayCounter: Integer;
begin
  // 1) Uninstall the application
  Log('forcing uninstall of application');
  uninstallStr := GetUninstallString();
  Result := Exec(uninstallStr, '/SILENT /NORESTART /SUPPRESSMSGBOXES /LOG', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) and (ResultCode = 0);
  if not Result then
  begin
    Log('application uninstall failed!');
    Exit
  end;
  Log('application uninstalled!');

  // 2) Be sure to wait a while, until the actual uninstaller is deleted!
  Log('waiting a while until uninstaller changes are flushed in the filesystem...');
  delayCounter := 0;
  repeat
    Sleep(DELAY_MILLIS);
    delayCounter := delayCounter + DELAY_MILLIS;
  until not FileExists(uninstallStr) or (delayCounter >= MAX_DELAY_MILLIS);
  if (delayCounter >= MAX_DELAY_MILLIS) then
    RaiseException('Timeout exceeded trying to delete uninstaller: ' + uninstallStr);
  Log('waited ' + IntToStr(delayCounter) + ' milliseconds');
end;

function PrepareToInstall(var NeedsRestart: Boolean): String;
begin
  ForceUninstallApplication();
end;
