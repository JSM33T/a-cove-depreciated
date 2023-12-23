
# .NET Project Structure

This repository contains the source code for a community website project made with .NET/Typescript, organized with modular design principles for better maintainability and scalability. The project includes modules, services, interfaces, middlewares, and extensions.

## Project Structure

### C#/.NET:
  - **src:** Contains all .NET source files.
  - **Module Classes:** Helper classes with specific functions that dont need a separate service representation.
  - **Services:** Directory for common services used across multiple modules.
  - **Controllers** : 
    - Routers : Handle routing and prerendering of contents
    - Api  : Api controllers for lazy loading & data fetching

  - **Interfaces:** Directory for defining interfaces, promoting abstraction and separation of concerns.
  - **Middlewares:** Directory for custom middlewares used in the project.
  - **Extensions:** Directory for extension methods or service collection extensions.
  - **Repositories** : Data access methods.
  - **Filters** : Action filters for permission management.

### Typescript

 - **global.ts** : Contains global accessible functions imported by submodules
 - **dedicated modules** : Page specific modules to carry around dedicated operations
 - **interfaces** : interfaces to define the shape of incoming DTOs


## Configuration and Startup

- **appsettings.json:** Configuration file containing application settings, connection strings, etc.
- **tsconfig.json:** TypeScript configuration file.
- **Program.cs** : Main entry point for configuring services, middleware, and the application pipeline.
- **Migrations** : SQL files for migrating db schema

## Project File

- **project.csproj:** The .NET project file specifying dependencies, references, and other project-related configurations.






## Tech stack used 

### Languages & framework

![TypeScript](https://img.shields.io/badge/typescript-%23007ACC.svg?style=for-the-badge&logo=typescript&logoColor=white) ![Javascript](https://img.shields.io/badge/javascript-%23007ACC.svg?style=for-the-badge&logo=typescript&logoColor=white) ![C#](https://img.shields.io/badge/csharp-%23000000.svg?style=for-the-badge&logo=csharp&logoColor=violet) ![C#](https://img.shields.io/badge/css-%23000000.svg?style=for-the-badge&logo=css3&logoColor=violet) 

![.NET](https://img.shields.io/badge/.NET-%235C2D91.svg?style=for-the-badge&logo=.net&logoColor=white) ![VuE JS](https://img.shields.io/badge/vue-%4FC08D.svg?style=for-the-badge&logo=VUEDOTJS&logoColor=white) ![NPM](https://img.shields.io/badge/npm-%339933.svg?style=for-the-badge&logo=nodedotjs&logoColor=white)



### Build & Tooling
 ![SWC](https://img.shields.io/badge/swc-%23000000.svg?style=for-the-badge&logo=swc&logoColor=white) ![NPM](https://img.shields.io/badge/npm-%232496ED.svg?style=for-the-badge&logo=npm&logoColor=white)

### Package Manager

![NUGET](https://img.shields.io/badge/nuget-%232496ED.svg?style=for-the-badge&logo=nuget&logoColor=white)  



### Database

![NUGET](https://img.shields.io/badge/MS%20SQL%20SERVER-%232496ED.svg?style=for-the-badge&logo=microsoft-sql-server&logoColor=white) 


