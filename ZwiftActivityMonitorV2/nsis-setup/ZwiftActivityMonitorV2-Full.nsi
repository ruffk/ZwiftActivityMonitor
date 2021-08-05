!verbose 4
Unicode True

!define PRODUCT_NAME "Zwift Activity Monitor Pro"
!define PRODUCT_VERSION "2.3.0"
!define PRODUCT_PUBLISHER "Kevin Ruff p/b EnJoy Fitness"
!define PRODUCT_WEB_SITE "https://github.com/ruffk/ZwiftActivityMonitor"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\ZwiftActivityMonitorV2.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

!define NPCAP_INSTALLER "npcap-1.50.exe"
!define NPCAP_INSTALLER_VERSION "1.50"

!define DOTNET_INSTALLER "windowsdesktop-runtime-5.0.8-win-x64.exe"
!define DOTNET_INSTALLER_VERSION "5.0.8"

!include "MUI2.nsh"

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\modern-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; Welcome page
!insertmacro MUI_PAGE_WELCOME

; Directory page
!insertmacro MUI_PAGE_DIRECTORY

; Instfiles page
!insertmacro MUI_PAGE_INSTFILES

; Finish page
!define MUI_FINISHPAGE_TEXT "Please take a moment to view the application's README file.  It contains important setup instructions."
!define MUI_FINISHPAGE_RUN "$INSTDIR\ZwiftActivityMonitorV2.exe"
!define MUI_FINISHPAGE_RUN_NOTCHECKED
!define MUI_FINISHPAGE_SHOWREADME "https://github.com/ruffk/ZwiftActivityMonitor#readme"
;!define MUI_FINISHPAGE_SHOWREADME_TEXT ""

!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files
!insertmacro MUI_LANGUAGE "English"

; MUI end ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "Setup-ZAM.exe"
;InstallDir "$PROGRAMFILES\Zwift Activity Monitor"
InstallDir "C:\Zwift Activity Monitor Pro"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowInstDetails show
ShowUnInstDetails show

Section "MainSection" SEC01

  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  File \
      /x "appsettings.json" \
      /x "appsettings.Production.json" \
      "..\bin\Release\net5.0-windows\*.*"

  ; don't overwrite the two .json config file
  SetOverwrite off
  File "C:\Users\kevin\Documents\GitHub\ZwiftActivityMonitor\ZwiftActivityMonitorV2\bin\Release\net5.0-windows\appsettings.json"
  File "C:\Users\kevin\Documents\GitHub\ZwiftActivityMonitor\ZwiftActivityMonitorV2\bin\Release\net5.0-windows\appsettings.Production.json"

  CreateDirectory "$SMPROGRAMS\Zwift Activity Monitor Pro"
  CreateShortCut "$SMPROGRAMS\Zwift Activity Monitor Pro\Zwift Activity Monitor Pro.lnk" "$INSTDIR\ZwiftActivityMonitorV2.exe"
  CreateShortCut "$DESKTOP\Zwift Activity Monitor Pro.lnk" "$INSTDIR\ZwiftActivityMonitorV2.exe"

  SetAutoClose true
SectionEnd

Section "Prerequisites" SEC02
  SetOutPath "$INSTDIR\Prerequisites"
  SetOverwrite ifnewer
  File \
      ".\Prerequisites\${NPCAP_INSTALLER}" \
      ".\Prerequisites\${DOTNET_INSTALLER}"

  
  ClearErrors
  ReadRegStr $0 HKLM "Software\WOW6432Node\Npcap" ""
  IfErrors 0 SkipNpcap 
    MessageBox MB_YESNO|MB_ICONQUESTION "The pre-requisite application Npcap ${NPCAP_INSTALLER_VERSION} needs to be installed.  Select Yes to install it now." IDNO SkipNpcap
    MessageBox MB_OKCANCEL "During the Npcap installation, please accept all installation default settings.  Select OK to continue." IDCANCEL SkipNpcap
    ExecWait "$INSTDIR\Prerequisites\${NPCAP_INSTALLER}" $0
  SkipNpcap:

  ReadRegDWORD $0 HKLM "Software\WOW6432Node\dotnet\Setup\InstalledVersions\x64\sharedfx\Microsoft.WindowsDesktop.App" "${DOTNET_INSTALLER_VERSION}"
  IfErrors 0 SkipDotNet 
    MessageBox MB_YESNO|MB_ICONQUESTION "The pre-requisite Microsoft .NET ${DOTNET_INSTALLER_VERSION} Desktop Runtime needs to be installed.  Select Yes to install it now." IDNO SkipDotNet
    ExecWait "$INSTDIR\Prerequisites\${DOTNET_INSTALLER}" $0
  SkipDotNet:
SectionEnd

Section -AdditionalIcons
  ;WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\Zwift Activity Monitor Pro\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\Zwift Activity Monitor Pro\Uninstall.lnk" "$INSTDIR\uninst-ZAM.exe"
SectionEnd

Section -Post
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\ZwiftActivityMonitorV2.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "Path" "$INSTDIR"
  
  WriteRegExpandStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst-ZAM.exe"
  WriteRegExpandStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "InstallLocation" "$INSTDIR"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\ZwiftActivityMonitorV2.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"

  WriteUninstaller "$INSTDIR\uninst-ZAM.exe"

SectionEnd


Function .onInit
  ;Not using the normal C:\Program Files directory as it doesn't allow file writes (.json config) if user isn't admin
  StrCpy $INSTDIR "C:\Zwift Activity Monitor Pro"
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) was successfully removed from your computer."
FunctionEnd

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove $(^Name) and all of its components (except configuration files)?" IDYES +2
  Abort
FunctionEnd

Section Uninstall
  ReadRegStr $0 HKLM "${PRODUCT_DIR_REGKEY}" "Path"

  IfFileExists $0\ZwiftActivityMonitorV2.exe zam_installed
    MessageBox MB_YESNO "It does not appear that Zwift Activity Monitor Pro is installed in the directory '$0'.$\r$\nContinue anyway (not recommended)?" IDYES zam_installed
    Abort "Uninstall aborted by user"
  zam_installed:

  Delete "$0\${PRODUCT_NAME}.url"
  Delete "$INSTDIR\uninst-ZAM.exe"
  Delete "$0\*.exe"
  Delete "$0\*.dll"
  Delete "$0\ZwiftActivityMonitor*.json"
  ;This will leave the appsettings*.json and ZAMSettings*.json files

  Delete "$SMPROGRAMS\Zwift Activity Monitor Pro\Uninstall.lnk"
  Delete "$SMPROGRAMS\Zwift Activity Monitor Pro\Website.lnk"
  Delete "$DESKTOP\Zwift Activity Monitor Pro.lnk"
  Delete "$SMPROGRAMS\Zwift Activity Monitor Pro\Zwift Activity Monitor Pro.lnk"

  RMDir "$SMPROGRAMS\Zwift Activity Monitor Pro"
  ; Do not attempt to remove directory because we want config files preserved
  ;RMDir /r "$0"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd