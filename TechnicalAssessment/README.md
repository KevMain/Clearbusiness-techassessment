# TechnicalAssessment (inside solution folder)

This folder contains the ASP.NET Core web project for the Hello World sample. The project exposes a single API endpoint `/api/hello` that the Vue frontend calls.

Run (CLI)
1. Open a terminal and run:

   cd TechnicalAssessment/TechnicalAssessment
   dotnet restore
   dotnet build
   dotnet run

2. dotnet run prints the application URL(s) (for example: https://localhost:5001). The API endpoint is:

   GET {BASE_URL}/api/hello

Run (Visual Studio)
1. Open `TechnicalAssessment.slnx` in Visual Studio
2. Set the `TechnicalAssessment` project as the startup project and run/debug

Frontend
- See `../ClientApp/README.md` for frontend run instructions and proxy configuration.
