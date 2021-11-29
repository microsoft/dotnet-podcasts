#! /usr/bin/pwsh

Param(
    [parameter(Mandatory=$true)][string]$resourceGroup,
    [parameter(Mandatory=$true)][string]$storageName
)

$storage = $(az storage account show -n $storageName -g $resourceGroup -o json | ConvertFrom-Json)

if (-not $storage) {
    Write-Host "Storage $storageName not found in RG $resourceGroup" -ForegroundColor Red
    exit 1
}

$url = $storage.primaryEndpoints.blob
$constr = $(az storage account show-connection-string -n $storageName -g $resourceGroup -o json | ConvertFrom-Json).connectionString

$containerExists = $(az storage container exists --name "covers" --connection-string "$constr" | ConvertFrom-Json).exists

if (!$containerExists) {

    Write-Host "Connecting to storage and creating containers" -ForegroundColor Green
    az storage container create --name "covers" --public-access blob --connection-string "$constr" 
    Write-Host "Copying images..." -ForegroundColor Green

    $accountName=$storage.name

    az storage blob upload-batch --destination "$url" --destination covers --source ./Covers --account-name $accountName
}