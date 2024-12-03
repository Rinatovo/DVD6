using System;

namespace DVD6
{
    public partial class Retour
    {
        public int LocationId { get; set; }
        public int ClientId { get; set; }
        public int DVDId { get; set; }
        public string ClientName { get; set; }
        public string DVDTitle { get; set; }
        public DateTime DateRented { get; set; }
        public DateTime? DateReturned { get; set; }
    }
}