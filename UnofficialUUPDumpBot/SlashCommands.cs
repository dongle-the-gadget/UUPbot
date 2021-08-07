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
using Microsoft.Extensions.Logging;

namespace UnofficialUUPDumpBot
{
    public class SlashCommands : SlashCommandModule
    {

        [SlashCommand("list", "List the latest build items belonging to a specific channel")]
        public async Task ListDumpItems(InteractionContext ctx,
            [Option("channel", "The channel to get builds from")]
            [Choice("Dev", "WIF")][Choice("Beta", "WIS")][Choice("ReleasePreview", "rp")][Choice("Retail", "retail")]string branch,
            [Option("arch", "The architecture")]
            [Choice("x86", "x86")][Choice("x64", "amd64")][Choice("ARM64", "arm64")]string arch)
        {
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.DeferredChannelMessageWithSource);

            try
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder();

                HttpClient client = new HttpClient();

                HttpResponseMessage httpResponse = await client.GetAsync($"https://api.uupdump.net/fetchupd.php?ring={branch}&arch={arch}");

                string resJson = await httpResponse.Content.ReadAsStringAsync();

                UUPDumpRes3 res = JsonSerializer.Deserialize<UUPDumpRes3>(resJson);

                foreach (var item in res.response.updateArray)
                {
                    embed.AddField(item.updateTitle, $"Architecture: {item.arch}\n" +
                        $"Link: <https://uupdump.net/selectlang.php?id={item.updateId}>");
                }

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed).WithContent("Here is the latest build items I've found matching your criteria."));
            }
            catch (Exception ex)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Sorry, but the bot encountered an error and cannot continue processing your request."));
                ctx.Client.Logger.LogError(ex.ToString());
            }
        }

        [SlashCommand("search", "Lists the top three results matching your query")]
        public async Task Search(InteractionContext ctx, [Option("query", "The query to search for")]string query)
        {
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.DeferredChannelMessageWithSource);

            try
            {
                string encodedQuery = HttpUtility.UrlEncode(query);

                HttpClient client = new HttpClient();
                var res = await client.GetAsync($"https://api.uupdump.net/listid.php?search={encodedQuery}");

                string resJson = await res.Content.ReadAsStringAsync();

                if (resJson.Contains("SEARCH_NO_RESULTS", StringComparison.InvariantCultureIgnoreCase))
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Sorry, but the servers returned no results."));
                }
                else
                {
                    DiscordEmbedBuilder embed = new DiscordEmbedBuilder();

                    try
                    {
                        UUPDumpRes response = JsonSerializer.Deserialize<UUPDumpRes>(await res.Content.ReadAsStringAsync());

                        foreach (var item in response.response.builds.Values.Take(3))
                        {
                            embed.AddField(item.title, $"Architecture: {item.arch}\nLink: <https://uupdump.net/selectlang.php?id={item.uuid}>");
                        }

                        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed).WithContent("Here are the top three results I've found."));
                    }
                    catch
                    {
                        UUPDumpRes2 response = JsonSerializer.Deserialize<UUPDumpRes2>(await res.Content.ReadAsStringAsync());

                        foreach (var item in response.response.builds.Take(3))
                        {
                            embed.AddField(item.title, $"Architecture: {item.arch}\nLink: <https://uupdump.net/selectlang.php?id={item.uuid}>");
                        }

                        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed).WithContent("Here are the top three results I've found."));
                    }
                }
            }
            catch (Exception ex)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Sorry, but the bot encountered an error and cannot continue processing your request."));
                ctx.Client.Logger.LogError(ex.ToString());
            }
        }
    }
}
