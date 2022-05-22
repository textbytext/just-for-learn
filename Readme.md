# 18 Jan 2022
Run docker images separetely
```
docker run -p 9090:8080 keycloak-with-admin //keycloak

docker build -t appstory-front . // build
docker run -p 9080:80 appstory-front //appstory-front
```

# 17 Jan 2022
## Create Docker Compose
```
1) docker-compose build
2) docker-compose up
```

# 16 Jan 2022

## Add Keycloak
1) Run keycloak with setup admin user
```
docker run -p 9080:8080 -e KEYCLOAK_USER=admin -e KEYCLOAK_PASSWORD=admin --name appstorykeycloak jboss/keycloak
```

2) Create a docker image from a keycloak image [https://www.sentinelone.com/blog/create-docker-image/](https://www.sentinelone.com/blog/create-docker-image/)
```
docker commit appstorykeycloak
docker images // get ID of newlly created image
docker tag <ID> keycloak-with-admin
```

3) Run a new keycloak image (with admin)
```
docker run -p 9080:8080 --name appstorykeycloak keycloak-with-admin
```

## Create microservices with .NET and ASP.NET Core
[https://docs.microsoft.com/en-us/learn/paths/create-microservices-with-dotnet/](https://docs.microsoft.com/en-us/learn/paths/create-microservices-with-dotnet/)

# 14 JAN 2022

## Docker

Docker info about: [https://docs.microsoft.com/en-us/learn/browse/?terms=docker](https://docs.microsoft.com/en-us/learn/browse/?terms=docker)

1) ### Introduction to Docker containers
[https://docs.microsoft.com/en-us/learn/modules/intro-to-docker-containers/](https://docs.microsoft.com/en-us/learn/modules/intro-to-docker-containers/)

2) ### Docker Compose about
[https://docs.microsoft.com/en-us/learn/modules/dotnet-microservices/?WT.mc_id=friends-0000-NANIL](https://docs.microsoft.com/en-us/learn/modules/dotnet-microservices/?WT.mc_id=friends-0000-NANIL)

3) ### Introduction to Kubernetes
[https://docs.microsoft.com/en-us/learn/modules/intro-to-kubernetes/](https://docs.microsoft.com/en-us/learn/modules/intro-to-kubernetes/)


## Keycloack

# 24 Dec 2021
+ Authorization
+ JWT

# 13 Dec 2021
+ DbContext


```
## Initial Create:
dotnet ef migrations add InitialCreate

## Add migration:
dotnet ef migrations add YOUR_MIGRATION_NAME

## Remove LAST! Migration
dotnet ef migrations remove

## Update DB:
dotnet ef database update

## Drop DB:
dotnet ef database drop
```

# 26 Nov 2021
+ Tests
+ Integration Tests

# 12 Nov 2021
+ Exception Handler
+ Refactoring
+ ProducesResponseType + global  (for Swagger)
+ Request Metrics

# 29 Oct 2021
+ Mediator
+ Request
+ RequestHandler
+ IRequestPreProcessor
