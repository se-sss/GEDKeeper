﻿; "GEDKeeper", the personal genealogical database editor.
; Copyright (C) 2009-2023 by Sergey V. Zhdanovskih.
; This file is part of "GEDKeeper".

!include "MUI2.nsh"
!include "DotNetChecker.nsh"
!define MUI_ICON "..\projects\GKv2\GEDKeeper2\GEDKeeper2.ico"


;Unicode true
Name "GEDKeeper"
OutFile "gedkeeper_2.26.1_win86.exe"
InstallDir $PROGRAMFILES\GEDKeeper2

CRCCheck on
SetCompress auto
;SetCompressor lzma
SetDatablockOptimize on
AllowRootDirInstall false
XPStyle on

ShowInstDetails show
RequestExecutionLevel admin


!insertmacro MUI_LANGUAGE "English"
LangString gkreq ${LANG_ENGLISH} "GEDKeeper2 (required)"
LangString gkscr ${LANG_ENGLISH} "Script samples"
LangString gkreg ${LANG_ENGLISH} "System registration"
LangString gklang ${LANG_ENGLISH} "Languages"
LangString gkplg ${LANG_ENGLISH} "Plugins"
LangString gkp_calc ${LANG_ENGLISH} "Expression calculator"
LangString gkp_calendar ${LANG_ENGLISH} "Calendar"
LangString gkp_nb ${LANG_ENGLISH} "Names book"
LangString gkp_timeline ${LANG_ENGLISH} "Time line"
LangString gkp_flowinput ${LANG_ENGLISH} "Flow input"
LangString gkp_pi ${LANG_ENGLISH} "Pedigrees importer"
LangString gkp_ts ${LANG_ENGLISH} "Text search"
LangString gkp_cl ${LANG_ENGLISH} "Conway Life"
LangString gkp_chron ${LANG_ENGLISH} "Chronicle"
LangString gkp_cloud ${LANG_ENGLISH} "Words Cloud"
LangString gkp_histdata ${LANG_ENGLISH} "History Data (links)"
LangString gkp_stdreports ${LANG_ENGLISH} "Standard Reports"
LangString gkp_navig ${LANG_ENGLISH} "Navigator"
LangString gk_shellopen ${LANG_ENGLISH} "Open"

!insertmacro MUI_LANGUAGE "Russian"
LangString gkreq ${LANG_RUSSIAN} "GEDKeeper2 (необходимо)"
LangString gkscr ${LANG_RUSSIAN} "Примеры скриптов"
LangString gkreg ${LANG_RUSSIAN} "Регистрация в системе"
LangString gklang ${LANG_RUSSIAN} "Языки"
LangString gkplg ${LANG_RUSSIAN} "Плагины"
LangString gkp_calc ${LANG_RUSSIAN} "Калькулятор выражений"
LangString gkp_calendar ${LANG_RUSSIAN} "Календарь"
LangString gkp_nb ${LANG_RUSSIAN} "Справочник имен"
LangString gkp_timeline ${LANG_RUSSIAN} "Линия времени"
LangString gkp_flowinput ${LANG_RUSSIAN} "Поточный ввод"
LangString gkp_pi ${LANG_RUSSIAN} "Импорт росписей"
LangString gkp_ts ${LANG_RUSSIAN} "Полнотекстовый поиск"
LangString gkp_cl ${LANG_RUSSIAN} "Игра 'Жизнь Конвея'"
LangString gkp_chron ${LANG_RUSSIAN} "Хроника"
LangString gkp_cloud ${LANG_RUSSIAN} "Облако слов/тэгов"
LangString gkp_histdata ${LANG_RUSSIAN} "Исторические данные (ссылки)"
LangString gkp_stdreports ${LANG_RUSSIAN} "Стандартные отчеты"
LangString gkp_navig ${LANG_RUSSIAN} "Навигатор"
LangString gk_shellopen ${LANG_RUSSIAN} "Открыть"

!insertmacro MUI_LANGUAGE "Ukrainian"
LangString gkreq ${LANG_UKRAINIAN} "GEDKeeper2 (потрібний)"
LangString gkscr ${LANG_UKRAINIAN} "Приклади скриптів"
LangString gkreg ${LANG_UKRAINIAN} "Реєстрація в системі"
LangString gklang ${LANG_UKRAINIAN} "Мови"
LangString gkplg ${LANG_UKRAINIAN} "Плагіни"
LangString gkp_calc ${LANG_UKRAINIAN} "Калькулятор"
LangString gkp_calendar ${LANG_UKRAINIAN} "Календар"
LangString gkp_nb ${LANG_UKRAINIAN} "Довідник імен"
LangString gkp_timeline ${LANG_UKRAINIAN} "Линія часу"
LangString gkp_flowinput ${LANG_UKRAINIAN} "Поточне введення"
LangString gkp_pi ${LANG_UKRAINIAN} "Імпорт розписів"
LangString gkp_ts ${LANG_UKRAINIAN} "Повнотекстовий пошук"
LangString gkp_cl ${LANG_UKRAINIAN} "Гра 'Життя Конвея'"
LangString gkp_chron ${LANG_UKRAINIAN} "Хроніка"
LangString gkp_cloud ${LANG_UKRAINIAN} "Слова хмара"
LangString gkp_histdata ${LANG_UKRAINIAN} "Історія даних (посилання)"
LangString gkp_stdreports ${LANG_UKRAINIAN} "Стандартні звіти"
LangString gkp_navig ${LANG_UKRAINIAN} "Навігатор"
LangString gk_shellopen ${LANG_UKRAINIAN} "Відкрити"

!insertmacro MUI_LANGUAGE "Polish"
LangString gkreq ${LANG_POLISH} "GEDKeeper2 (właściwy)"
LangString gkscr ${LANG_POLISH} "Przykłady skryptów"
LangString gkreg ${LANG_POLISH} "Rejestracja w systemie"
LangString gklang ${LANG_POLISH} "Języki"
LangString gkplg ${LANG_POLISH} "Wtyczki"
LangString gkp_calc ${LANG_POLISH} "Kalkulator"
LangString gkp_calendar ${LANG_POLISH} "Kalendarz"
LangString gkp_nb ${LANG_POLISH} "Kieszonkowy nazw"
LangString gkp_timeline ${LANG_POLISH} "Linia czasu"
LangString gkp_flowinput ${LANG_POLISH} "Źródło wejścia"
LangString gkp_pi ${LANG_POLISH} "Importuj rodowód"
LangString gkp_ts ${LANG_POLISH} "Wyszukiwanie pełnotekstowe"
LangString gkp_cl ${LANG_POLISH} "Gra w życie"
LangString gkp_chron ${LANG_POLISH} "Kronika"
LangString gkp_cloud ${LANG_POLISH} "Chmura słów"
LangString gkp_histdata ${LANG_POLISH} "Dane historyczne (linki)"
LangString gkp_stdreports ${LANG_POLISH} "Raporty standardowe"
LangString gkp_navig ${LANG_POLISH} "Nawigator"
LangString gk_shellopen ${LANG_POLISH} "Otworz"

!insertmacro MUI_LANGUAGE "French"
LangString gkreq ${LANG_FRENCH} "GEDKeeper2 (obligatoire)"
LangString gkscr ${LANG_FRENCH} "Échantillons de script"
LangString gkreg ${LANG_FRENCH} "Enregistrement du système"
LangString gklang ${LANG_FRENCH} "Langues"
LangString gkplg ${LANG_FRENCH} "Plugins"
LangString gkp_calc ${LANG_FRENCH} "Calculateur d'expression"
LangString gkp_calendar ${LANG_FRENCH} "Calendrier"
LangString gkp_nb ${LANG_FRENCH} "Livre des noms"
LangString gkp_timeline ${LANG_FRENCH} "Chronologie"
LangString gkp_flowinput ${LANG_FRENCH} "Flux d'entrée"
LangString gkp_pi ${LANG_FRENCH} "Pedigrees importateur"
LangString gkp_ts ${LANG_FRENCH} "Recherche de texte"
LangString gkp_cl ${LANG_FRENCH} "Jeu de la vie"
LangString gkp_chron ${LANG_FRENCH} "Chronique"
LangString gkp_cloud ${LANG_FRENCH} "Nuage de mots"
LangString gkp_histdata ${LANG_FRENCH} "Données historiques (liens)"
LangString gkp_stdreports ${LANG_FRENCH} "Rapports standards"
LangString gkp_navig ${LANG_FRENCH} "Navigateur"
LangString gk_shellopen ${LANG_FRENCH} "Ouvrir"

!insertmacro MUI_LANGUAGE "Italian"
LangString gkreq ${LANG_ITALIAN} "GEDKeeper2 (necessario)"
LangString gkscr ${LANG_ITALIAN} "Esempi di script"
LangString gkreg ${LANG_ITALIAN} "Registrazione del sistema"
LangString gklang ${LANG_ITALIAN} "Le lingue"
LangString gkplg ${LANG_ITALIAN} "Plugins"
LangString gkp_calc ${LANG_ITALIAN} "Calcolatrice di espressione"
LangString gkp_calendar ${LANG_ITALIAN} "Calendario"
LangString gkp_nb ${LANG_ITALIAN} "Libro di nomi"
LangString gkp_timeline ${LANG_ITALIAN} "Sequenza temporale"
LangString gkp_flowinput ${LANG_ITALIAN} "Ingresso flusso"
LangString gkp_pi ${LANG_ITALIAN} "Pedigrees importer"
LangString gkp_ts ${LANG_ITALIAN} "Ricerca testo"
LangString gkp_cl ${LANG_ITALIAN} "Conway Life"
LangString gkp_chron ${LANG_ITALIAN} "Cronaca"
LangString gkp_cloud ${LANG_ITALIAN} "Nuvola di parole"
LangString gkp_histdata ${LANG_ITALIAN} "Dati storici (collegamenti)"
LangString gkp_stdreports ${LANG_ITALIAN} "Rapporti standard"
LangString gkp_navig ${LANG_ITALIAN} "Navigatore"
LangString gk_shellopen ${LANG_ITALIAN} "Apri"

!insertmacro MUI_LANGUAGE "German"
LangString gkreq ${LANG_GERMAN} "GEDKeeper2 (benötigt)"
LangString gkscr ${LANG_GERMAN} "Skript Proben"
LangString gkreg ${LANG_GERMAN} "Systemregistrierung"
LangString gklang ${LANG_GERMAN} "Sprachen"
LangString gkplg ${LANG_GERMAN} "Plugins"
LangString gkp_calc ${LANG_GERMAN} "Ausdruckrechner"
LangString gkp_calendar ${LANG_GERMAN} "Kalender"
LangString gkp_nb ${LANG_GERMAN} "Buch von den Namen"
LangString gkp_timeline ${LANG_GERMAN} "Zeitleiste"
LangString gkp_flowinput ${LANG_GERMAN} "Strömungseingang"
LangString gkp_pi ${LANG_GERMAN} "Importeur von den Stammbäumen"
LangString gkp_ts ${LANG_GERMAN} "Textsuche"
LangString gkp_cl ${LANG_GERMAN} "Conway Spiel des Lebens"
LangString gkp_chron ${LANG_GERMAN} "Chronik"
LangString gkp_cloud ${LANG_GERMAN} "Wörter Wolke"
LangString gkp_histdata ${LANG_GERMAN} "Verlaufsdaten (Links)"
LangString gkp_stdreports ${LANG_GERMAN} "Standardberichte"
LangString gkp_navig ${LANG_GERMAN} "Navigator"
LangString gk_shellopen ${LANG_GERMAN} "Datei öffnen"

!insertmacro MUI_LANGUAGE "SimpChinese"
LangString gkreq ${LANG_SIMPCHINESE} "GEDKeeper2 (必装)"
LangString gkscr ${LANG_SIMPCHINESE} "脚本样例"
LangString gkreg ${LANG_SIMPCHINESE} "系统注册"
LangString gklang ${LANG_SIMPCHINESE} "语言"
LangString gkplg ${LANG_SIMPCHINESE} "插件"
LangString gkp_calc ${LANG_SIMPCHINESE} "公式计算器"
LangString gkp_calendar ${LANG_SIMPCHINESE} "日历"
LangString gkp_nb ${LANG_SIMPCHINESE} "花名册"
LangString gkp_timeline ${LANG_SIMPCHINESE} "时间轴"
LangString gkp_flowinput ${LANG_SIMPCHINESE} "流输入"
LangString gkp_pi ${LANG_SIMPCHINESE} "家谱导入器"
LangString gkp_ts ${LANG_SIMPCHINESE} "文本检索"
LangString gkp_cl ${LANG_SIMPCHINESE} "康威生命游戏"
LangString gkp_chron ${LANG_SIMPCHINESE} "编年史"
LangString gkp_cloud ${LANG_SIMPCHINESE} "词云"
LangString gkp_histdata ${LANG_SIMPCHINESE} "历史数据（链接）"
LangString gkp_stdreports ${LANG_SIMPCHINESE} "标准报告"
LangString gkp_navig ${LANG_SIMPCHINESE} "航海家"
LangString gk_shellopen ${LANG_SIMPCHINESE} "打开文件"

!insertmacro MUI_LANGUAGE "Czech"
LangString gkreq ${LANG_CZECH} "GEDKeeper2 (požadováno)"
LangString gkscr ${LANG_CZECH} "Vzorky skriptů"
LangString gkreg ${LANG_CZECH} "Registrace systému"
LangString gklang ${LANG_CZECH} "Jazyky"
LangString gkplg ${LANG_CZECH} "Pluginy"
LangString gkp_calc ${LANG_CZECH} "Expresní kalkulačka"
LangString gkp_calendar ${LANG_CZECH} "Kalendář"
LangString gkp_nb ${LANG_CZECH} "Kniha jmen"
LangString gkp_timeline ${LANG_CZECH} "Časový řádek"
LangString gkp_flowinput ${LANG_CZECH} "Průtokový vstup"
LangString gkp_pi ${LANG_CZECH} "Dovozce rodokmenů"
LangString gkp_ts ${LANG_CZECH} "Hledání textu"
LangString gkp_cl ${LANG_CZECH} "Conway Life"
LangString gkp_chron ${LANG_CZECH} "Kronika"
LangString gkp_cloud ${LANG_CZECH} "Slova Cloud"
LangString gkp_histdata ${LANG_CZECH} "Historie dat (odkazů)"
LangString gkp_stdreports ${LANG_CZECH} "Standardní přehledy"
LangString gkp_navig ${LANG_CZECH} "Navigátor"
LangString gk_shellopen ${LANG_CZECH} "Otevřete"

!insertmacro MUI_LANGUAGE "Spanish"
LangString gkreq ${LANG_SPANISH} "GEDKeeper2 (obligatorio)"
LangString gkscr ${LANG_SPANISH} "Ejemplos de guiones"
LangString gkreg ${LANG_SPANISH} "Registro del sistema"
LangString gklang ${LANG_SPANISH} "Idiomas"
LangString gkplg ${LANG_SPANISH} "Complementos"
LangString gkp_calc ${LANG_SPANISH} "Calculadora de expresiones"
LangString gkp_calendar ${LANG_SPANISH} "Calendario"
LangString gkp_nb ${LANG_SPANISH} "Libro de nombres"
LangString gkp_timeline ${LANG_SPANISH} "Línea de tiempo"
LangString gkp_flowinput ${LANG_SPANISH} "Entrada de flujo"
LangString gkp_pi ${LANG_SPANISH} "Importador de pedigríes"
LangString gkp_ts ${LANG_SPANISH} "Búsqueda de texto"
LangString gkp_cl ${LANG_SPANISH} "Vida Conway"
LangString gkp_chron ${LANG_SPANISH} "Crónica"
LangString gkp_cloud ${LANG_SPANISH} "Nube de palabras"
LangString gkp_histdata ${LANG_SPANISH} "Datos históricos (enlaces)"
LangString gkp_stdreports ${LANG_SPANISH} "Informes estándar"
LangString gkp_navig ${LANG_SPANISH} "Navegador"
LangString gk_shellopen ${LANG_SPANISH} "Abrir"


; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\GEDKeeper2" "Install_Dir"


; Pages
Page components
Page directory
Page instfiles


function .onInit
    !insertmacro MUI_LANGDLL_DISPLAY
functionEnd

UninstPage uninstConfirm
UninstPage instfiles

Section "$(gkreq)"
    SectionIn RO

    SetOutPath $INSTDIR

    !insertmacro CheckNetFramework 471

    CreateDirectory "$INSTDIR\bin"
    SetOutPath "$INSTDIR\bin"

    File "..\bin\GEDKeeper2.exe"
    File "..\bin\GEDKeeper2.exe.config"
    File "..\bin\GKComponents.dll"
    File "..\bin\GKCore.dll"
    File "..\bin\GKTray.exe"

    File "..\bin\BSLib.dll"
    File "..\bin\BSLib.DataViz.Model.dll"
    File "..\bin\BSLib.DataViz.WFControls.dll"
    File "..\bin\BSLib.SQLite.dll"
    File "..\bin\RtfWriter.dll"
    File "..\bin\ExcelLibrary.dll"
    File "..\bin\ExifLib.dll"
    File "..\bin\GKMap.Core.dll"
    File "..\bin\GKMap.WinForms.dll"
    File "..\bin\itextsharp.dll"
    File "..\bin\NLua.dll"
    File "..\bin\KopiLua.dll"
    File "..\bin\NLog.dll"
    File "..\bin\nVLC.dll"
    File "..\bin\UtfUnknown.dll"
    File "..\bin\YamlDotNet.dll"
    File "..\bin\Newtonsoft.Json.dll"
    File "..\bin\ZedGraph.dll"

    File "..\LICENSE"

    CreateDirectory "$INSTDIR\locales"
    SetOutPath "$INSTDIR\locales"

    CreateDirectory "$INSTDIR\locales\cultures"
    SetOutPath "$INSTDIR\locales\cultures"
    File "..\locales\cultures\*.*"

    CreateDirectory "$INSTDIR\plugins"
    SetOutPath "$INSTDIR\plugins"

    CreateDirectory "$INSTDIR\samples"
    SetOutPath "$INSTDIR\samples"
    File "..\samples\*.*"

    CreateDirectory "$INSTDIR\themes"
    SetOutPath "$INSTDIR\themes"
    File "..\themes\*.*"

    CreateDirectory "$INSTDIR\externals"
    SetOutPath "$INSTDIR\externals"
    File "..\externals\resources.yaml"

    CreateDirectory "$SMPROGRAMS\GEDKeeper2"
    SetOutPath "$INSTDIR\bin"
    CreateShortCut "$SMPROGRAMS\GEDKeeper2\GKTray.lnk" "$INSTDIR\bin\GKTray.exe" "" "$INSTDIR\bin\GKTray.exe" 0
    CreateShortCut "$SMPROGRAMS\GEDKeeper2\GEDKeeper2.lnk" "$INSTDIR\bin\GEDKeeper2.exe" "" "$INSTDIR\bin\GEDKeeper2.exe" 0
    CreateShortCut "$DESKTOP\GEDKeeper2.lnk" "$INSTDIR\bin\GEDKeeper2.exe" "" "$INSTDIR\bin\GEDKeeper2.exe" 0

    CreateShortCut "$SMPROGRAMS\GEDKeeper2\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0

    ; Write the installation path into the registry
    WriteRegStr HKLM SOFTWARE\GEDKeeper2 "Install_Dir" "$INSTDIR"

    ; Write the uninstall keys for Windows
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\GEDKeeper2" "DisplayName" "GEDKeeper2"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\GEDKeeper2" "Publisher" "Sergey V. Zhdanovskih"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\GEDKeeper2" "UninstallString" '"$INSTDIR\uninstall.exe"'
    WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\GEDKeeper2" "NoModify" 1
    WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\GEDKeeper2" "NoRepair" 1
    WriteUninstaller "uninstall.exe"
SectionEnd

Section "$(gkscr)"
    CreateDirectory "$INSTDIR\scripts"
    SetOutPath "$INSTDIR\scripts"

    File "..\scripts\*.lua"
SectionEnd

Section "$(gkreg)"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\App Paths\GEDKeeper2.exe" "" "$INSTDIR\bin\GEDKeeper2.exe"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\App Paths\GEDKeeper2.exe" "Path" "$INSTDIR\bin"

    WriteRegStr HKCR ".ged" "" "GEDCOM.File"
    WriteRegStr HKCR "GEDCOM.File" "" "GEDCOM File"
    WriteRegStr HKCR "GEDCOM.File\DefaultIcon" "" "$INSTDIR\bin\GEDKeeper2.exe,0"
    WriteRegStr HKCR "GEDCOM.File\shell" "" "open"
    WriteRegStr HKCR "GEDCOM.File\shell\open" "" "$(gk_shellopen)"
    WriteRegStr HKCR "GEDCOM.File\shell\open\command" "" '$INSTDIR\bin\GEDKeeper2.exe "%1"'
SectionEnd

SectionGroup /e "$(gklang)"
    Section "English"
        SectionIn RO

        SetOutPath "$INSTDIR\locales"
        File "..\locales\english.lng"
        File "..\locales\readme_eng.html"

        CreateDirectory "$INSTDIR\locales\help_enu"
        SetOutPath "$INSTDIR\locales\help_enu"
        File "..\locales\help_enu\*.*"

        CreateDirectory "$INSTDIR\locales\help_enu\images"
        SetOutPath "$INSTDIR\locales\help_enu\images"
        File "..\locales\help_enu\images\*.*"

        CreateShortCut "$SMPROGRAMS\GEDKeeper2\Manual (English).lnk" "$INSTDIR\locales\help_enu\GEDKeeper.html" "" "$INSTDIR\locales\help_enu\GEDKeeper.html" 0
        CreateShortCut "$SMPROGRAMS\GEDKeeper2\Bach family (sample).lnk" "$INSTDIR\samples\Bach_Family.ged" "" "$INSTDIR\samples\Bach_Family.ged" 0
        CreateShortCut "$SMPROGRAMS\GEDKeeper2\Nehru-Ghandi family (sample).lnk" "$INSTDIR\samples\Nehru-Ghandi_Family.ged" "" "$INSTDIR\samples\Nehru-Ghandi_Family.ged" 0
        CreateShortCut "$SMPROGRAMS\GEDKeeper2\Human Mitochondria DNA Haplogroups (sample).lnk" "$INSTDIR\samples\Human_Mitochondria_DNA_Haplogroups.ged" "" "$INSTDIR\samples\Human_Mitochondria_DNA_Haplogroups.ged" 0
        CreateShortCut "$SMPROGRAMS\GEDKeeper2\Human Y-chromosome DNA Haplogroups (sample).lnk" "$INSTDIR\samples\Human_Y-chromosome_DNA_Haplogroups.ged" "" "$INSTDIR\samples\Human_Y-chromosome_DNA_Haplogroups.ged" 0
    SectionEnd

    Section "Русский"
        SetOutPath "$INSTDIR\locales"
        File "..\locales\russian.lng"
        File "..\locales\readme_rus.html"

        CreateDirectory "$INSTDIR\locales\help_rus"
        SetOutPath "$INSTDIR\locales\help_rus"
        File "..\locales\help_rus\*.*"

        CreateDirectory "$INSTDIR\locales\help_rus\images"
        SetOutPath "$INSTDIR\locales\help_rus\images"
        File "..\locales\help_rus\images\*.*"

        CreateShortCut "$SMPROGRAMS\GEDKeeper2\Справка (русский).lnk" "$INSTDIR\locales\help_rus\GEDKeeper.html" "" "$INSTDIR\locales\help_rus\GEDKeeper.html" 0
        CreateShortCut "$SMPROGRAMS\GEDKeeper2\Род Пушкиных (пример).lnk" "$INSTDIR\samples\Sample_Russia.ged" "" "$INSTDIR\samples\Sample_Russia.ged" 0
        CreateShortCut "$SMPROGRAMS\GEDKeeper2\Происхождение Человечества (пример).lnk" "$INSTDIR\samples\Human_Origins.ged" "" "$INSTDIR\samples\Human_Origins.ged" 0
        CreateShortCut "$SMPROGRAMS\GEDKeeper2\Древние царства (пример).lnk" "$INSTDIR\samples\Ancient_Kingdoms.ged" "" "$INSTDIR\samples\Ancient_Kingdoms.ged" 0
    SectionEnd

    Section "Українська"
        SetOutPath "$INSTDIR\locales"
        File "..\locales\ukrainian.lng"

        CreateShortCut "$SMPROGRAMS\GEDKeeper2\Шевченко Тарас Григорович (приклад).lnk" "$INSTDIR\samples\Taras_Shevchenko.ged" "" "$INSTDIR\samples\Taras_Shevchenko.ged" 0
    SectionEnd

    Section "Polski"
        SetOutPath "$INSTDIR\locales"
        File "..\locales\polish.lng"
    SectionEnd

    Section "Français"
        SetOutPath "$INSTDIR\locales"
        File "..\locales\french.lng"
    SectionEnd

    Section "Italiano"
        SetOutPath "$INSTDIR\locales"
        File "..\locales\italian.lng"
    SectionEnd

    Section "Deutsch"
        SetOutPath "$INSTDIR\locales"
        File "..\locales\german.lng"
    SectionEnd

    Section "中文"
        SetOutPath "$INSTDIR\locales"
        File "..\locales\Chinese Simplified.lng"
    SectionEnd

    Section "Čeština"
        SetOutPath "$INSTDIR\locales"
        File "..\locales\czech.lng"
    SectionEnd
SectionGroupEnd

SectionGroup /e "$(gkplg)"
    Section "$(gkp_calc)"
        SetOutPath "$INSTDIR\plugins"
        File "..\plugins\GKCalculatorPlugin.dll"
        File "..\plugins\GKCalculatorPlugin.rus"
        File "..\plugins\GKCalculatorPlugin.enu"
        File "..\plugins\GKCalculatorPlugin.ukr"
        File "..\plugins\GKCalculatorPlugin.pol"
    SectionEnd

    Section "$(gkp_calendar)"
        SetOutPath "$INSTDIR\plugins"
        File "..\plugins\GKCalendarPlugin.dll"
        File "..\plugins\GKCalendarPlugin.rus"
        File "..\plugins\GKCalendarPlugin.enu"
        File "..\plugins\GKCalendarPlugin.ukr"
        File "..\plugins\GKCalendarPlugin.pol"
    SectionEnd

    Section "$(gkp_nb)"
        SetOutPath "$INSTDIR\plugins"
        File "..\plugins\GKNamesBookPlugin.dll"
        File "..\plugins\GKNamesBookPlugin.rus"
        File "..\plugins\GKNamesBookPlugin.enu"
        File "..\plugins\GKNamesBookPlugin.ukr"
        File "..\plugins\GKNamesBookPlugin.pol"
    SectionEnd

    Section "$(gkp_timeline)"
        SetOutPath "$INSTDIR\plugins"
        File "..\plugins\GKTimeLinePlugin.dll"
        File "..\plugins\GKTimeLinePlugin.rus"
        File "..\plugins\GKTimeLinePlugin.enu"
        File "..\plugins\GKTimeLinePlugin.ukr"
        File "..\plugins\GKTimeLinePlugin.pol"
    SectionEnd

    Section "$(gkp_flowinput)"
        SetOutPath "$INSTDIR\plugins"
        File "..\plugins\GKFlowInputPlugin.dll"
        File "..\plugins\GKFlowInputPlugin.rus"
        File "..\plugins\GKFlowInputPlugin.enu"
        File "..\plugins\GKFlowInputPlugin.ukr"
        File "..\plugins\GKFlowInputPlugin.pol"
    SectionEnd

    Section "$(gkp_pi)"
        SetOutPath "$INSTDIR\plugins"
        File "..\plugins\GKPedigreeImporterPlugin.dll"
        File "..\plugins\GKPedigreeImporterPlugin.rus"
        File "..\plugins\GKPedigreeImporterPlugin.enu"
        File "..\plugins\GKPedigreeImporterPlugin.ukr"
        File "..\plugins\GKPedigreeImporterPlugin.pol"
    SectionEnd

    Section "$(gkp_ts)"
        SetOutPath "$INSTDIR\plugins"
        File "..\plugins\Lucene.Net.dll"
        File "..\plugins\ICSharpCode.SharpZipLib.dll"
        File "..\plugins\GKTextSearchPlugin.dll"
        File "..\plugins\GKTextSearchPlugin.rus"
        File "..\plugins\GKTextSearchPlugin.enu"
        File "..\plugins\GKTextSearchPlugin.ukr"
        File "..\plugins\GKTextSearchPlugin.pol"
    SectionEnd

    Section "$(gkp_cl)"
        SetOutPath "$INSTDIR\plugins"
        File "..\plugins\GKLifePlugin.dll"
        File "..\plugins\GKLifePlugin.rus"
        File "..\plugins\GKLifePlugin.enu"
        File "..\plugins\GKLifePlugin.ukr"
        File "..\plugins\GKLifePlugin.pol"
    SectionEnd

    Section "$(gkp_chron)"
        SetOutPath "$INSTDIR\plugins"
        File "..\plugins\GKChroniclePlugin.dll"
        File "..\plugins\GKChroniclePlugin.rus"
        File "..\plugins\GKChroniclePlugin.enu"
        File "..\plugins\GKChroniclePlugin.ukr"
        File "..\plugins\GKChroniclePlugin.pol"
    SectionEnd

    Section "$(gkp_cloud)"
        SetOutPath "$INSTDIR\plugins"
        File "..\plugins\GKWordsCloudPlugin.dll"
        File "..\plugins\GKWordsCloudPlugin.rus"
        File "..\plugins\GKWordsCloudPlugin.enu"
        File "..\plugins\GKWordsCloudPlugin.ukr"
        File "..\plugins\GKWordsCloudPlugin.pol"
    SectionEnd

    Section "$(gkp_histdata)"
        SetOutPath "$INSTDIR\plugins"
        File "..\plugins\GKHistoryDataPlugin.dll"
        File "..\plugins\GKHistoryDataPlugin.rus"
        File "..\plugins\GKHistoryDataPlugin.enu"
    SectionEnd

    Section "$(gkp_stdreports)"
        SetOutPath "$INSTDIR\plugins"
        File "..\plugins\GKStdReports.dll"
        File "..\plugins\GKStdReports.rus"
        File "..\plugins\GKStdReports.enu"
        File "..\plugins\GKStdReports.fra"
        File "..\plugins\GKStdReports.deu"
        File "..\plugins\GKStdReports.ukr"
    SectionEnd

    Section "GEDmill"
        SetOutPath "$INSTDIR\plugins"
        File "..\plugins\GEDmillPlugin.dll"
        File "..\plugins\GEDmillPlugin.rus"
        File "..\plugins\GEDmillPlugin.enu"
        File "..\plugins\log4net.dll"
    SectionEnd

    Section "$(gkp_navig)"
        SetOutPath "$INSTDIR\plugins"
        File "..\plugins\GKNavigatorPlugin.dll"
        File "..\plugins\GKNavigatorPlugin.rus"
        File "..\plugins\GKNavigatorPlugin.enu"
        File "..\plugins\GKNavigatorPlugin.ukr"
    SectionEnd
SectionGroupEnd

Section "Uninstall"
    ; Remove registry keys
    DeleteRegKey HKCR ".ged"
    DeleteRegKey HKCR "GEDCOM.File"

    DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\App Paths\GEDKeeper2.exe"
    DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\GEDKeeper2"
    DeleteRegKey HKLM "SOFTWARE\GEDKeeper2"

    ; Remove files and uninstaller
    Delete $INSTDIR\LICENSE

    Delete $INSTDIR\uninstall.exe

    Delete "$INSTDIR\bin\*.*"
    RMDir "$INSTDIR\bin"

    Delete "$INSTDIR\locales\help_rus\images\*.*"
    RMDir "$INSTDIR\locales\help_rus\images"

    Delete "$INSTDIR\locales\help_rus\*.*"
    RMDir "$INSTDIR\locales\help_rus"

    Delete "$INSTDIR\locales\help_enu\images\*.*"
    RMDir "$INSTDIR\locales\help_enu\images"

    Delete "$INSTDIR\locales\help_enu\*.*"
    RMDir "$INSTDIR\locales\help_enu"

    Delete "$INSTDIR\locales\cultures\*.*"
    RMDir "$INSTDIR\locales\cultures"

    Delete "$INSTDIR\locales\*.*"
    RMDir "$INSTDIR\locales"

    Delete "$INSTDIR\externals\*.*"
    RMDir "$INSTDIR\externals"

    Delete "$INSTDIR\scripts\*.lua"
    RMDir "$INSTDIR\scripts"

    Delete "$INSTDIR\plugins\*.*"
    RMDir "$INSTDIR\plugins"

    Delete "$INSTDIR\samples\*.*"
    RMDir "$INSTDIR\samples"

    Delete "$INSTDIR\themes\*.*"
    RMDir "$INSTDIR\themes"

    ; Remove shortcuts, if any
    Delete "$SMPROGRAMS\GEDKeeper2\*.*"
    Delete "$DESKTOP\GEDKeeper2.lnk"

    ; Remove directories used
    RMDir "$SMPROGRAMS\GEDKeeper2"
    RMDir "$INSTDIR"
SectionEnd
