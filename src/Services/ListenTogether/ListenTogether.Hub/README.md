## Listen together

![Services Build status](/../../actions/workflows/podcast-hub.yml/badge.svg)

## Table of contents

- [Run locally](#run-backend-locally)
- [Deployment](#deployment)

# Run locally <a name="run-backend-locally"></a>

### Compose

The easiest way to run your backend services locally is using _Compose_. To run the services type `docker-compose up` from terminal located in the root folder. This will build (if needed) the Docker images and bring up all the containers.

### Visual Studio

Run `docker-compose up -d podcast.api` from terminal located in the root folder. 

Open Visual Studio select ListenTogether.Hub as start up project and hit f5.

# Deployment <a name="deployment"></a>

## Deploy using GitHub Actions

Before deploying this service It is required to deploy `podcast-api.yml` workflow. This will allow to configure API url automatically.

To configure the Github Action it is necessary to create an environment called `prod` and set up the following secrets. (Consider that the action will create these resources automatically, so resource names need to be unique). [Learn how to create an environment.](https://docs.github.com/en/actions/deployment/targeting-different-environments/using-environments-for-deployment)

- `AZURE_CREDENTIALS` : service principal to authenticate with Azure. See [Create Azure Credentials](https://docs.microsoft.com/en-us/azure/developer/github/connect-from-azure?tabs=azure-portal%2Cwindows#create-a-service-principal-and-add-it-as-a-github-secret).
- `PODCASTDB_USER_LOGIN` : Username for Podcast db. 
- `PODCASTDB_USER_PASSWORD` : Password for Podcast db.
- `AZURE_RESOURCE_GROUP_NAME`: Existing resource group where resources will be deployed.
- `HUB_WEBAPP_NAME`: Web App name
- `SERVICE_PLAN_NAME`: Service plan name
- `PODCASTDB_SERVER_NAME`: Azure SQL server name