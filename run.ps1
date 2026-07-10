# run.ps1 - launch backend and frontend in separate PowerShell windows
# Usage: .\run.ps1

$backendDir = Join-Path $PSScriptRoot 'TechnicalAssessment\TechnicalAssessment'
$frontendDir = Join-Path $PSScriptRoot 'ClientApp'

Write-Host "Starting backend in a new PowerShell window (dotnet run) ..."
Start-Process powershell -ArgumentList "-NoExit","-Command","cd '$backendDir'; dotnet run" -WorkingDirectory $backendDir

Write-Host "Starting frontend in a new PowerShell window (npm run dev) ..."
Start-Process powershell -ArgumentList "-NoExit","-Command","cd '$frontendDir'; npm install; npm run dev" -WorkingDirectory $frontendDir

Write-Host "Launched backend and frontend."
