
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

// Other common functions and constants
