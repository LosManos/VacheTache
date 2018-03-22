Write-Host 'Packing the nuget package.'

Write-Host 'If you haven''t set to compile as Release, do that now.'
Read-Host 'Press enter to continue.';

pushd VacheTacheLibrary
.\..\nuget.exe pack -Prop Configuration=Release
popd

pushd VacheTacheLibrary.FileSystem
.\..\nuget.exe pack -Prop Configuration=Release
popd
