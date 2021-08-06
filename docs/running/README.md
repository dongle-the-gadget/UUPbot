# Running the UUP dump bot

Please note that these steps require you to have "Manage Server" permission on the server you wish to invite the bot to.

## Pre deployed bot
Click on [this link](https://discord.com/api/oauth2/authorize?client_id=872508836194447370&permissions=0&scope=bot%20applications.commands) and invite my premade bot to your server.

## Premade binaries

### Create the bot and invite it to your own server
  1. Go to [Discord Developers Portal](https://discord.com/developers), and create a new application.
  2. Go to the OAuth tab, check `applications.commands` and `bot` and copy the generated OAuth link and paste it to a separate browser tab.
  3. Follow the instructions on screen to invite the bot to your desired server.

### Get the authentication token
  1. In the Discord Developers Portal, go to the Bot tab and select "Create a bot", confirm if asked
  2. Under Token, there should be two buttons: "Copy" and "Regenerate", select "Copy" and paste it somewhere safe, we will need it later.

WARNING: Please keep the Token safe since anyone with the token can perform actions under your bot's credentials. If you found your token stolen, select Regenerate to invalidate the stolen one and get a new token.

### Get the binaries

You can get the premade binaries at [releases](https://github.com/superkid200/UUPdumpbot/releases/latest). They are self-contained binaries so there's no need to install .NET 6 Runtime.

#### Windows binaries

These binaries won't work on Windows 7.

Architecture | File name
-------------|---------------------------------------
x86          | UUPbot-win-x86.exe
x64          | UUPbot-win-x64.exe
ARM32        | UUPbot-win-arm32.exe
ARM64        | UUPbot-win-arm64.exe

#### macOS binaries

Minimum required version | Architecture          | File name
-------------------------|-----------------------|----------------------
10.12 (Sierra)           | x64 (Intel)           | UUPbot-macos-x64
11.01 (Big Sur)          | ARM64 (Apple Silicon) | UUPbot-macos-arm64

#### Linux binaries

Distribution (distro)                                     | Architecture | File name
----------------------------------------------------------|--------------|-------------------
x64-based CentOS, Debian, Fedora, Ubuntu, and derivatives | x64          | UUPbot-linux-x64
x64-based Lightweight musl-based (i.e. Alphine)           | x64          | UUPbot-linux-musl-x64
ARM32-based Linux distros                                 | ARM32        | UUPbot-linux-arm32
ARM64-based Linux distros                                 | ARM64        | UUPbot-linux-arm64

### Run the binaries

Before running, set the `UUPDUMP_BOTTOKEN` environment variable to the bot token you got from Discord Developers Portal.

Then you can execute the bot executable and the bot should be online!

## Building your own binaries

Follow the steps in **Premade binaries** to invite your bot and get the authentication token.

### Deploy the bot
Clone the repository and choose one of the following deployment options:
  1. [Docker](/docs/running/docker.md)
  2. [Standalone .NET application](/docs/running/standalone.md)
