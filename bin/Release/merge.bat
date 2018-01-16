@echo off

if "%programfiles(x86)%XXX"=="XXX" goto 32BIT
    :: 64-bit
    set PROGS=%programfiles(x86)%
    goto CONT
:32BIT
    set PROGS=%ProgramFiles%
:CONT

md tmp
ilmerge /out:tmp\TraktRater.exe TraktRater.exe CsvHelper.dll /target:dll /targetplatform:"v4,%PROGS%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0" /wildcards
IF EXIST TraktRater_UNMERGED.exe del TraktRater_UNMERGED.exe
ren TraktRater.exe TraktRater_UNMERGED.exe
IF EXIST TraktRater_UNMERGED.pdb del TraktRater_UNMERGED.pdb
ren TraktRater.pdb TraktRater_UNMERGED.pdb

move tmp\*.* .
rd tmp

