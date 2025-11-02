# Solar Systems Sales Platform

This repository contains a sophisticated solar systems sales platform designed to demonstrate a modern, full-stack application using microservices, Clean Architecture, and real-time communication. Customers can browse solar system models, request personalized quotes, and track their status in real-time.

The backend is built with .NET 8 following **Clean Architecture** principles, ensuring a clear separation of concerns between the Domain, Application, and Infrastructure layers. It leverages asynchronous processing via a message queue for handling quote requests and uses Redis for caching and real-time notifications. The frontend is a modern single-page application built with Angular 18, utilizing Signals for state management.

## Core Features

- **Customer Quote Requests**: Users can request personalized quotes for solar systems based on their location and custom configurations.
- **Asynchronous Processing**: Quote requests are placed on a message queue (e.g., RabbitMQ) and processed by a background worker, ensuring the API remains responsive.
- **Real-Time Updates**: The frontend receives live status updates for quotes and orders via WebSockets (SignalR), powered by a Redis Pub/Sub backbone.
- **Secure Authentication**: APIs are secured using JWT with role-based authorization (Admin/Customer).
- **Admin Dashboard**: A dedicated view for administrators to monitor and manage all incoming quote requests and orders.

## Tech Stack

| Area      | Technology / Principle                               |
|-----------|------------------------------------------------------|
| Backend   | .NET 8, ASP.NET Core Web API, C#                     |
| Frontend* | Angular 18, TypeScript, Angular Signals              |
| Database  | SQL Server                                           |
| Caching   | Redis (for caching processed quotes & orders)        |
| Messaging | RabbitMQ (for async processing)                      |
| Real-Time | SignalR, Redis Pub/Sub, WebSockets                   |
| Arch.     | Clean Architecture, Microservices, RESTful APIs, CQRS|
| Auth      | JWT (JSON Web Tokens), ASP.NET Core Identity         |
| Testing*  | xUnit, Moq, FluentAssertions                         |
*WIP
## Prerequisites

Before you begin, ensure you have the following software installed on your machine:

- **Visual Studio 2022**: With the "ASP.NET and web development" workload installed.
- **.NET 8 SDK**.
- **Docker Desktop**: Ensure it is running and configured to use Linux containers.
- **Node.js and npm**: For running the Angular frontend.
- **Git**: For cloning the repository.

## Getting Started

Follow these instructions to get the application up and running on your local machine.

### 1. Clone the Repository

Open a terminal or command prompt and clone the repository to your local machine:

```bash
git clone https://github.com/awebdesigner09/solarsystems
cd solarsystems
```

### 2. Running with Visual Studio 2022 & Docker Compose

This is the simplest way to run the entire application stack. The solution is configured to use Docker Compose for orchestration.

1.  **Open the Solution**: Open the `SolarSystems.sln` file in Visual Studio 2022.
2.  **Set Startup Project**: In the Solution Explorer, right-click the `docker-compose` project and select **Set as Startup Project**.
3.  **Run the Application**: Press **F5** or click the "Docker Compose" run button (with the green play icon) in the Visual Studio toolbar.

Visual Studio will perform the following steps:
- Pull necessary base images from Docker Hub.
- Build Docker images for each project.
- Start all the containers defined in the `docker-compose.yml` file.

Once all services are running, you can interact with the application.

### 3. Running with the Command Line (Docker Compose)

Alternatively, you can use the command line to build and run the Docker containers.

1.  **Navigate to the root directory** of the cloned repository.
2.  **Run the following command**:

    ```bash
    docker-compose up --build
    ```

This command will build the images and start all the services. To stop the services, press `Ctrl+C` in the terminal, and then run `docker-compose down` to remove the containers.

## Services Overview

The solution consists of multiple services that are containerized using Docker. Here is an overview of the services and the ports they expose:

| Service Name         | Description                                          | Host Port(s) | Access URL / UI                               |
| -------------------- | ---------------------------------------------------- | ------------ | --------------------------------------------- |
| `ui`                 | Angular 18 client application.                       | `3001`       | `http://localhost:3001`                       |
| `sales.api`          | Main API for handling quotes, orders, and users.     | `4041`       | `http://localhost:4041`               |
| `sales.quotesworker` | Background service to process quote requests.        | N/A          | N/A (no direct access)                        |
| `salesdb`            | SQL Server database for storing application data.    | `1433`       | Use SSMS                                      |
| `messagebroker`      | RabbitMQ for asynchronous messaging.                 | `15672`      | `http://localhost:15672` (Management UI)      |
| `distributedcache`   | Redis for caching and real-time notifications.       | `6379`       | Use CLI                                       |

**Note:** The host ports are configurable in the `docker-compose.override.yml` file.

## How to Use the Application

### 1. Access the Client Application

Once the services are running, you can access the web client by navigating to:

**`http://localhost:3001`**

The web application interacts with the backend services through the API Gateway.

### 2. Explore the APIs

The APIs are documented using Swagger/OpenAPI. You can explore and interact with the API endpoints directly through your browser.

- **Sales API**: The direct endpoint for the sales and user management service.
  - **URL**: `https://localhost:4041`

### 3. Initial Data and Users

The `Sales.Api` service is configured to seed the database with initial data on startup. When the application runs for the first time, it will populate the database with:
- Sample solar system models.
- Pre-defined user accounts for testing:
  - **Admin**: `admin@solarsystems.com` / `Admin123!`
  - **Customer**: `johndoe@email.com` / `Customer123!`

You can verify this data by using the API endpoints or connecting to the database.

### 4. Connecting to the Database

You can connect to the SQL Server instance using any database management tool (like Azure Data Studio or SSMS).

- **Host**: `localhost`
- **Port**: `1433`
- **User**: `sa`
- **Password**: `MyStr0ngPa$w0rd` (as defined in `docker-compose.override.yml`)
- **Database**: `SalesDB`