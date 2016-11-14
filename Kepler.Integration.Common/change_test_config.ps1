param (
	[string]$buildId = (throw "Teamcity Build Id isn not specified!"),
	[string]$branchName = (throw "Teamcity Build Id isn not specified!")
)

function ModifyUiTestConfig([string]$configPath,  [string]$hostName,  [string]$branchName)
{
	$xml = [xml](Get-Content $configPath)

	$xml.configuration.appSettings.add | foreach { if ($_.key -eq 'sravni') { $_.value = $hostName } }
	
	$xml.configuration.appSettings.add | foreach { if ($_.key -eq 'branchName') { $_.value = $branchName } }

	$xml.Save($configPath)
}


###################################

$url="http://srvm-dev1.altcms.local:8080/httpAuth/app/rest/latest/builds/$buildId/tags/"

Write-Output ("Teamcity REST URL: " + $url)

$user = "deploy"
$pass= "deploy"

$secpasswd = ConvertTo-SecureString $pass -AsPlainText -Force
$cred = New-Object System.Management.Automation.PSCredential ($user, $secpasswd)

$response = Invoke-RestMethod -Uri $url -Credential $cred -Method "Get"
$text = $response.tags.tag.name

$hostName = $text | % { if ($_ -match "deployed on (.+). Environment") { 
    #Return match from group 1
    $Matches[1] 
    }
}

Write-Output ("Host under test is '$hostName'")

###################################

$basePath = (Get-Item -Path ".\" -Verbose).FullName + "\"



$configPath = $basePath + "\bin\Release\Sravni.ScreenShotTest.dll.config"
ModifyUiTestConfig $configPath  $hostName $branchName