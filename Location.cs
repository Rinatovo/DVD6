namespace DVD6
{
    public class ClientLocation
    {
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class DVDLocation
    {
        public int DVDId { get; set; }
        public string Title { get; set; }
        public int IsAvailable { get; set; }
    }

}
