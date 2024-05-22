# Dotnet 8 API Boilerplate with MySQL & JWT Auth 

This is a simple barebones API boilerplate that has JWT configured for authentication and database setup with MySQL. No framework required, just pure dotnet 8. Sample controllers are also included for role authorization.

## Features
- MySQL Database with EFCore
- JWT Authentication
- Admin and User roles
- Rate Limit
- Anti-spam Registration
- Swagger UI
- Serilog


## Development

Run migrations with the following command:
`dotnet ef database update`

To make a new migration:
`dotnet ef migrations add MigrationName`

To remove a migration:
`dotnet ef migrations remove`

To run the project:
`dotnet run`

## To Do
- [ ] Change password hash to Bcrypt
- [ ] Add email verification

---

## License
A large portion of the code was adopted from the Spark framework but they dropped support for Web API in version 2 so I decided to create my own. I also didn't want to rely on a framework to get the auth implemented.

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.