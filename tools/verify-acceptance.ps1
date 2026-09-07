<#
.SYNOPSIS
Runs the LTS acceptance verification checks and reports pass/fail.

.DESCRIPTION
Runs format, build, test, container config, container build, and container smoke checks.
Exits non-zero if any check fails. See docs/acceptance-criteria.md for the acceptance contract.

.PARAMETER SkipContainer
Skips the container config/build/smoke checks (useful when Docker is unavailable).

.EXAMPLE
powershell -ExecutionPolicy Bypass -File tools/verify-acceptance.ps1
#>
[CmdletBinding()]
param(
    [switch]$SkipContainer
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
Set-Location $root

$results = [System.Collections.Generic.List[object]]::new()

function Invoke-Check {
    param(
        [string]$Name,
        [scriptblock]$Body
    )
    Write-Host "== $Name ==" -ForegroundColor Cyan
    try {
        & $Body
        $results.Add([pscustomobject]@{ Name = $Name; Result = 'Pass' })
        Write-Host "PASS: $Name" -ForegroundColor Green
    }
    catch {
        $results.Add([pscustomobject]@{ Name = $Name; Result = 'Fail' })
        Write-Host "FAIL: $Name - $($_.Exception.Message)" -ForegroundColor Red
    }
}

Invoke-Check 'Format' {
    dotnet format Legislature.TrackingSystem.sln --verify-no-changes --no-restore
    if ($LASTEXITCODE -ne 0) { throw "dotnet format failed with exit code $LASTEXITCODE" }
}

Invoke-Check 'Build' {
    dotnet build Legislature.TrackingSystem.sln --configuration Release -m:1
    if ($LASTEXITCODE -ne 0) { throw "dotnet build failed with exit code $LASTEXITCODE" }
}

Invoke-Check 'Test' {
    dotnet test Legislature.TrackingSystem.sln --configuration Release --no-build -m:1
    if ($LASTEXITCODE -ne 0) { throw "dotnet test failed with exit code $LASTEXITCODE" }
}

if (-not $SkipContainer) {
    Invoke-Check 'Container config' {
        docker compose config
        if ($LASTEXITCODE -ne 0) { throw "docker compose config failed with exit code $LASTEXITCODE" }
    }

    Invoke-Check 'Container build' {
        docker compose build web
        if ($LASTEXITCODE -ne 0) { throw "docker compose build failed with exit code $LASTEXITCODE" }
    }

    Invoke-Check 'Container smoke' {
        docker compose up -d --force-recreate web
        if ($LASTEXITCODE -ne 0) { throw "docker compose up failed with exit code $LASTEXITCODE" }
        Start-Sleep -Seconds 8
        $base = 'http://localhost:5088'
        foreach ($path in @('/', '/health', '/health/ready')) {
            $r = Invoke-WebRequest -Uri ($base + $path) -UseBasicParsing -TimeoutSec 15
            if ($r.StatusCode -ne 200) { throw "GET $path returned $($r.StatusCode)" }
        }
        $token = Invoke-RestMethod -Uri ($base + '/api/v1/auth/token') -Method Post -ContentType 'application/json' -Body '{"userKey":"admin"}' -TimeoutSec 15
        if (-not $token.token) { throw "token endpoint returned no token" }
    }
}

Write-Host "`n=== Acceptance Summary ===" -ForegroundColor Cyan
$failed = $results | Where-Object { $_.Result -eq 'Fail' }
foreach ($r in $results) {
    $color = if ($r.Result -eq 'Pass') { 'Green' } else { 'Red' }
    Write-Host ("{0,-20} {1}" -f $r.Name, $r.Result) -ForegroundColor $color
}

if ($failed.Count -gt 0) {
    Write-Host "`nAcceptance FAILED: $($failed.Count) check(s) failed." -ForegroundColor Red
    exit 1
}
Write-Host "`nAcceptance PASSED." -ForegroundColor Green
exit 0
