@echo off

echo # CLEAN all...
RMDIR /S /Q ".\build\"

echo # CLEAN project-2017-cars-2048...
./tools/ci/core-2048-lib/clean.bat
