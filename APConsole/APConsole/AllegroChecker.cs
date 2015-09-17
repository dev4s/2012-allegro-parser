using System.Collections.Generic;
using System.Linq;
using APConsole.AllegroService;

namespace APConsole
{
	/// <summary>
	/// Klasa "połączeniowa" pomiędzy klasą AllegroClient, a AllegroFile
	/// Żeby nie mieszać filtrowania pobranych danych z zwracaną nową klasą.
	/// </summary>
	public static class AllegroChecker
	{
		public static IEnumerable<AllegroFileResult> FindDuplicatesAndCreateLinks(List<SearchResponseType> foundAuctions, List<AllegroFileResult> earlierFoundAuctions, AllegroFileSearch search)
		{
			//sprawdzanie czy użytkownik nie wystawił ponownie tej samej aukcji
			//sprawdzanie ze wcześniej przeskanowanymi listami
			var tempFilteredList = new List<SearchResponseType>();
			if (earlierFoundAuctions.Any())
			{
				foreach (var earlierFoundAuction in earlierFoundAuctions)
				{
					tempFilteredList.AddRange(foundAuctions.Where(x => x.sitsellerinfo.sellerid == earlierFoundAuction.NickId));
				}
			}
			else
			{
				tempFilteredList.AddRange(foundAuctions);
			}

			//przetwarzanie wbudowanej klasy allegro, na naszą wewnętrzną
			var cleanedList = new List<AllegroFileResult>();
			foreach (var filteredItem in tempFilteredList)
			{
				var newFilteredItem = new AllegroFileResult
				                      	{
				                      		Link = CreateLink(filteredItem.sitid),
				                      		Nick = filteredItem.sitsellerinfo.sellername,
				                      		NickId = filteredItem.sitsellerinfo.sellerid,
				                      		FoundTitle = filteredItem.sitname,
				                      		Title = search.Title,
				                      		Price = search.Price,
				                      		Comment = search.Comment
				                      	};

				//typy aukcji:
				//- tylko licytacja
				//- tylko kup teraz
				//- licytacja i kup teraz
				var auction = Auction(newFilteredItem, filteredItem);
				if (auction != null) cleanedList.Add(auction);

				var buyItNow = BuyItNow(newFilteredItem, filteredItem);
				if (buyItNow != null) cleanedList.Add(buyItNow);
			}

			return cleanedList;
		}

		private static AllegroFileResult BuyItNow(AllegroFileResult newFilteredItem, SearchResponseType filteredItem)
		{
			if (filteredItem.sitisbuynow == 1)
			{
				newFilteredItem.FoundPrice = (decimal)filteredItem.sitbuynowprice;
				newFilteredItem.Type = AuctionType.BuyItNow;

				return newFilteredItem;
			}
			return null;
		}

		private static AllegroFileResult Auction(AllegroFileResult newFilteredItem, SearchResponseType filteredItem)
		{
			if ((decimal) filteredItem.sitprice != 0)
			{
				newFilteredItem.FoundPrice = (decimal)filteredItem.sitprice;
				newFilteredItem.Type = AuctionType.Auction;

				return newFilteredItem;
			}

			return null;
		}

		private static string CreateLink(long auctionId)
		{
			return string.Format("http://allegro.pl/i{0}.html", auctionId);
		}
	}
}