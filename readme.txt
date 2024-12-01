Im validating the login credentials against the database user credentials (no frontend hardcoding)

Frontend should run on port 3000
backend should run on port 5004
---
## Run the frontend
npm install
npm start

## Run the backend
dotnet restore
dotnet build
dotnet run

## run the sql server database

im using the developer version of sql server.

make sure to change connection string (Data Source) in the appsettings.json file
example:"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=tailshark;Trusted_Connection=True;"
There is a .bak file if needed for backup of database
Applicable table is only the Users table