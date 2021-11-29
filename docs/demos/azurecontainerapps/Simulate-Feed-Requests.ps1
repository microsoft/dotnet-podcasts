Param(
    [parameter(Mandatory=$false)][string]$baseUrl = ""
)

class FeedRequest 
{
    [string] $Title
    [string] $Url
    [System.Collections.Generic.List[String]] $Categories
}

function SimulateRequest {
    Param(
     [int]$sleep = 100,
     [string]$baseUrl
    )
    
    $feeds = [System.Collections.Generic.List[FeedRequest]](Get-Content './feeds.json' | Out-String | ConvertFrom-Json)
    
    $feeds | ForEach-Object -ThrottleLimit 20 -Parallel {
        Write-Host "> Requesting feed" $_.Title 

        Invoke-WebRequest -Method POST -Uri $using:baseUrl"v1/feeds" `
                        -Body ($_|ConvertTo-Json) `
                        -ContentType application/json

        [System.Threading.Thread]::Sleep($sleep)
    } 
}

SimulateRequest -baseUrl $baseUrl
