using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using WindowsUpdateLib;

namespace UnofficialUUPDumpBot
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            DiscordConfiguration config = new() 
            {
                Token = Environment.GetEnvironmentVariable("UUPDUMP_BOTTOKEN"),
                TokenType = TokenType.Bot
            };

            DiscordClient client = new DiscordClient(config);

            var slashCommands = client.UseSlashCommands();

            slashCommands.RegisterCommands<SlashCommands>();

            await client.ConnectAsync(activity: new DiscordActivity("Windows Insider ISOs", ActivityType.Playing), UserStatus.Online);

            await Task.Delay(-1);
        }
    }
}
