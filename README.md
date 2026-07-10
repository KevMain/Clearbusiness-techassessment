# Clearbusiness-techassessment

This repository contains a minimal ASP.NET Core backend and a Vue 3 (Vite) frontend used for the technical assessment.

Quick start — Backend

1. Open a terminal and run:

   cd TechnicalAssessment/TechnicalAssessment
   dotnet restore
   dotnet build
   dotnet run

   dotnet run prints the application URL(s) (for example: https://localhost:5001). The API endpoint is:

   GET {BASE_URL}/api/hello

Quick start — Frontend

1. Open a terminal and run:

   cd ClientApp
   npm install
   npm run dev

   Vite prints the dev server URL (e.g., http://localhost:5173). By default the frontend calls `/api/hello`.

Proxy / CORS

If the backend runs on a different origin/port when developing, configure a Vite dev proxy (see ClientApp/README.md) or update the fetch URL in `ClientApp/src/App.vue` to the full API URL.

More details

- See `TechnicalAssessment/README.md` for backend details.
- See `ClientApp/README.md` for frontend details and example Vite proxy configuration.

One-command run (Windows / PowerShell)

From the repository root you can run a single PowerShell script that launches the backend and frontend in separate PowerShell windows:

   .\run.ps1

This opens two new terminals: one running the ASP.NET backend and one running the Vite dev server.

