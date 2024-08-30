# Define environment variables or use known paths
$serviceExePath = "C:\Users\LENOVO\source\repos\WindowsServiceC\WindowsServiceC\bin\Release\WindowsServiceC.exe"
$serviceName = "YWindowsServiceC"
$serviceDescription = "My Windows Service Description WindowsServiceC"
$logFilePath = "C:\Users\LENOVO\source\repos\WindowsServiceC\LogFile.txt"

# Function to check if running as administrator
function Test-Administrator {
    $currentUser = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
    return $currentUser.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}

# Elevate script if not running as administrator
if (-not (Test-Administrator)) {
    Start-Process powershell -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`"" -Verb RunAs
    exit
}

# Clear the log file at the start of the script
Clear-Content -Path $logFilePath

# Log function to output to both console and file
function Write-Log {
    param (
        [string]$message
    )
    Write-Output $message
    $message | Out-File -FilePath $logFilePath -Append
}

if (Get-Service -Name $serviceName -ErrorAction SilentlyContinue) {
    Write-Log "Service already installed."
} else {
    New-Service -Name $serviceName -BinaryPathName $serviceExePath -DisplayName $serviceName -Description $serviceDescription -StartupType Automatic
    Start-Service -Name $serviceName
    Write-Log "Service installed and started."
}
