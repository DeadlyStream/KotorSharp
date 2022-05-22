@echo off

echo "Copying files from game directory %1. This may take awhile"

xcopy %1 "%~dp0..\KPatcherTests\Snapshots\Resources\gameRoot" /s /d /f

echo "Finished copying files to \KPatcherTests\Snapshots\Resources\gameRoot"
pause