Write-Host 'Packing the nuget package in Release.'

pushd VacheTacheLibrary
.\..\nuget.exe pack -Prop Configuration=Release
popd

pushd VacheTacheLibrary.FileSystem
.\..\nuget.exe pack -Prop Configuration=Release
popd
