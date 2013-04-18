call setEnv.bat
msbuild build.proj @%RESPONSE_FILE% /t:Publish
if %ERRORLEVEL% NEQ 0 goto failed

@echo.
@echo Build *COMPLETED* successfully
goto end

:failed
@echo.
@echo Build *FAILED* with errors

:end