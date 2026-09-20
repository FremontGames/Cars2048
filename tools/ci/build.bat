@echo off

echo # BUILD project-2017-cars-2048...
@REM ./tools/ci/core-2048-lib/build.bat
./tools/ci/core-2048-lib/deploy.bat

echo # BUILD project-2017-cars-2048...
./tools/ci/project-2017-cars-2048/package.bat
