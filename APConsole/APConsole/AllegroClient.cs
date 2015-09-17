using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using APConsole.AllegroService;

namespace APConsole
{
	public class AllegroClient
	{
		public static string WebApiKey { private get; set; }

		private readonly string _username;
		private readonly string _password;
		private readonly string _sessionKey;
		private readonly AllegroWebApiPortTypeClient _asvc;
		private readonly List<CatInfoType> _categories;

		private const int CountryCode = 1; //Polska
		private const int WebApiCode = 1;

		//temporaries (and not needed)
		private long _tempKeyVersion;
		private string _tempStringVersion;
		private long _tempUserLoggedId;
		private long _tempServerTime;
		private int _tempCountFeaturedAuctions;
		private string[] _tempExcludedWords; //chyba niepotrzebne...

		public AllegroClient(string username, string password, bool checkings)
		{
			_username = username;
			_password = password;

			_asvc = new AllegroWebApiPortTypeClient();
			_asvc.Open();

			//sprawdzanie klucza wersji
			var webApiKeyVersion = GetWebApiKeyVersion();

			if (checkings) return;
			//logowanie do allegro (pobieranie
			_sessionKey = GetSessionKeyForLoggedUser(_username, _password, webApiKeyVersion);

			//pobieranie wszystkich kategorii
			_categories = GetAllCategories();
		}

		/// <summary>
		/// Pobieranie wszystkich kategorii przedmiotów z Allegro
		/// </summary>
		/// <returns>Lista kategorii</returns>
		private List<CatInfoType> GetAllCategories()
		{
			//TODO: Exceptions
			//TODO: Cache'owanie
			return new List<CatInfoType>(_asvc.doGetCatsData(out _tempKeyVersion, out _tempStringVersion, 
															CountryCode, 0, WebApiKey));
		}

		/// <summary>
		/// Pobieranie klucza sesji dla zalogowanego użytkownika, potrzebne do doSearch (i innych)
		/// </summary>
		/// <param name="username">Login użytkownika</param>
		/// <param name="password">Hasło użytkownika</param>
		/// <param name="webApiKeyVersion">Aktualny klucz wersji (dynamicznie zmieniający się)</param>
		/// <returns>Klucz sesji</returns>
		private string GetSessionKeyForLoggedUser(string username, string password, long webApiKeyVersion)
		{
			return _asvc.doLogin(out _tempUserLoggedId, out _tempServerTime, 
								username, password, CountryCode, WebApiKey, webApiKeyVersion);
		}

		/// <summary>
		/// Pobieranie aktualnego klucza wersji (dynamicznie zmieniający się)
		/// </summary>
		/// <returns>Aktualny klucz wersji</returns>
		private long GetWebApiKeyVersion()
		{
			long componentKeyVersion;
			_asvc.doQuerySysStatus(out componentKeyVersion, WebApiCode, CountryCode, WebApiKey);

			return componentKeyVersion;
		}

		/// <summary>
		/// Wyszukiwanie
		/// </summary>
		/// <param name="searchQuery">Szukane słowa</param>
		/// <param name="category">Kategoria, w której będzie wykonywane przeszukiwanie</param>
		/// <param name="price">Szukana cena</param>
		public List<SearchResponseType> Search(string searchQuery, string category, decimal price)
		{
			//TODO: check connection open state (on every search)
			//TODO: check session key (or refresh it by default, by every 30 minutes)
			if (_asvc.State != CommunicationState.Opened) _asvc.Open();

			//może być, że więcej kategorii spełnia wymogi wyszukiwania
			//dlatego niżej jest wprowadzone wyszukiwanie dla większej ilości
			//przeszukiwanych kategorii
			var foundCategories = _categories.Where(x => ReplacePolishChars(x.catname.ToLower()).Contains(ReplacePolishChars(category.ToLower())));

			var foundAuctions = new List<SearchResponseType>();

			foreach (var foundCategory in foundCategories)
			{
				for (var i = 0; i < 50; i++)	//doSearch zwraca tylko i wyłącznie 50 pierwszych wyników, przypuszczamy, że
												//maksymalnie będzie 50 "stron" zwracanych
				{
					SearchResponseType[] tempFoundAuctions;

					var allegroSearchQuery = new SearchOptType
					                         	{
					                         		searchcategory = foundCategory.catid,
					                         		searchstring = searchQuery,
					                         		searchpriceto = (float) price,
					                         		searchoffset = i,
													searchlimit = 100,	//maksymalna ilość pobieranych danych (od 0 do 100)
													searchorder = 4,	//sortowanie wyniku po cenie przedmiotu
													searchordertype = 0	//sortowanie rosnąco (domyślnie, ale wolę wprowadzić)
					                         	};

					var countFoundAuctions = _asvc.doSearch(out _tempCountFeaturedAuctions, out tempFoundAuctions, 
															out _tempExcludedWords, _sessionKey, allegroSearchQuery);

					foundAuctions.AddRange(tempFoundAuctions);

					if (countFoundAuctions < 50)	//jeżeli aukcji będzie mniej niż 50, przerywa działanie
					{
						break;
					}
				}

				Thread.Sleep(new Random().Next(100, 600));	//usypianie wyszukiwania (w celu uniknięcia blokady)
			}

			return foundAuctions;
		}

		private static string ReplacePolishChars(string categoryName)
		{
			var forbiddenChars =	new[] {'ą', 'ć', 'ę', 'ł', 'ń', 'ó', 'ś', 'ź', 'ż'};
			var notForbiddenChars = new[] {'a', 'c', 'e', 'l', 'n', 'o', 's', 'z', 'z' };

			for (var i = 0; i < forbiddenChars.Length; i++)
			{
				categoryName = categoryName.Replace(forbiddenChars[i], notForbiddenChars[i]);
			}

			return categoryName;
		}
	}
}