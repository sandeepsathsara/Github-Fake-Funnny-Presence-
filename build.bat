@echo off
cd /d "%~dp0"

echo Building Porn Hub.exe...
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe /target:winexe /optimize+ /platform:x64 /win32icon:"%~dp0app.ico" /out:"%~dp0Porn Hub.exe" "%~dp0Program.cs"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo [SUCCESS] "Porn Hub.exe" built successfully!
) else (
    echo.
    echo [ERROR] Build failed.
)

pause
