properties {
  $testMessage = 'Executed Test!'
  $compileMessage = 'Executed Compile!'
  $cleanMessage = 'Executed Clean!'
  $solutionName = 'Simple.Messaging.sln'
  $build_dir = Split-Path $psake.build_script_file	
  $nugetPackagesDirectory = Join-Path $build_dir "GeneratedPackages"
  $version = 0.0
}

task default -depends NugetPack

Task NugetPack -Depends Build {
    if((Test-Path $nugetPackagesDirectory) -eq $false){
        New-Item $nugetPackagesDirectory -type directory
    }
    gci  *.nuspec | 
        ForEach-Object {
            $expression = ".\.nuget\nuget.exe pack {0} -Build -OutputDirectory {1} -version {2} -Symbols" -f $_.Name, $nugetPackagesDirectory, $version
            Invoke-Expression $expression
        }
}
Task NugetDeploy -Depends NugetPack {
   
    gci $nugetPackagesDirectory  *.nupkg | 
        ForEach-Object {

            $expression = ".\.nuget\nuget.exe push {0} {1}" -f $_.FullName, $nugetApiKey
            Invoke-Expression $expression
        }
}
task Build -depends Clean { 
	Update-AssemblyInfo $version
	Exec { msbuild $solutionName /t:Build /p:Configuration=Release} 
}

task Clean { 
	Exec { msbuild $solutionName /t:Clean /p:Configuration=Release} 
}

function Update-AssemblyInfo($version)
{

}