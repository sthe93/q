param(
    [string]$configPath,
    [string]$QualificationVerificationAPI
)

Write-Host "Updating web.config at: $configPath"

[xml]$xml = Get-Content $configPath

# Create a hashtable of key-value pairs to update
$settingsToUpdate = @{
    "QualificationVerificationAPI"     = $QualificationVerificationAPI
    
   
    
}

foreach ($key in $settingsToUpdate.Keys) {
    $value = $settingsToUpdate[$key]
    $node = $xml.configuration.appSettings.add | Where-Object { $_.key -eq $key }
    
    if ($node) {
        $node.value = $value
        Write-Host "Updated '$key' to '$value'"
    } else {
        Write-Warning "Key '$key' not found in appSettings"
    }
}

# Save the updated config
$xml.Save($configPath)
Write-Host "web.config updated successfully."

