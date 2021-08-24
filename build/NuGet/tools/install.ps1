param($installPath, $toolsPath, $package, $project)

Write-Host "installPath:" "${installPath}"
Write-Host "toolsPath:" "${toolsPath}"

Write-Host " "

if ($project) {
	$dateTime = Get-Date -Format yyyyMMdd-HHmmss

	# Create paths and list them
	$projectPath = (Get-Item $project.Properties.Item("FullPath").Value).FullName
	Write-Host "projectPath:" "${projectPath}"

	# Copy Vendr files from package to project folder	
	$srcAppPluginPath = Join-Path $installPath "App_Plugins"
	$targetAppPluginPath = Join-Path $projectPath "App_Plugins"

    Write-Host "copying files to $projectPath ..."
    # see https://support.microsoft.com/en-us/help/954404/return-codes-that-are-used-by-the-robocopy-utility-in-windows-server-2
    robocopy "$srcAppPluginPath " "$targetAppPluginPath " /is /it /e
    if (($lastexitcode -eq 1) -or ($lastexitcode -eq 3) -or ($lastexitcode -eq 5) -or ($lastexitcode -eq 7))
    {
        write-host "Copy succeeded!"
    }
    else
    {
        write-host "Copy failed with exit code:" $lastexitcode
    }
	
	# Open appropriate readme
	# if($copyWebconfig -eq $true)  
	# {
	# 	$DTE.ItemOperations.OpenFile($toolsPath + '\Readme.txt')
	# } 
	# else 
	# {	
	# 	$DTE.ItemOperations.OpenFile($toolsPath + '\ReadmeUpgrade.txt')
	# }
}