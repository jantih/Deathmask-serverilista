using System;
using System.Collections.Generic;
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
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                if (osa.gametype.Length < 7)
                {
                    System.Text.StringBuilder pName = new System.Text.StringBuilder();
                    pName.Append(osa.gametype);
                    pName.Append("\t");
                    osa.gametype = pName.ToString();
                }
                Console.WriteLine($"\n{osa.hostname}:\nModi: {osa.gametype}\t\tKartta: {osa.map}\n" +
                    $"Pelaajia: {osa.numplayers}/{osa.maxplayers}\t\t\tBotteja: {osa.bots}");
                
                // Hae läpi tietueen pelaajat
                foreach(var plr in osa.players)
                {
                    // Suodata pelaajien nimestä turha sälä
                    plr.name = plr.name.Replace("&lt;", "")     // Tämän tilalle voisi tällätä oman aliohjelmansa van ei se halunna toimia vielä..
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
                    else if (plr.name.Length <= 15)
                    {
                        System.Text.StringBuilder pName = new System.Text.StringBuilder();
                        pName.Append(plr.name);
                        pName.Append("\t");
                        plr.name = pName.ToString();
                    }

                    // Pelaajan tiimin muuttaminen tekstimuotoon
                    string pTeam = "";
                    if (plr.team == 1)
                        pTeam = "NEUTRAALI";
                    else if (plr.team == 2)
                        pTeam = "TIIMI A";
                    else if (plr.team == 3)
                        pTeam = "TIIMI B";
                    else
                        pTeam = "KATSELEE";

                    // Tulosta pelaajat ja heidän tiedot
                    Console.ResetColor();
                    Console.WriteLine($"Pelaaja: {plr.name}\tLatenssi: {plr.ping}\t\t{pTeam}");
                }
                Console.WriteLine("");
            }
        }
        /* public static string Name(string name)
         {
             string placeholder = name;
             placeholder.Replace("&lt;", "")
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

             if (placeholder.Length < 7)
             {
                 System.Text.StringBuilder pName = new System.Text.StringBuilder();
                 pName.Append(placeholder);
                 pName.Append("\t\t");
                 placeholder = pName.ToString();
             }
             else if (placeholder.Length < 14)
             {
                 System.Text.StringBuilder pName = new System.Text.StringBuilder();
                 pName.Append(placeholder);
                 pName.Append("\t");
                 placeholder = pName.ToString();
             }
             return placeholder;
         }*/
    }
}
