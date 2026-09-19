cd ./packages/core-2048-lib/

@REM Install - Core2048
cd ./Core2048
dotnet restore
dotnet build

cd ..

@REM Install - Core2048.Tests
cd ./Core2048.Tests
dotnet restore
dotnet build
