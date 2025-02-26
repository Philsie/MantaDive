# Script to Check System and Environment for Python Project Setup

Write-Host "--- System and Environment Check ---" -ForegroundColor Cyan

# Check Operating System
Write-Host "Operating System: $($PSVersionTable.OS)" -ForegroundColor Cyan

# Check PowerShell Version
Write-Host "PowerShell Version: $($PSVersionTable.PSVersion)" -ForegroundColor Cyan

# Check Python Installation
Write-Host "--- Python Checks ---" -ForegroundColor Cyan
try {
    $pythonVersion = python --version 2>&1
    Write-Host "Python Version: $pythonVersion" -ForegroundColor Green
} catch {
    Write-Host "Python not found or not in PATH." -ForegroundColor Red
}

# Check Pip Installation
try {
    $pipVersion = pip --version 2>&1
    Write-Host "Pip Version: $pipVersion" -ForegroundColor Green
} catch {
    Write-Host "Pip not found or not in PATH." -ForegroundColor Red
}

# Check Virtual Environment
Write-Host "--- Virtual Environment Checks ---" -ForegroundColor Cyan
if ($env:VIRTUAL_ENV) {
    Write-Host "Virtual Environment Active: $($env:VIRTUAL_ENV)" -ForegroundColor Green
} else {
    Write-Host "No Virtual Environment Active." -ForegroundColor Yellow
}

# Check .venv Directory
if (Test-Path -Path ./.venv) {
    Write-Host ".venv Directory Exists." -ForegroundColor Green
} else {
    Write-Host ".venv Directory Does Not Exist." -ForegroundColor Yellow
}

# Check Requirements.txt
Write-Host "--- Requirements Checks ---" -ForegroundColor Cyan
if (Test-Path -Path ./requirements.txt) {
    Write-Host "requirements.txt Exists." -ForegroundColor Green
    # Display the contents of requirements.txt
    Write-Host "Contents of requirements.txt:" -ForegroundColor Green
    Get-Content ./requirements.txt
} else {
    Write-Host "requirements.txt Does Not Exist." -ForegroundColor Red
}

# Check for PostgreSQL
Write-Host "--- PostgreSQL Checks ---" -ForegroundColor Cyan
try {
    $psqlVersion = psql --version 2>&1
    Write-Host "PostgreSQL Client: $psqlVersion" -ForegroundColor Green
} catch {
    Write-Host "PostgreSQL Client not found or not in PATH." -ForegroundColor Yellow
}

# Check Visual Studio Build Tools (Basic Check)
Write-Host "--- Visual Studio Build Tools Checks (Basic) ---" -ForegroundColor Cyan
try {
    $clVersion = cl 2>&1 | Select-String "Compiler version"
    if ($clVersion) {
        Write-Host "Visual Studio Build Tools (cl.exe) Found." -ForegroundColor Green
        Write-Host "$clVersion" -ForegroundColor Green
    } else {
        Write-Host "Visual Studio Build Tools (cl.exe) Not Found (or version info not found)." -ForegroundColor Yellow
    }
} catch {
    Write-Host "Visual Studio Build Tools (cl.exe) Not Found." -ForegroundColor Red
}

Write-Host "--- End of Checks ---" -ForegroundColor Cyan