@echo off

echo "Copying files from game directory %1. This may take awhile"

xcopy %1\data\ "%~dp0..\KPatcherTests\Snapshots\Resources\gameRoot\data\" /s /d /f /i
xcopy %1\lips\ "%~dp0..\KPatcherTests\Snapshots\Resources\gameRoot\lips\" /s /d /f /i
xcopy %1\Modules\ "%~dp0..\KPatcherTests\Snapshots\Resources\gameRoot\modules\" /s /d /f /i
xcopy %1\rims\ "%~dp0..\KPatcherTests\Snapshots\Resources\gameRoot\rims\" /s /d /f /i
xcopy %1\dialog.tlk "%~dp0..\KPatcherTests\Snapshots\Resources\gameRoot\" /s /d /f /i
xcopy %1\chitin.key "%~dp0..\KPatcherTests\Snapshots\Resources\gameRoot\" /s /d /f /i

echo "Finished copying files to \KPatcherTests\Snapshots\Resources\gameRoot"
pause