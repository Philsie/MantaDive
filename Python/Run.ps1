if(Test-Path -Path ./.venv){
	.\.venv\Scripts\Activate.ps1
} else {
    ./Setup.ps1
}
if(Test-Path -Path ./dev.db){
    
} else {
    python DevDbSetup.py
}
python Backend.py 