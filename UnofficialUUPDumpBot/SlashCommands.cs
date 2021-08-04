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

        [SlashCommand("list", "List builds belonging to a specific channel")]
        public async Task ListDumpItems(InteractionContext ctx,
            [Option("channel", "Get the channel to get builds from")]
            [Choice("Dev", "Dev")][Choice("Beta", "Beta")][Choice("ReleasePreview", "ReleasePreview")][Choice("Retail", "Retail")]string branch)
        {
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.DeferredChannelMessageWithSource);

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();

            IEnumerable<UpdateData> data = await FE3Handler.GetUpdates(null, GetRingCtacs(MachineType.amd64)[branch], string.Empty, FileExchangeV3UpdateFilter.ProductRelease).ConfigureAwait(false);
            data = data.Select(x => TrimDeltasFromUpdateData(x));

            foreach (UpdateData update in data)
            {
                string title = update.Xml.LocalizedProperties.Title;

                if (title.ToLowerInvariant().Contains("cumulative update"))
                {
                    continue;
                }

                string encodedTitle = HttpUtility.UrlEncode(title);

                HttpClient client = new HttpClient();
                var res = await client.GetAsync($"https://api.uupdump.net/listid.php?search={encodedTitle}");


                UUPDumpRes response = JsonSerializer.Deserialize<UUPDumpRes>(await res.Content.ReadAsStringAsync());

                var item = response.response.builds.Values.OrderByDescending(f => f.title).ElementAt(0);

                embed.AddField(item.title, $"Architecture: {item.arch}\nLink: <https://uupdump.net/selectlang.php?id={item.uuid}>");
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed).WithContent("Here are the items I've found matching your criteria."));
        }
    }
}
