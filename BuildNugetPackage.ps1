Write-Host 'Packing the nuget package in Release.'

$msbuildexe = 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\msbuild.exe'

function BuildNugetPackage( $name){
	pushd $name
	& $msbuildexe -t:pack -p:Configuration=Release
	Get-ChildItem bin/release
	popd
}

BuildNugetPackage VacheTacheLibrary

BuildNugetPackage 'VacheTacheLibrary.FileSystem'
