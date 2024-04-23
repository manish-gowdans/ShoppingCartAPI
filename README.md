# Mini Shopping Cart API

Welcome to the Mini Shopping Cart API project! This API provides endpoints for managing shopping cart system.

## Project Setup Guide

### Prerequisites
1. **Visual Studio 2022**: Make sure that Visual Studio 2022 is installed. It can be downloaded from [here](https://visualstudio.microsoft.com/downloads/).
2. **SQL Server and SQL Server Management Studio (SSMS)**: Download SQL Server from [here](https://www.microsoft.com/en-in/sql-server/sql-server-downloads) and SQL Server Management Studio (SSMS) from [here](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16).
3. **Postman**: Download Postman from [here](https://www.postman.com/downloads/) and install it on machine to test the API endpoints.
4. **NuGet Packages**: Ensure the following NuGet packages are installed in project:
   - Microsoft.EntityFrameworkCore
   - Microsoft.EntityFrameworkCore.Sqlite
   - Microsoft.EntityFrameworkCore.SqlServer
   - Microsoft.EntityFrameworkCore.Tools

### Installation Steps
1. **Clone the Repository**: Clone the Mini Shopping Cart API repository using gitbash.

2. **Open Project in Visual Studio**: Open the solution file (`MiniShoppingCartAPI.sln`) in Visual Studio 2022.

3. **Install NuGet Packages**: Open (`Manage Nuget Packages`) through right click on (`Dependencies`) in (`Solution explorer`) and then install the mentioned packages

4. **Database Migration**: Execute the following commands in the Package Manager Console to perform database migration:
   - `add-migration <Migration-Name>`
   - `update-database`

### Testing the API
1. **Run the Application**: Build and run the Mini Shopping Cart API project in Visual Studio.

2. **Test Endpoints with Postman**: Import the provided Postman collections to test the API endpoints.
