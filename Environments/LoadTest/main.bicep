// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

@description('Name of the load test service')
@minLength(5)
@maxLength(50)
param name string

@description('Location for all resources.')
param location string = resourceGroup().location

resource load_tests 'Microsoft.LoadTestService/loadTests@2022-12-01' = {
  name: name
  location: location
  identity: {
    type: 'SystemAssigned'
  }
}
