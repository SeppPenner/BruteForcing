dotnet nuget push "src\BruteForcing\bin\Release\HaemmerElectronics.SeppPenner.BruteForcing.*.nupkg" -s "github" --skip-duplicate
dotnet nuget push "src\BruteForcing\bin\Release\HaemmerElectronics.SeppPenner.BruteForcing.*.nupkg" -s "nuget.org" --skip-duplicate -k "%NUGET_API_KEY%"
PAUSE