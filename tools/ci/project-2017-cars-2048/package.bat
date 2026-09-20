@ECHO OFF

SET "SOURCE=.\apps\project-2017-cars-2048"
set "DEST=.\build\project-2017-cars-2048-sources"

ECHO # PACKAGE CLEAN project-2017-cars-2048...
set "ZIP=%DEST%.zip"
RMDIR /S /Q "%DEST%"
DEL "%ZIP%"
mkdir "%DEST%"

ECHO # PACKAGE APP project-2017-cars-2048...

ECHO # PACKAGE SOURCE project-2017-cars-2048...
robocopy "%SOURCE%\Assets" "%DEST%\Assets" /E >nul
robocopy "%SOURCE%\ProjectSettings" "%DEST%\ProjectSettings" /E >nul
robocopy "%SOURCE%\UserSettings" "%DEST%\UserSettings" /E >nul
copy "%SOURCE%\Project 2048 Cars.sln" "%DEST%\Project 2048 Cars.sln"
tar -a -c -f  "%ZIP%" "%DEST%"
ECHO Archive created: %ZIP%
