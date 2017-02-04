@ECHO off
DEL /s /f /q *.nupkg
FOR /D %%p IN (".\packages\*.*") DO RMDIR "%%p" /s /q
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"
