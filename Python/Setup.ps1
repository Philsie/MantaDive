if(Test-Path -Path ./.venv){
	rm .venv -Recurse -Force -Confirm:$false
}
python -m venv .venv
.\.venv\Scripts\Activate.ps1
pip install -r requirements.txt
python DevDbSetup.py
