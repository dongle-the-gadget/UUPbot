# UUP Dump bot
Unofficial UUP dump bot ([UUP dump website](https://uupdump.net)) for Discord

## Functionality
This bot can search through MS servers, find the latest build for a specific branch and get the UUP dump link for it.

## Running
This bot is still an early experiment, so I didn't add any failsafe (so if bot failed to contact servers at any point, it will crash),
but if you are fine with it, here's how (please note that you'll need .NET 6 preview SDK for this):

### Getting a token and add the bot to your server

  1. Create an application on https://discord.com/developers
  2. Go to the OAuth tab and select `application.commands`
  3. Copy the generated OAuth link and navigate to that link on a web browser
  4. Use the on-screen instructions to add the app to your server
  5. Go to the Bot screen and choose to create a bot account, confirm if asked.
  6. Click on Copy under "Token" to copy the Token, we'll need this later
  
### Get the server ID
  1. Open Settings in Discord, go to Advanced and enable Developer Mode
  2. Right click on the server you added the bot into in step 1 and select "Copy ID"
  
### Modify the source code and run it

  1. Open the Program.cs file in UnofficialUUPDumpBot folde
  2. Add the Token to the `[yours]` field at `Token = "[yours]"`
  3. Replace the long number at `RegisterCommands` with your server ID.
  4. Build the bot and run! It should show you DSharpPus version ... without any errors.

### Testing

Just type a forward-slash to see all commands that this bot offers (currently only `list`)
