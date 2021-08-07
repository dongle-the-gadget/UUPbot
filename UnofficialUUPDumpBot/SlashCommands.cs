using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using WindowsUpdateLib;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;

namespace UnofficialUUPDumpBot
{
    public class SlashCommands : SlashCommandModule
    {
        static Dictionary<string, CTAC> GetRingCtacs(MachineType arch)
        {
            return new Dictionary<string, CTAC>
            {
                { "Dev", new CTAC(OSSkuId.Professional, "10.0.19041.200", arch, "External", "Dev", "CB", "vb_release", "Production", false, false) },
                { "Beta", new CTAC(OSSkuId.Professional, "10.0.19041.200", arch, "External", "Beta", "CB", "vb_release", "Production", false, false) },
                { "ReleasePreview", new CTAC(OSSkuId.Professional, "10.0.19041.200", arch, "External", "ReleasePreview", "CB", "vb_release", "Production", false, false) },
                { "Retail", new CTAC(OSSkuId.Professional, "10.0.19041.84", arch, "Retail", "", "CB", "vb_release", "Production", false) }
            };
        }

        static UpdateData TrimDeltasFromUpdateData(UpdateData update)
        {
            update.Xml.Files.File = update.Xml.Files.File.Where(x => !x.FileName.Replace('\\', Path.DirectorySeparatorChar).EndsWith(".psf", StringComparison.InvariantCultureIgnoreCase)
            && !x.FileName.Replace('\\', Path.DirectorySeparatorChar).StartsWith("Diff", StringComparison.InvariantCultureIgnoreCase)
             && !x.FileName.Replace('\\', Path.DirectorySeparatorChar).StartsWith("Baseless", StringComparison.InvariantCultureIgnoreCase)).ToArray();
            return update;
        }

        [SlashCommand("list", "List the latest build items belonging to a specific channel")]
        public async Task ListDumpItems(InteractionContext ctx,
            [Option("channel", "The channel to get builds from")]
            [Choice("Dev", "Dev")][Choice("Beta", "Beta")][Choice("ReleasePreview", "ReleasePreview")][Choice("Retail", "Retail")]string branch,
            [Option("arch", "The architecture")]
            [Choice("x86", "x86")][Choice("x64", "amd64")][Choice("arm64", "arm64")]string arch)
        {
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.DeferredChannelMessageWithSource);

            try
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder();

                IEnumerable<UpdateData> data = await FE3Handler.GetUpdates(null, GetRingCtacs(Enum.Parse<MachineType>(arch))[branch], string.Empty, FileExchangeV3UpdateFilter.ProductRelease).ConfigureAwait(false);
                data = data.Select(x => TrimDeltasFromUpdateData(x));

                foreach (UpdateData update in data)
                {
                    string title = update.Xml.LocalizedProperties.Title;

                    string encodedTitle = HttpUtility.UrlEncode(title);

                    HttpClient client = new HttpClient();
                    var res = await client.GetAsync($"https://api.uupdump.net/listid.php?search={encodedTitle}");

                    UUPDumpRes response = JsonSerializer.Deserialize<UUPDumpRes>(await res.Content.ReadAsStringAsync());

                    if (response.response != null)
                    {
                        var item = response.response.builds.Values.First(f => f.arch == arch);

                        embed.AddField(item.title, $"Architecture: {item.arch}\nLink: <https://uupdump.net/selectlang.php?id={item.uuid}>");
                    }
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
        
        [SlashCommand("testup", "Test bot, UUP dump and Microsoft's Windows Updates server status.")]
        public async Task PingAsync(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.DeferredChannelMessageWithSource);
            
            try
            {
                var pingdump = await PingServerAsync("api.uupdump.net", 4000);
                
                DiscordWebhookBuilder builder = new DiscordWebhookBuilder().WithContent($"Pong!\n" +
                    $"**UUP dump API ping status:** {pingdump.Status}\n" +
                    $"**UUP dump API address:** {pingdump.Address}\n" +
                    $"**UUP dump API round trip time:** {pingdump.Status}");
                
                await ctx.EditResponseAsync(builder);
            }
            catch (Exception ex)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Sorry, but the bot encountered an error and cannot continue processing your request."));
                ctx.Client.Logger.LogError(ex.ToString());
            }
        }
        
        private async Task<PingReply> PingServerAsync(string host, int milTimeout)
        {
            Ping ping = new Ping();
            
            return await ping.SendPingAsync(host, milTimeout);
        }
    }
}
