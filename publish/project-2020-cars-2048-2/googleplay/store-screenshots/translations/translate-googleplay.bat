# translate-googleplay.bat

cd "c:\Users\damien\git\translate-cli"

set MYFILE="c:\Users\damien\git\project-2048-2\googleplay\store-screenshots\translations\groups-to-screenshot"
set MYVERSION=1.3.0

set MYLANG=fr
java -jar translate-cli-%MYVERSION%.jar --mode=json --input=%MYFILE%-en.json --input-lang=en --output=%MYFILE%-%MYLANG%.json --output-lang=%MYLANG%
set MYLANG=ja
java -jar translate-cli-%MYVERSION%.jar --mode=json --input=%MYFILE%-en.json --input-lang=en --output=%MYFILE%-%MYLANG%.json --output-lang=%MYLANG%
set MYLANG=ko
java -jar translate-cli-%MYVERSION%.jar --mode=json --input=%MYFILE%-en.json --input-lang=en --output=%MYFILE%-%MYLANG%.json --output-lang=%MYLANG%
set MYLANG=zh-TW
java -jar translate-cli-%MYVERSION%.jar --mode=json --input=%MYFILE%-en.json --input-lang=en --output=%MYFILE%-%MYLANG%.json --output-lang=%MYLANG%
set MYLANG=de
java -jar translate-cli-%MYVERSION%.jar --mode=json --input=%MYFILE%-en.json --input-lang=en --output=%MYFILE%-%MYLANG%.json --output-lang=%MYLANG%
set MYLANG=pt
java -jar translate-cli-%MYVERSION%.jar --mode=json --input=%MYFILE%-en.json --input-lang=en --output=%MYFILE%-%MYLANG%.json --output-lang=%MYLANG%
set MYLANG=es
java -jar translate-cli-%MYVERSION%.jar --mode=json --input=%MYFILE%-en.json --input-lang=en --output=%MYFILE%-%MYLANG%.json --output-lang=%MYLANG%
set MYLANG=it
java -jar translate-cli-%MYVERSION%.jar --mode=json --input=%MYFILE%-en.json --input-lang=en --output=%MYFILE%-%MYLANG%.json --output-lang=%MYLANG%
set MYLANG=ru
java -jar translate-cli-%MYVERSION%.jar --mode=json --input=%MYFILE%-en.json --input-lang=en --output=%MYFILE%-%MYLANG%.json --output-lang=%MYLANG%
set MYLANG=hi
java -jar translate-cli-%MYVERSION%.jar --mode=json --input=%MYFILE%-en.json --input-lang=en --output=%MYFILE%-%MYLANG%.json --output-lang=%MYLANG%
