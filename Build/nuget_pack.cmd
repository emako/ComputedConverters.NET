cd /d %~dp0
cd /d ..\ComputedAnimations.WPF
dotnet restore
dotnet build -c Release
dotnet pack -c Release -o ../Build/
cd /d %~dp0
cd /d ..\ComputedBehaviors.WPF
dotnet restore
dotnet build -c Release
dotnet pack -c Release -o ../Build/
cd /d %~dp0
cd /d ..\ComputedConverters.WPF
dotnet restore
dotnet build -c Release
dotnet pack -c Release -o ../Build/
@pause
