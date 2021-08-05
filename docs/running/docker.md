# Running UUP dump bot on a Docker container

## Install Docker
This step varies from an OS to another, on Linux it should be as easy as running two commands.
```
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh
```

## Creating a Docker image
To create a docker image, run the following command:
```
docker build -t uupdumpbot .
```

## Running the bot
If you have a Docker-compatible hosting service, use their documentation to host your bot. (Please note that you need to set the environment variable UUPDUMP_BOTTOKEN to the bot token you got from Discord Developers Portal)

If you prefer to run the bot on your local machine, create an `.env` file on the bot root folder with the following content (replace `[your bot token here]` with the bot token):
```
UUPDUMP_BOTTOKEN=[your bot token here]
```

Then run the following command on the bot root folder:
```
docker run -d --env-file .env --name [container name] uupdumpbot
```
