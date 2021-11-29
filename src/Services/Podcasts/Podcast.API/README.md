## DotNet Podcast API

![Services Build status](/../../actions/workflows/podcast-api.yml/badge.svg)

## Table of contents

- [Run Backend Services Locally](#run-backend-locally)
- [Deployment](#deployment)

# Run Backend Services Locally <a name="run-backend-locally"></a>

### Compose

The easiest way to run your backend services locally is using _Compose_. To run the services type `docker-compose up` from terminal located in the root folder. This will build (if needed) the Docker images and bring up all the containers.

### Visual Studio

Run `docker-compose up -d podcast.db storage` from terminal located in the root folder. 

Open Visual Studio select Podcast.Api as start up project and hit f5.

# Deployment <a name="deployment"></a>

## Deploy using GitHub Actions

To configure the Github Action it is necessary to create an environment called `prod` and set up the following secrets. (Consider that the action will create these resources automatically, so resource names need to be unique). [Learn how to create an environment.](https://docs.github.com/en/actions/deployment/targeting-different-environments/using-environments-for-deployment)

- `AZURE_CREDENTIALS` : service principal to authenticate with Azure. See [Create Azure Credentials](https://docs.microsoft.com/en-us/azure/developer/github/connect-from-azure?tabs=azure-portal%2Cwindows#create-a-service-principal-and-add-it-as-a-github-secret).
- `PODCASTDB_USER_LOGIN` : Username for Podcast db. 
- `PODCASTDB_USER_PASSWORD` : Password for Podcast db.
- `AZURE_RESOURCE_GROUP_NAME`: Existing resource group where resources will be deployed.
- `ACR_NAME`: Container registry name
- `STORAGE_NAME`: Storage name
- `PODCASTDB_SERVER_NAME`: Azure SQL server name
- `KUBERNETES_ENV_NAME`: Container apps environment name
- `WORKSPACE_NAME`: Log Analytics workspace name