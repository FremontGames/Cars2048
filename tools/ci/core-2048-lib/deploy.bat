@echo off
set "SOURCE=.\packages\core-2048-lib\Core2048\Sources"

@REM Deploy - Core2048
set "DEST=.\apps\project-2017-cars-2048\Assets\Project 2048\Scripts\Core"
robocopy "%SOURCE%" "%DEST%" /E >nul
echo Deploy to "%DEST%"...
