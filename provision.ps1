
$group = 'securedfunc-demo-rg'
$location = 'NorwayEast'
$storage = 'securedfuncdemostorage'
$functionapp = 'securedfuncdemo'

az account set -s 5c45e8*
az group create -n $group --location $location
az storage account create -n $storage -g $group --location $location 
az functionapp create -s $storage -g  $group `
    -n $functionapp -c $location --functions-version 3

func azure functionapp publish $functionapp
