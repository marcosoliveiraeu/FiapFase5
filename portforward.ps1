# portforward.ps1
$ns = "fiapfase5"

Start-Process powershell -ArgumentList "-NoExit","-Command","kubectl port-forward -n $ns svc/identity 5001:80"
Start-Process powershell -ArgumentList "-NoExit","-Command","kubectl port-forward -n $ns svc/farm 5002:80"
Start-Process powershell -ArgumentList "-NoExit","-Command","kubectl port-forward -n $ns svc/telemetry 5003:80"
Start-Process powershell -ArgumentList "-NoExit","-Command","kubectl port-forward -n $ns svc/ingestion 5004:80"