;--------------------------------
;Include Modern UI

  !include "MUI.nsh"

;--------------------------------
;General

  ;Name and file
  Name "Razor"
  OutFile "Razor1013_Inst.exe"

  ;Default installation folder
  InstallDir "$PROGRAMFILES\Razor"
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKLM "Software\Razor" "InstallDir"


;--------------------------------
;Variables

  Var MUI_TEMP
  Var STARTMENU_FOLDER

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_LICENSE ".\License.txt"
  !insertmacro MUI_PAGE_DIRECTORY
  
  ;Start Menu Folder Page Configuration
  !define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKLM" 
  !define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\Razor" 
  !define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "StartMenuFolder"
  
  !insertmacro MUI_PAGE_STARTMENU Application $STARTMENU_FOLDER

  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Installer Sections

Section "Razor" 

  ;Store installation folder
  WriteRegStr HKLM "Software\Razor" "InstallDir" $INSTDIR
  WriteRegStr HKLM "Software\Razor" "UId" "0"

  ; For tugsoft plugins to function just make sure the tugsoft key exists... put in a dummy value
  WriteRegStr HKCU "Software\Tugsoft\UOAssist\ThirdParty" "Owner" "Razor"

  WriteRegStr HKCR ".rpv" "" "Razor.PacketVideo"
  WriteRegStr HKCR "Razor.PacketVideo\DefaultIcon" "" "$INSTDIR\RPVOpenStub.exe,0"
  WriteRegStr HKCR "Razor.PacketVideo\shell\open\command" "" '"$INSTDIR\RPVOpenStub.exe" %1'

  CreateDirectory "$INSTDIR\Language"

  SetOutPath "$INSTDIR"

  WriteUninstaller "$INSTDIR\Uninstall.exe"

  File ".\Razor.exe"
  File ".\RPVOpenStub.exe"
  ;File ".\RPVEditor.exe"
  File ".\Updater.exe"
  File ".\UnRar.dll"
  File ".\Crypt.dll"
  File ".\Loader.dll"
  File ".\Ultima.dll"
  File ".\zlib.dll"
  File ".\License.txt"
  File ".\License_CHS.txt"
  File ".\changelog.txt"

  SetOverwrite ifnewer
  File ".\Guardlines.def"
  File ".\Spells.def"
  File ".\Counters.xml"
  SetOverwrite on

  Delete "$INSTDIR\Crash.log"

  SetOutPath "$INSTDIR\Language"
  File ".\Language\Razor_lang.*"
  
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    
    ;Create shortcuts
    CreateDirectory "$SMPROGRAMS\$STARTMENU_FOLDER"
    CreateShortCut "$SMPROGRAMS\$STARTMENU_FOLDER\Razor.lnk" "$INSTDIR\Razor.exe"
    ;CreateShortcut "$SMPROGRAMS\$STARTMENU_FOLDER\RPV Editor (Alpha).lnk" "$INSTDIR\RPVEditor.exe"
    CreateShortCut "$SMPROGRAMS\$STARTMENU_FOLDER\Visit Razor's Website.lnk" "https://github.com/msturgill/razor"
    CreateShortCut "$SMPROGRAMS\$STARTMENU_FOLDER\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
    CreateShortcut "$DESKTOP\Razor.lnk" "$INSTDIR\Razor.exe"
  
  !insertmacro MUI_STARTMENU_WRITE_END

SectionEnd

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  DeleteRegKey HKCR ".rpv"
  DeleteRegKey HKCR "Razor.PacketVideo"
  DeleteRegKey HKLM "Software\Razor"
  DeleteRegKey HKCU "Software\Razor"

  Delete "$INSTDIR\Razor.exe"
  Delete "$INSTDIR\RPVOpenStub.exe"
  ;Delete "$INSTDIR\RPVEditor.exe"
  Delete "$INSTDIR\Crypt.dll"
  Delete "$INSTDIR\UOG.dll"
  Delete "$INSTDIR\Loader.dll"
  Delete "$INSTDIR\Ultima.dll"
  Delete "$INSTDIR\zlib.dll"
  Delete "$INSTDIR\Spells.def"
  Delete "$INSTDIR\Uninstall.exe"
  Delete "$INSTDIR\License.txt"
  Delete "$INSTDIR\License_CHS.txt"
  Delete "$INSTDIR\UOReport0.log"
  Delete "$INSTDIR\Crash.log"
  Delete "$INSTDIR\changelog.txt"
  Delete "$INSTDIR\Guardlines.def"
  RMDir /r "$INSTDIR\Language"

  MessageBox MB_YESNO|MB_ICONQUESTION "Do you want to save your razor profiles and macros?  (Selecting no will delete them)" IDYES AfterProfiles
    Delete "$INSTDIR\Counters.xml"
    RMDir /r "$INSTDIR\Profiles"
    RMDir /r "$INSTDIR\Macros"

AfterProfiles:

  MessageBox MB_YESNO|MB_ICONQUESTION "Do you want to save your screen shots and video captures?  (Selecting no will delete them)" IDYES AfterSS
    RMDir /r "$INSTDIR\ScreenShots"
    RMDir /r "$INSTDIR\Videos"

AfterSS:

  RMDir "$INSTDIR"
  
  !insertmacro MUI_STARTMENU_GETFOLDER Application $MUI_TEMP
    
  Delete "$SMPROGRAMS\$MUI_TEMP\Uninstall.lnk"
  Delete "$SMPROGRAMS\$MUI_TEMP\Razor.lnk"
  ;Delete "$SMPROGRAMS\$MUI_TEMP\RPV Editor (Alpha).lnk"
  Delete "$SMPROGRAMS\$MUI_TEMP\Visit Razor's Website.lnk"
  Delete "$DESKTOP\Razor.lnk"
  
  ;Delete empty start menu parent diretories
  StrCpy $MUI_TEMP "$SMPROGRAMS\$MUI_TEMP"
 
  startMenuDeleteLoop:
    RMDir $MUI_TEMP
    GetFullPathName $MUI_TEMP "$MUI_TEMP\.."
    
    IfErrors startMenuDeleteLoopDone
  
    StrCmp $MUI_TEMP $SMPROGRAMS startMenuDeleteLoopDone startMenuDeleteLoop
  startMenuDeleteLoopDone:

SectionEnd
