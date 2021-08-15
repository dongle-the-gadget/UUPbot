# Running UUP dump bot as a standalone application
If you prefer to have the application run on your local machine natively rather than through a Docker container, you'll need to install the dependencies manually.

## Install .NET
This project uses .NET 5, so you'll need to install the .NET 5 SDK from [Microsoft's website](https://dotnet.microsoft.com/download/dotnet/5.0)

## Building the bot
Run the following command at bot's root folder:
```
dotnet build ./UnofficialUUPDumpBot/UnofficialUUPDumpBot.csproj -c Release
```
Please note that during the build process, warnings may appear. This is totally normal.

## Running the bot
First, Set the `UUPDUMP_BOTTOKEN` environment variable to the bot token you got from Discord Developers Portal.

Run the following command at bot's root folder:
```
dotnet run --project ./UnofficialUUPDumpBot/UnofficialUUPDumpBot.csproj -c Release
```
