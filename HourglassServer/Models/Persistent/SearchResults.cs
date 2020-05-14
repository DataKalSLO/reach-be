namespace HourglassServer.Models.Persistent
{
    public partial class SearchResults
    {
        public int hits { get; set; }

        public string id { get; set; }
        
        public string index { get; set; }
        public string title { get; set; } 
    }
}