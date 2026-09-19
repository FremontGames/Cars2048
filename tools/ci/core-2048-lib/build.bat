@echo off
set "SOURCE=.\packages\core-2048-lib\Core2048\Sources"

@REM Build - Core2048
set "DEST=.\build\core-2048-lib\Core"
robocopy "%SOURCE%" "%DEST%" /E >nul
echo Copy to "%DEST%"...
