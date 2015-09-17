namespace APConsole
{
	public enum AuctionType
	{
		Auction = 'a',
		BuyItNow = 'k'
	}

	public abstract class AllegroFileBase
	{
		public int ID { get; set; }
		public string Title { get; set; }
		public decimal Price { get; set; }
		public string Comment { get; set; }
	}

	public class AllegroFileResult : AllegroFileBase
	{
		public decimal FoundPrice { get; set; }
		public string FoundTitle { get; set; }
		public AuctionType Type { get; set; }
		public string Link { get; set; }
		public string Nick { get; set; }
		public int NickId { get; set; }
	}

	public class AllegroFileSearch : AllegroFileBase
	{
		public string Category { get; set; }
	}
}