@echo off
echo #################Setting up the environment variables #####################

set path=C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319;%path%

set RESPONSE_FILE=%COMPUTERNAME%.rsp
if not exist %RESPONSE_FILE% set RESPONSE_FILE=local.rsp
