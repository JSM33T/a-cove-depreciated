
# .NET Project Structure

This repository contains the source code for a community website project made with .NET/Typescript, organized with modular design principles for better maintainability and scalability. The project includes modules, services, interfaces, middlewares, and extensions.

## Project Structure

### C#/.NET:
  - **src:** Contains all .NET source files.
  - **Utility Classes:** Directory for modularizing features. Each feature has its own folder with controllers, services, and other related files.
  - **Services:** Directory for common services used across multiple modules.
  - **Controllers** : 
    - Routers : handle routing and prerendering of contents
    - Api  : Api controllers for lazy loading & js operations

  - **Interfaces:** Directory for defining interfaces, promoting abstraction and separation of concerns.
  - **Middlewares:** Directory for custom middlewares used in the project.
  - **Extensions:** Directory for extension methods or service collection extensions.
  - **Repositories** : Data access methods.
  - **Migrations** : SQL files for migrating db schema.
  - **appsettings.json:** Configuration file for application settings.
  - **Program.cs:** Main entry point for configuring services, middleware, and the application pipeline.

### Typescript

 - **global.ts** : Contains global accessible functions imported by submodules
 - **dedicated modules** : Page specific modules to carry around dedicated operations
 - **interfaces** : interfaces to define the shape of incoming DTOs


## Configuration and Startup

- **appsettings.json:** Configuration file containing application settings, connection strings, etc.
- **Program.cs** : Main entry point for configuring services, middleware, and the application pipeline.
- **Migrations** : Migrations

## Project File

- **project.csproj:** The .NET project file specifying dependencies, references, and other project-related configurations.

### Components/File types

- **Scripts (Typescript):** Contains all TypeScript source files.
  - **global.ts file:** Global TypeScript file containing common functions, constants, or configurations used across multiple modules.
  - **dedicated .ts files:** Dedicated TypeScript files for specific features or modules in the individual project pages.
  - **interface .ts files:** Interfaces for data structures.

- **tsconfig.json:** TypeScript configuration file.

## Tech stack used 

![TypeScript](https://img.shields.io/badge/typescript-%23007ACC.svg?style=for-the-badge&logo=typescript&logoColor=white)![.NET](https://img.shields.io/badge/.NET-%235C2D91.svg?style=for-the-badge&logo=.net&logoColor=white)![C#](https://img.shields.io/badge/csharp-%23000000.svg?style=for-the-badge&logo=csharp&logoColor=violet)


![PWA](https://img.shields.io/badge/pwa-%232496ED.svg?style=for-the-badge&logo=pwa&logoColor=white)
![SWC](https://img.shields.io/badge/swc-%23000000.svg?style=for-the-badge&logo=swc&logoColor=white)

