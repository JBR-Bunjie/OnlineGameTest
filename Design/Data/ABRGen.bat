set LUBANRESOURCESSPACE=..\..\..\GlobalToolsForUnityProject\Luban
set CONFIGSPACE=.
set WORKSPACE=..\..\Assets

@Rem PLEASE PAY ATTENTION TO YOUR WORKSPACES!

set LUBAN_DLL=%LUBANRESOURCESSPACE%\Tools\Luban\Luban.dll
set CONF_DIR=%CONFIGSPACE%

dotnet %LUBAN_DLL% ^
	-t client ^
	-c cs-simple-json ^
	-d json ^
	--conf %CONF_DIR%\ABRConf.conf ^
	-x outputCodeDir=%WORKSPACE%\HotFixAssembly\ProceduralCode ^
	-x outputDataDir=%WORKSPACE%\ABResources\ProceduralData

pause