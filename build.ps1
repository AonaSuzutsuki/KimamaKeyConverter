Param( $outName )

if ( [string]::IsNullOrEmpty($outName) ) {
    $outName = "bin"
}

$CurrentDirectory = Split-Path $MyInvocation.MyCommand.Path -Parent
$OutBinDirectory = "$CurrentDirectory\$outName"
$Framework = "net8.0-windows7.0"
$Profile = "Release"

$KeyConverter = "$CurrentDirectory\KeyConverter\bin\$Profile\$Framework"

#KeyConverter
xcopy /Y $KeyConverter\*.dll $OutBinDirectory\
xcopy /Y $KeyConverter\*.exe $OutBinDirectory\
xcopy /Y $KeyConverter\*.deps.json $OutBinDirectory\
xcopy /Y $KeyConverter\*.runtimeconfig.json $OutBinDirectory\
Copy-Item -Path $KeyConverter\en-US\ -Destination "$OutBinDirectory\" -Recurse -Force
