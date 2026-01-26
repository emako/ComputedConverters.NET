Set-Location $PSScriptRoot

$projects = @(
    "..\ComputedAnimations.WPF",
    "..\ComputedBehaviors.WPF",
    "..\ComputedConverters.WPF"
)

foreach ($proj in $projects) {
    Push-Location $proj
    Write-Host "Processing $proj..."
    dotnet restore
    dotnet build -c Release
    dotnet pack -c Release -o ../Build/
    Pop-Location
}

Pause
