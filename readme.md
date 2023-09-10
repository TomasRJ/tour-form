### Noter
I Linux kør følgende for at få hot-reload til at virke:

```
export DOTNET_ROOT=/usr/share/dotnet/
```

```
dotnet watch
```

## Krav
1. .NET 6 SDK. I Windows 10/11 skriv `winget install Microsoft.DotNet.SDK.6` i terminalen for en nem installering af .NET 6 SDK.

## Brug af kode
1. Kør `tour-form/webform` med `dotnet watch` (eller `dotnet run` hvis man ikke behøver hot-reload), hvis man bruger Linux kig på noten ovenover.
2. Kør `tour-form/email-service` med `dotnet run`.
3. Kør `tour-form/back-office` med `dotnet run`

TLDR:
1. ```
    cd webform
    dotnet watch
    ```
2. ```
    cd email-service
    dotnet run
    ```
3. ```
    cd back-office
    dotnet run
    ```
