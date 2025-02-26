Write-Host "Checking if .venv exists..." -ForegroundColor Cyan
if (Test-Path -Path ./.venv) {
    Write-Host ".venv found. Removing..." -ForegroundColor Cyan
    rm .venv -Recurse -Force -Confirm:$false
    Write-Host ".venv removed." -ForegroundColor Cyan
} else {
    Write-Host ".venv not found. Skipping removal." -ForegroundColor Cyan
}

Write-Host "Checking if dev.db exists..." -ForegroundColor Cyan
if (Test-Path -Path ./dev.db) {
    Write-Host "dev.db found. Removing..." -ForegroundColor Cyan
    rm dev.db -Force -Confirm:$false
    Write-Host "dev.db removed." -ForegroundColor Cyan
} else {
    Write-Host "dev.db not found. Skipping removal." -ForegroundColor Cyan
}

Write-Host "Creating a new virtual environment..." -ForegroundColor Cyan
py -3.12 -m venv .venv
Write-Host "Virtual environment created." -ForegroundColor Cyan

Write-Host "Activating virtual environment..." -ForegroundColor Cyan
.\.venv\Scripts\Activate.ps1
Write-Host "Virtual environment activated." -ForegroundColor Cyan

Write-Host "Installing dependencies from requirements.txt..." -ForegroundColor Cyan
pip install -r requirements.txt
Write-Host "Dependencies installed." -ForegroundColor Cyan

Write-Host "Running DevDbSetup.py..." -ForegroundColor Cyan
python DevDbSetup.py
Write-Host "DevDbSetup.py execution completed." -ForegroundColor Cyan