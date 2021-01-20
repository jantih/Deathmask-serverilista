using System.Collections.Generic;

namespace Serverilista
{
    class Suodata
    {
        // Json suodatus
        public string hostname { get; set; }
        public string gametype { get; set; }
        public string map { get; set; }
        public int numplayers { get; set; }
        public int maxplayers { get; set; }
        public int numspectators { get; set; }
        public int bots { get; set; }
        public List<Players> players { get; set; }
    }
    // Alemman tason tietueen hakeminen
    public class Players 
    {
        public string name { get; set; }
        public int score { get; set; }
        public int team { get; set; }
        public int ping { get; set; }
    }
}

