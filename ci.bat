powershell.exe -NoProfile -ExecutionPolicy unrestricted -Command "& {  Import-Module '.\src\packages\psake.4.4.1\tools\psake.psm1'; Invoke-psake '.\default.ps1' ci; if ($lastexitcode -ne 0) {write-host "ERROR: $lastexitcode" -fore RED; exit $lastexitcode} }" 