using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Serverilista
{
    class Program
    {
        readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            Program program = new Program();
            await program.HaeSivu();
        }
        private async Task HaeSivu()
        {
            // Hae JSON - vastaus sivulta ja tulosta koko mälli
            string vastaus = await client.GetStringAsync("http://dpmaster.deathmask.net/?game=warfork&json=1");
            Console.WriteLine(vastaus);

            // Suodata koko paketti osiksi
            List<Suodata> suodata = JsonConvert.DeserializeObject<List<Suodata>>(vastaus);

            foreach(var osa in suodata)
            {
                // suodata tyhjät serverit
                if (osa.numplayers == 0)
                {
                    continue;
                }
                // Tulosta jokaisesta serveristä haluttavat tiedot
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n{osa.hostname}:\nModi: {osa.gametype}\nKartta: {osa.map}\n" +
                    $"Pelaajia: {osa.numplayers}/{osa.maxplayers}\nBotteja: {osa.bots}\n");
                
                // Tulosta json syvemmän nestauksen tietoja, tässä tapauksessa pelaajat
                foreach(var plr in osa.players)
                {
                    // Suodata pelaajien nimestä turha sälä
                    plr.name = plr.name
                        .Replace("&lt;", "")
                        .Replace("/font&gt;", "")
                        .Replace("font color*&gt;", "")
                        .Replace("font color=\"#FFFFFF\"&gt;", "")
                        .Replace("font color=\"#000000\"&gt;", "")
                        .Replace("font color=\"#FF0000\"&gt;", "")
                        .Replace("font color=\"#00FF00\"&gt;", "")
                        .Replace("font color=\"#0000FF\"&gt;", "")
                        .Replace("font color=\"#FF00FF\"&gt;", "")
                        .Replace("font color=\"#FFFF00\"&gt;", "")
                        .Replace("font color=\"#00FFFF\"&gt;", "");

                    if (plr.name.Length < 7)
                    {
                        System.Text.StringBuilder pName = new System.Text.StringBuilder();
                        pName.Append(plr.name);
                        pName.Append("\t\t");
                        plr.name = pName.ToString();
                    }
                    else if (plr.name.Length < 14)
                    {
                        System.Text.StringBuilder pName = new System.Text.StringBuilder();
                        pName.Append(plr.name);
                        pName.Append("\t");
                        plr.name = pName.ToString();
                    }

                    Console.ResetColor();
                    /*Console.WriteLine($"Pelaajan nimi: {plr.name}\nPisteet: {plr.score}\nTiimi: {plr.team}\n" +
                        $"Latenssi: {plr.ping}\n");*/
                    Console.WriteLine($"Pelaaja: {plr.name}\tLatenssi: {plr.ping}");
                }
            }
        }
    }
}
