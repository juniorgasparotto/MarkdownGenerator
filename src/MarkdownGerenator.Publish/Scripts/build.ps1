Add-Type -assembly "system.io.compression.filesystem"

function buildVS
{
    param
    (
        [parameter(Mandatory=$true)]
        [String] $path,

        [parameter(Mandatory=$false)]
        [bool] $nuget = $true,
        
        [parameter(Mandatory=$false)]
        [bool] $clean = $true
    )
    process
    {
        $msBuildExe = 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe'

        if ($nuget) {
            Write-Host "Restoring NuGet packages" -foregroundcolor green
            nuget restore "$($path)"
        }

        if ($clean) {
            Write-Host "Cleaning $($path)" -foregroundcolor green
            & "$($msBuildExe)" "$($path)" /t:Clean /m
        }

        Write-Host "Building $($path)" -foregroundcolor green
        & "$($msBuildExe)" "$($path)" /t:Build /m /property:Configuration=Release /p:OutDir="$WorkingDir"
    }
}

function clearOutputs
{
    param
    (
    )
    process
    {
        if (Test-Path $DestinationZip)
        {
            Remove-Item $DestinationZip -Confirm:$false
        }

        if (Test-Path $WorkingDir)
        {
            Remove-Item $WorkingDir -recurse -Confirm:$false
        }
    }
}

##### Vars

$WorkingDir = Convert-Path .
$WorkingDir = "$WorkingDir/Output"
$DestinationZip = Convert-Path .
$DestinationZip = "$DestinationZip/WiremockUI.zip"
$PublishDir = Convert-Path . | split-path -parent

##### Vars

clearOutputs
buildVS "..\..\WiremockUI\WiremockUI.csproj" $false $true
Remove-Item "$WorkingDir/WiremockUI.application" -Confirm:$false
Remove-Item "$WorkingDir/WiremockUI.exe.config" -Confirm:$false
Remove-Item "$WorkingDir/WiremockUI.exe.manifest" -Confirm:$false
Remove-Item "$WorkingDir/WiremockUI.pdb" -Confirm:$false
Copy-Item "$PublishDir/.app/db.json" "$WorkingDir/.app" -force
[io.compression.zipfile]::CreateFromDirectory($WorkingDir, $DestinationZip)
# clearOutputs