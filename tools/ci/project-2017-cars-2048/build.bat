@ECHO OFF

SET "SOURCE=.\apps\project-2017-cars-2048"
SET "EXE=C:\Program Files\Unity\Hub\Editor\6000.3.23f1\Editor\Unity.exe"
SET "LOG=.\build\project-2017-cars-2048-BUILD.log"

ECHO # BUILD project-2017-cars-2048...
"%EXE%" -executeMethod BuildScripts.BuildWindows64 -buildTarget StandaloneWindows64 -batchmode -quit -projectPath "%SOURCE%" -logFile "%LOG%"
