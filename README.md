# Biodiversity Cloud Backend

This is the backend API for the **Biodiversity Cloud Platform**, designed for **storing and analyzing biodiversity data** in Moldova. Built with **.NET Core, Entity Framework, and PostgreSQL**, this API supports **offline data collection, real-time syncing, and analytics**.

## Features
- **User Authentication (JWT)**
- **Observations Management (CRUD)**
-  **Location-based Species Tracking**
- **Photo & Comments Support**
- **Offline Data Syncing**
- **Data Visualization & Reports**
- **Role-Based Access Control (RBAC)**

## Tech Stack
- **Backend:** C#, .NET Core Web API
- **Database:** PostgreSQL (Cloud)
- **Authentication:** JWT + Role-Based Access Control

## How to run

### 1. Download postgres

`docker pull postgres:14.17`

### 2. Create the volume to store the data

`docker volume create biodiversity-data`

### 3. Run the database container

```bash
docker run \
    -d \
    -p \
    5432:5432 \
    --name biodiversity-db \
    -e POSTGRES_DB=biodiversity_db \
    -e POSTGRES_USER=postgres \
    -e POSTGRES_PASSWORD=Bio20Diversity25 \
    -v biodiversity-data:/var/lib/postgresql/data \
    postgres:14.17
```

### 4. Apply the migration (only for the first run)

install `ef` tool: `dotnet tool install --global dotnet-ef`

create the migration: `dotnet ef migrations add start`

apply the migration to the database `dotnet ef database update`

### 5. Run the project

`dotnet run`

## Access the swagger

CTRL + Click on [this](http://localhost:5044/swagger/index.html) link