# BuberBreakfast

This is my extension of **BuberBreakfast**, where I implement **Entity Framework (EF)** along with **PostgreSQL** to ensure data persistence in the database. Additionally, I've integrated authentication using **JSON Web Tokens (JWT)** to enhance security and manage user sessions.

This project is a modification of the original **buber-breakfast** project, which can be found at [buber-breakfast Original Repository](https://github.com/amantinband/buber-breakfast). 

## Project Dependencies

This project requires the following NuGet packages:

- **BCrypt.Net-Next**: Version 4.0.3
- **ErrorOr**: Version 2.0.1
- **Microsoft.AspNetCore.Authentication.JwtBearer**: Version 6.0.3
- **Microsoft.EntityFrameworkCore**: Version 7.0.11
- **Microsoft.EntityFrameworkCore.Design**: Version 7.0.5
- **Microsoft.EntityFrameworkCore.Tools**: Version 7.0.5
- **Npgsql.EntityFrameworkCore.PostgreSQL**: Version 7.0.11

## Entity Relationship Diagram (ERD)

![Entity Relationship Diagram](https://i.imgur.com/Oj2JM3L.png)

## Getting Started

To get started with the project, clone the repository and restore the dependencies:

```bash
git clone https://github.com/carlosxmerca/BuberBreakfast
cd /BuberBreakfast/BuberBreakfast
dotnet restore
dotnet ef database update   # Run migrations to update the database
dotnet build                # Build the project
dotnet run                  # Start the application
```
