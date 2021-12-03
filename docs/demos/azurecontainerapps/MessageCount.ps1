
Param(
    [parameter(Mandatory=$true)][string]$storageAccount,
    [parameter(Mandatory=$true)][string]$accesskey
)

function GetMessages($storageAccount, $accesskey){
    $method = "GET"
    $contenttype = "application/x-www-form-urlencoded"
    $version = "2017-04-17"
    $resource = "$QueueName/?comp=metadata"
    $queue_url = "https://$storageAccount.queue.core.windows.net/$resource"
    $GMTTime = (Get-Date).ToUniversalTime().toString('R')
    $canonheaders = "x-ms-date:$GMTTime`nx-ms-version:$version`n"
    $stringToSign = "$method`n`n$contenttype`n`n$canonheaders/$storageAccount/$resource"
    $hmacsha = New-Object System.Security.Cryptography.HMACSHA256
    $hmacsha.key = [Convert]::FromBase64String($accesskey)
    $signature = $hmacsha.ComputeHash([Text.Encoding]::UTF8.GetBytes($stringToSign))
    $signature = [Convert]::ToBase64String($signature)
    $headers = @{
        'x-ms-date' = $GMTTime
        Authorization = "SharedKeyLite " + $storageAccount + ":" + $signature
        "x-ms-version" = $version
        Accept = "text/xml"
    }
    $response = Invoke-WebRequest -Method $method -Uri $queue_url -Headers $headers -ContentType $contenttype
    return $response.Headers["x-ms-approximate-messages-count"]
}
 
$messageCount = GetMessages -storageAccount $storageAccount -accesskey $accesskey
Write-Output "Feed Queue Messages: $messageCount"