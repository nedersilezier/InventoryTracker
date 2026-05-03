# InventoryTracker

Inventory management platform built with ASP.NET Core, Clean Architecture and CQRS.

## Overview

InventoryTracker is a work-in-progress inventory management system for tracking item flow between clients and warehouses

The project is designed as a multi-client solution with:
- ASP.NET Core Web API
- ASP.NET MVC WebAdmin
- ASP.NET MVC WebOperator
- React Native / Expo mobile client for warehouse operator
- shared contracts between API and clients

The main goal of this project is to build a practical inventory system while learning and applying modern backend architecture patterns in .NET.

## Status

This project is currently **work in progress**.

Already implemented:
- MVP backend API for authentication, user management, master data and inventory transactions
- ASP.NET Core Identity authentication and authorization
- JWT access tokens and refresh token flow
- FluentValidation with global exception handling in API
- Role-based access control
- Basic inventory-related modules
- Shared contracts between API and clients
- Secured admin API endpoints
- Reusable ApiClient layer for web clients
- `AccessTokenHandler` for web clients
- automatic refresh token handling for web and mobile
- `ServiceResult` pattern for handling API responses and parsing validation/errors in web clients
- WebAdmin integration with API through services
- WebAdmin MVP with paginated browsing, filters and search, full CRUD
- WebAdmin transaction creation, editing, approval and cancellation 
- Mobile paginated transaction browsing with search and filtering
- Mobile transaction creation and editing in mobile


Still in progress:
- WebOperator client
- Mobile client features
- Client's balance support in the domain model and transaction logic
- Transaction document generator
- deployment setup

## Main Features

### Authentication and authorization
- ASP.NET Core Identity
- JWT authentication
- refresh tokens
- login / refresh / logout flow
- role-based authorization
- Admin/User role support

### Admin user management
- list users
- search users
- pagination
- create users
- edit users
- assign roles
- activate / deactivate users

### Inventory master data
- items
- clients with address data
- countries
- warehouses with address data

### Stock and transaction control
Stock levels are controlled through the transaction workflow instead of being managed as simple standalone CRUD data.

The system supports:
- inventory transactions(adjustment, issue, return, transfer)
- transaction items
- stock changes based on transaction approval
- transaction status handling
- warehouse-based stock tracking

### Multi-client architecture
The solution contains multiple clients consuming the same API:
- WebAdmin – administration panel
- WebOperator – operator-facing web client
- Mobile – operator-facing React Native / Expo mobile client

## Architecture

The solution follows a Clean Architecture-inspired structure:
````text
InventoryTracker.API             - ASP.NET Core Web API
InventoryTracker.Application     - CQRS commands, queries, handlers, validation
InventoryTracker.Domain          - domain entities and core business model
InventoryTracker.Infrastructure  - EF Core, Identity, persistence, services
InventoryTracker.Contracts       - shared request/response DTOs
InventoryTracker.Shared          - shared enums and common types
InventoryTracker.WebAdmin        - ASP.NET MVC admin client
InventoryTracker.WebOperator     - ASP.NET MVC operator client
InventoryTracker.Mobile          - React Native / Expo mobile client
InventoryTracker.APIClient       - API communication helper layer
InventoryTracker.AuthClient      - authentication client layer
````
## Backend Architecture

### CQRS with MediatR

The application layer separates write operations and read operations using commands and queries.

Examples:
- `CreateUserCommand`
- `UpdateUserCommand`
- `GetUsersQuery`
- `CreateTransactionCommand`
- `ApproveTransactionCommand`
- `CancelTransactionCommand`
- `GetTransactionsQuery`

### Validation pipeline

FluentValidation is integrated through a MediatR pipeline behavior, so requests are validated before reaching command/query handlers.

API validation errors are handled by a global exception handler and returned in a consistent format to clients.

### Persistence and infrastructure

The infrastructure layer contains the concrete persistence and service implementations used by the application.

It includes:
- Entity Framework Core database context
- ASP.NET Core Identity configuration
- repositories for selected domain areas
- authentication and token services
- application service implementations used by the API layer

The project currently uses a local development database.

### Auditing and soft delete

The database context automatically handles audit fields such as:
- `CreatedAt`
- `CreatedBy`
- `UpdatedAt`
- `UpdatedBy`

Soft-deletable entities are deactivated instead of being physically removed.

## API

The API exposes secured endpoints for managing system data and inventory workflows.

Example endpoint groups:
- `/api/admin/users`
- `/api/admin/items`
- `/api/admin/warehouses`
- `/api/admin/transactions`
- `/api/user/transactions`

Swagger/OpenAPI is configured for development and supports Bearer token authentication.

## Client API Integration

Web clients communicate with the backend API through a reusable API client layer.

The web client infrastructure includes:
- typed API client services
- shared request/response contracts
- `AccessTokenHandler` for attaching access tokens to outgoing API requests
- automatic refresh token handling when access tokens expire
- `ServiceResult` pattern for checking API responses and parsing validation/errors
- centralized API communication instead of duplicating HTTP logic in controllers

## WebAdmin

The WebAdmin client is an ASP.NET MVC application that communicates with the backend API through reusable client services.

Current WebAdmin MVP includes:
- paginated browsing, filtering and search
- full CRUD for users
- full CRUD for master data
- full CRUD for inventory transactions
- transaction creation and editing
- transaction approval and cancellation
- API validation error handling
- token-based API communication
- automatic access token refresh through the web client infrastructure

## Mobile Client

The mobile client is built with React Native / Expo and communicates with the same backend API.

Current mobile MVP includes:
- paginated transaction browsing
- transaction search and filtering
- transaction creation
- transaction editing
- automatic refresh token handling

Additional mobile features are still in progress.

## Tech Stack

### Backend
- C#
- ASP.NET Core
- ASP.NET Core Identity
- Entity Framework Core
- SQL Server
- MediatR
- FluentValidation
- JWT Bearer Authentication
- Swagger / OpenAPI

### Frontend / Clients
- ASP.NET MVC
- Razor Views
- React Native
- Expo
- TypeScript

### Architecture / Patterns
- Clean Architecture
- CQRS
- Repository implementations for selected domain areas
- DTO contracts
- Dependency Injection
- Role-based authorization
- Auditing
- Soft delete
- reusable API client layer
- centralized API response handling

## Notable Implementation Details

- JWT authentication with refresh token rotation
- inactive users cannot log in
- refresh tokens can be revoked during logout
- secured admin API endpoints with role-based authorization
- MediatR validation pipeline
- FluentValidation with global API exception handling
- shared contracts between API and clients
- reusable API client layer for web clients
- `AccessTokenHandler` for secured API communication
- automatic refresh token handling for web and mobile clients
- `ServiceResult` pattern for API response and error handling
- WebAdmin full CRUD integrated with secured backend endpoints
- stock controlled through transaction workflows instead of direct manual edits
- transaction approval and cancellation flow
- transaction domain model prepared for stock movements and warehouse operations

## Running the project

### Requirements
- Visual Studio 2022 or compatible version
- .NET 10
- SQL Server
- Node.js / npm for the mobile client

### Backend setup
1. Clone the repository.
2. Configure the API connection string in `appsettings.json` or user secrets.
3. Configure JWT settings.
4. Apply EF Core migrations or create the database.
5. Run `InventoryTracker.API`.

### WebAdmin setup
1. Make sure the API is running.
2. Configure `ApiSettings:BaseUrl` in WebAdmin settings.
3. Run `InventoryTracker.WebAdmin`.

### Mobile setup

1. Go to the mobile project directory:

```bash
cd InventoryTracker.Mobile/inventory-tracker
```

2. Install dependencies:

```bash
npm install
```

3. Start Expo:

```bash
npx expo start
```

## Screenshots

### WebAdmin

![WebAdmin Login](Screenshots/web_login.png)

![WebAdmin Transactions](Screenshots/web_transactions.png)

![WebAdmin New Transaction](Screenshots/web_return.png)

![WebAdmin Clients](Screenshots/web_clients.png)

![WebAdmin Client Details](Screenshots/web_client_details.png)

### Mobile

![Mobile Login](Screenshots/mobile_login.png)

![Mobile Transactions](Screenshots/mobile_transactions.png)

![Mobile Search](Screenshots/mobile_search_modal.png)

![Mobile Filter](Screenshots/mobile_filters_modal.png)

![Mobile New Transaction - General](Screenshots/mobile_new_transaction1.png)

![Mobile New Transaction - General](Screenshots/mobile_new_transaction2.png)

![Mobile New Transaction - Items](Screenshots/mobile_new_transaction3.png)

![Mobile New Transaction - Review](Screenshots/mobile_new_transaction4.png)

## What I am learning / practicing

While building this project, I am working on:
- designing a multi-project .NET solution
- applying Clean Architecture principles
- implementing CQRS with MediatR
- securing APIs with JWT and refresh tokens
- building role-based admin functionality
- connecting MVC and mobile clients to a shared API
- implementing reusable API client infrastructure
- handling API responses and validation errors
- modelling inventory and warehouse workflows
- structuring a larger application beyond simple CRUD

## Repository Notes

This repository is a learning project and is actively being developed.

The focus is on backend architecture, authentication, authorization, API design, client/API integration and practical inventory management workflows.

The backend API, WebAdmin MVP and selected mobile transaction flows are already functional, while additional operator and mobile features are still being implemented.