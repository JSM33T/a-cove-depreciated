
# .NET Project Structure

This repository contains the source code for a community website project made with .NET/Typescript, organized with modular design principles for better maintainability and scalability. The project includes modules, services, interfaces, middlewares, and extensions.

## Project Structure

The .NET project is organized as follows:



- **src:** Contains all .NET source files.
  - **Modules:** Directory for modularizing features. Each feature has its own folder with controllers, services, and other related files.
  - **Services:** Directory for common services used across multiple modules.
  - **Interfaces:** Directory for defining interfaces, promoting abstraction and separation of concerns.
  - **Middlewares:** Directory for custom middlewares used in the project.
  - **Extensions:** Directory for extension methods or service collection extensions.
  - **appsettings.json:** Configuration file for application settings.
  - **Startup.cs:** Main entry point for configuring services, middleware, and the application pipeline.

## Key Components

### 1. Services

directory contains common services used across the application:

- `Feature1Controller.cs`: Controller for handling HTTP requests related to Feature1.
- `Feature1Service.cs`: Service class containing the business logic for Feature1.

### 2. Modules (statically imported functionalities)

The `Modules` directory contains common services used across the application:

- `CommonService.cs`: Common service providing shared functionality.

### 3. Interfaces

The `Interfaces` directory contains interfaces for promoting abstraction:

- `IFeature1Service.cs`: Interface defining the contract for Feature1 service.
- `IFeature2Service.cs`: Interface defining the contract for Feature2 service.

### 4. Middlewares

The `Middlewares` directory contains custom middlewares:

- `CustomMiddleware.cs`: Custom middleware for handling specific aspects of the request/response pipeline.

### 5. Extensions

The `Extensions` directory contains extension methods or service collection extensions:

- `ServiceCollectionExtensions.cs`: Extension methods for IServiceCollection.

## Configuration and Startup

- **appsettings.json:** Configuration file containing application settings, connection strings, etc.
- **Startup.cs:** Main entry point for configuring services, middleware, and the application pipeline.

## Project File

- **project.csproj:** The .NET project file specifying dependencies, references, and other project-related configurations.

Feel free to explore and customize the project structure according to your specific requirements. If you have any questions or need further clarification, please don't hesitate to reach out.

Happy coding!


### Components/File types

- **Scripts (Typescript):** Contains all TypeScript source files.
  - **global.ts file:** Global TypeScript file containing common functions, constants, or configurations used across multiple modules.
  - **dedicated .ts files:** Dedicated TypeScript files for specific features or modules in the individual project pages.
  - **interface .ts files:** Interfaces for data structures.

- **tsconfig.json:** TypeScript configuration file.

## Global TypeScript File (`global.ts`)

The `global.ts` file serves as a central location for common functionalities and constants used throughout the project. For example:

```typescript
// global.ts

export const API_BASE_URL = 'https://api.example.com';

export function logMessage(message: string): void {
  console.log(`[LOG]: ${message}`);
}

// Other common functions and constants
