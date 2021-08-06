# Running the UUP dump bot

Please note that these steps require you to have "Manage Server" permission on the server you wish to invite the bot to.

## The easy and lazy way
Click on [this link](https://discord.com/api/oauth2/authorize?client_id=872508836194447370&permissions=0&scope=bot%20applications.commands) and invite my premade bot to your server.

## The hard but customizable (and maybe more reliable) way

### Create the bot and invite it to your own server
  1. Go to [Discord Developers Portal](https://discord.com/developers), and create a new application.
  2. Go to the OAuth tab, check `applications.commands` and copy the generated OAuth link and paste it to a separate browser tab.
  3. Follow the instructions on screen to invite the bot to your desired server.

### Get the authentication token
  1. In the Discord Developers Portal, go to the Bot tab and select "Create a bot", confirm if asked
  2. Under Token, there should be two buttons: "Copy" and "Regenerate", select "Copy" and paste it somewhere safe, we will need it later.

WARNING: Please keep the Token safe since anyone with the token can perform actions under your bot's credentials. If you found your token stolen, select Regenerate to invalidate the stolen one and get a new token.

### Deploy the bot
Clone the repository and choose one of the following deployment options:
  1. [Docker](/docs/running/docker.md)
  2. [Standalone .NET application](/docs/running/standalone.md)
