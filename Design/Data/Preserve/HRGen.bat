@REM set LUBANRESOURCESSPACE=..\..\..\GlobalToolsForUnityProject\Luban
@REM set CONFIGSPACE=.
@REM set WORKSPACE=..\..\Assets

@REM @Rem PLEASE PAY ATTENTION TO YOUR WORKSPACES!

@REM set LUBAN_DLL=%LUBANRESOURCESSPACE%\Tools\Luban\Luban.dll
@REM set CONF_DIR=%CONFIGSPACE%

@REM dotnet %LUBAN_DLL% ^
@REM 	-t client ^
@REM 	-c cs-simple-json ^
@REM 	-d json ^
@REM 	--conf %CONF_DIR%\HRConf.conf ^
@REM 	-x outputDataDir=%WORKSPACE%\Resources\HRProceduralData

@REM pause