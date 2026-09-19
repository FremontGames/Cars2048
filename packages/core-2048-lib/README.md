CORE 2048 LIB
================

## Content

- [Install](#install)
- [Usage](#usage)
- [Publish](#publish)

---------------------------------------

## Install

```powershell
.\tools\ci\core-2048-lib\clean.bat
# OUTPUT: Remove-Item : ...
# OUTPUT: Build succeeded in 0.5s

.\tools\ci\core-2048-lib\install.bat
# OUTPUT: Build succeeded in 1.3s
```


## Usage

```powershell
.\tools\ci\core-2048-lib\test.bat
# OUTPUT: Test summary: total: 21, failed: 0, ...
# OUTPUT: Build succeeded in 1.6s
```

```powershell
.\tools\ci\core-2048-lib\build.bat
# OUTPUT: Copy to ...
```

## Deploy

```powershell
.\tools\ci\core-2048-lib\deploy.bat
# OUTPUT: Copy to ...
```
