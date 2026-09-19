cd ./packages/core-2048-lib/

@REM Clean - Core2048
cd ./Core2048/
dotnet clean

cd ..

@REM Clean - Core2048.Tests
cd ./Core2048.Tests/
dotnet clean
Remove-Item -Recurse -Force bin,obj
