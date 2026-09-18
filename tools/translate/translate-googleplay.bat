# translate-googleplay.bat

cd "c:\Users\damien\git\translate-cli"

set MYFILE="c:\Users\damien\git\project-2048\googleplay\locale"
set MYVERSION=1.2.2

set MYLANG=fr
java -jar translate-cli-%MYVERSION%.jar --mode=html2text --input=%MYFILE%.txt --input-lang=en --output=%MYFILE%-%MYLANG%.txt --output-lang=%MYLANG%
set MYLANG=en
java -jar translate-cli-%MYVERSION%.jar --mode=html2text --input=%MYFILE%.txt --input-lang=en --output=%MYFILE%-%MYLANG%.txt --output-lang=%MYLANG%
set MYLANG=ja
java -jar translate-cli-%MYVERSION%.jar --mode=html2text --input=%MYFILE%.txt --input-lang=en --output=%MYFILE%-%MYLANG%.txt --output-lang=%MYLANG%
set MYLANG=ko
java -jar translate-cli-%MYVERSION%.jar --mode=html2text --input=%MYFILE%.txt --input-lang=en --output=%MYFILE%-%MYLANG%.txt --output-lang=%MYLANG%
set MYLANG=zh-TW
java -jar translate-cli-%MYVERSION%.jar --mode=html2text --input=%MYFILE%.txt --input-lang=en --output=%MYFILE%-%MYLANG%.txt --output-lang=%MYLANG%
set MYLANG=de
java -jar translate-cli-%MYVERSION%.jar --mode=html2text --input=%MYFILE%.txt --input-lang=en --output=%MYFILE%-%MYLANG%.txt --output-lang=%MYLANG%
set MYLANG=pt
java -jar translate-cli-%MYVERSION%.jar --mode=html2text --input=%MYFILE%.txt --input-lang=en --output=%MYFILE%-%MYLANG%.txt --output-lang=%MYLANG%
set MYLANG=es
java -jar translate-cli-%MYVERSION%.jar --mode=html2text --input=%MYFILE%.txt --input-lang=en --output=%MYFILE%-%MYLANG%.txt --output-lang=%MYLANG%
set MYLANG=it
java -jar translate-cli-%MYVERSION%.jar --mode=html2text --input=%MYFILE%.txt --input-lang=en --output=%MYFILE%-%MYLANG%.txt --output-lang=%MYLANG%
set MYLANG=ru
java -jar translate-cli-%MYVERSION%.jar --mode=html2text --input=%MYFILE%.txt --input-lang=en --output=%MYFILE%-%MYLANG%.txt --output-lang=%MYLANG%
set MYLANG=hi
java -jar translate-cli-%MYVERSION%.jar --mode=html2text --input=%MYFILE%.txt --input-lang=en --output=%MYFILE%-%MYLANG%.txt --output-lang=%MYLANG%