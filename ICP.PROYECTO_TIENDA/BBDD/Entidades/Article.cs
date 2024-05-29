namespace Icp.TiendaApi.BBDD.Modelos
{
    public partial class Article
    {
        public Article()
        {
            Items = new HashSet<Item>();
            Stocks = new HashSet<Stock>();
        }

        public int IdArticle { get; set; }
        public string Description { get; set; }
        public string Maker { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Price { get; set; }
        public string Foto { get; set; }
        public string Status { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<Stock> Stocks { get; set; }
    }
}
