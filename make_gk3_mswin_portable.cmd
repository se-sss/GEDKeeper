@echo off

set APP_VER=3.1.0

call .\clean.cmd

dotnet build projects/GKv3/GEDKeeper3.sln -c MSWin_Release

set BUILD_STATUS=%ERRORLEVEL%
if %BUILD_STATUS%==0 goto installer
if not %BUILD_STATUS%==0 goto fail

:fail
pause 
exit /b %BUILD_STATUS% 

:installer
cd .\deploy
call gk_win_portable.cmd %APP_VER%
cd ..
pause
exit /b 0
