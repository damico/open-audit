
#pragma parseroption -b- -u+
#pragma verboselevel 9

#define MyAppName "Open Audit"
#define MyAppVersion "0.1"
#define MyAppPublisher "JOSE DAMICO"
#define MyAppURL "https:\\github.com\damico\open-audit"

[Setup]
AppPublisher=JOSE_DAMICO
AppPublisherURL=https:\\github.com\damico\open-audit
PrivilegesRequired=admin
AppName=OpenAudit
AppVersion=0.1
DefaultDirName={pf}\open-audit
DefaultGroupName=OpenAudit
Compression=lzma2
SolidCompression=yes
UninstallDisplayIcon={app}\open-audit-config.exe
SetupIconFile=pkg\Hopstarter-Scrap-Magnifying-Glass.ico
OutputBaseFilename=pkg\open-audit-setup-0.1.exe
OutputDir=.\

[Files]
Source: "pkg\open-audit-config.exe"; DestDir: "{app}";
Source: "pkg\open-audit-lib.dll"; DestDir: "{app}";
Source: "pkg\conf\open-audit.conf"; DestDir: "{app}\conf"
Source: "pkg\open-audit-service.exe"; DestDir: "{app}"; AfterInstall: AfterMyProgInstall()

[run]

Filename: {sys}\sc.exe; Parameters: "create open-audit-service start= auto binPath= ""{app}\open-audit-service.exe""" ; Flags: runhidden
Filename: {sys}\sc.exe; Parameters: "start open-audit-service" ; Flags: runhidden

[UninstallRun]
Filename: {sys}\sc.exe; Parameters: "stop open-audit-service" ; Flags: runhidden
Filename: {sys}\sc.exe; Parameters: "delete open-audit-service" ; Flags: runhidden

[Code]
procedure AfterMyProgInstall();
var
 ResultCode: Integer;
begin
 Exec(ExpandConstant('{app}\open-audit-config.exe'), '', '',SW_SHOW, ewWaitUntilTerminated, ResultCode);
end;
