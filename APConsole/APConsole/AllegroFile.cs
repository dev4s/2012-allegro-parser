using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace APConsole
{
	public class AllegroFile
	{
		//TODO: regex problem... very difficult problem...
		private readonly Regex _commentRegex = new Regex(@"(?:.*###.*|\x22?[Ll][Pp].*)", RegexOptions.Multiline);
		private readonly Regex _searchRegex = new Regex(@"(?<ID>\d+)(?:;\x22?)((?<Title>.*)\x22|(?<Title>.*))(?:;)(?<Price>\d+(?:.?|,?)\d*)(?:;)(?:;|\d+(?:.?|,?)\d*;)(?:\x22?)((?<Category>.*)\x22|(?<Category>.*))(?:;\x22?)((?<Comment>.*)\x22|(?<Comment>.*))", RegexOptions.Multiline);
		private readonly Regex _resultsRegex = new Regex(@"(?<ID>\d+)(?:;\x22?)((?<Title>.*)\x22|(?<Title>.*))(?:;\x22?)((?<FoundTitle>.*)\x22|(?<FoundTitle>.*))(?:;)(?<Price>\d+(?:.?|,?)\d*)(?:;)(?<FoundPrice>\d+(?:.?|,?)\d*)(?:;\x22?)(?<Type>.)(?:\x22?;\x22?)((?<Link>.*)\x22|(?<Link>.*))(?:;\x22?)(?<NickId>\d+)(?:;\x22?)((?<Nick>.*)\x22|(?<Nick>.*))(?:;\x22?)((?<Comment>.*)\x22|(?<Comment>.*))", RegexOptions.Multiline);

		private readonly StreamReader _filePathIn;
		private readonly StreamReader _filePathOut;
		private readonly string _resultPath;

		public AllegroFile(string filePathIn, string filePathOut)
		{
			_filePathIn = Read(filePathIn);
			_filePathOut = Read(filePathOut);
			_resultPath = filePathOut;
		}

		private static StreamReader Read(string filePath)
		{
			return new StreamReader(filePath);
		}

		private static StreamWriter Write(string filePath)
		{
			return new StreamWriter(filePath);
		}

		/// <summary>
		/// Zwraca listę z elementami do wyszukania
		/// </summary>
		/// <returns>Lista z elementami do wyszukania</returns>
		public IEnumerable<AllegroFileSearch> GetDatabase()
		{
			return (List<AllegroFileSearch>) GetInformationFromStream(new List<AllegroFileSearch>(), _filePathIn);
		}

		/// <summary>
		/// Zwraca listę wcześniej wyszukanych aukcji
		/// </summary>
		/// <returns>Lista wyszukanych wcześniej aukcji</returns>
		public List<AllegroFileResult> GetResults()
		{
			return (List<AllegroFileResult>) GetInformationFromStream(new List<AllegroFileResult>(), _filePathOut);
		}

		private IEnumerable<dynamic> GetInformationFromStream<T>(IEnumerable<T> list, StreamReader stream) 
			where T : AllegroFileBase
		{
			#region simple reflection
			dynamic tempList;
			Regex regex;

			if (list is List<AllegroFileResult>)
			{
				tempList = (List<AllegroFileResult>) list;
				regex = _resultsRegex;
			}
			else if(list is List<AllegroFileSearch>)
			{
				tempList = (List<AllegroFileSearch>) list;
				regex = _searchRegex;
			}
			else
			{
				throw new ArgumentException("Bad Conversion. Is this list based on AllegroFileBase?");
			}
			#endregion

			while (!stream.EndOfStream)
			{
				var temp = stream.ReadLine();

				if (FoundComments(temp)) continue;
				if (temp == null) continue;

				var regexGroups = regex.Match(temp).Groups;

				try
				{
					if(tempList is List<AllegroFileResult>)
					{
						#region AllegroFileResults adding
						AuctionType tempType;
						switch (regexGroups["Type"].Value)
						{
							case "a":
								tempType = AuctionType.Auction;
								break;

							case "k":
								tempType = AuctionType.BuyItNow;
								break;

							default:
								tempType = 0;
								break;
						}

						tempList.Add(new AllegroFileResult
						{
							ID = Convert.ToInt32(regexGroups["ID"].Value),
							Title = regexGroups["Title"].Value,
							FoundTitle = regexGroups["FoundTitle"].Value,
							Price = Convert.ToDecimal(regexGroups["Price"].Value),
							FoundPrice = Convert.ToDecimal(regexGroups["FoundPrice"].Value),
							Type = tempType,
							Link = regexGroups["Link"].Value,
							Nick = regexGroups["Nick"].Value,
							NickId = Convert.ToInt32(regexGroups["NickId"].Value),
							Comment = regexGroups["Comment"].Value
						});
						#endregion
					}
					else if(tempList is List<AllegroFileSearch>)
					{
						#region AllegroFileSearch adding
						tempList.Add(new AllegroFileSearch
						{
							ID = Convert.ToInt32(regexGroups["ID"].Value),
							Title = regexGroups["Title"].Value,
							Price = Convert.ToDecimal(regexGroups["Price"].Value.Replace('.',',')),
							Category = CleanCategory(regexGroups["Category"].Value),
							Comment = regexGroups["Comment"].Value
						});
						#endregion
					}
				}
				catch (Exception e)
				{
					//TODO: throw exception
				}
			}

			stream.Close();
			return tempList;
		}

		private static string CleanCategory(string categoryName)
		{
			if(categoryName.Contains("http"))
			{
				var separatedAddress = categoryName.Split('/');
				var tempString = separatedAddress[separatedAddress.Length - 1].Split('-');

				int? lastNumber;
				try
				{
					// ReSharper disable RedundantAssignment
					lastNumber = Convert.ToInt32(tempString[tempString.Length - 1]);
					// ReSharper restore RedundantAssignment
					lastNumber = tempString.Length;
				}
				catch(Exception)
				{
					lastNumber = null;
				}

				var cleanedString = "";
				// ReSharper disable LoopCanBeConvertedToQuery
				for (var i = 0; i < tempString.Length; i++)
				{
					if (i == lastNumber - 1) break;
					cleanedString += string.Format("{0} ", tempString[i]);
				}
				// ReSharper restore LoopCanBeConvertedToQuery

				return cleanedString.Remove(cleanedString.Length - 1);
			}
			return categoryName;
		}

		private bool FoundComments(string value)
		{
			return _commentRegex.Match(value).Success;
		}

		public void WriteResults(IEnumerable<AllegroFileResult> cleanedResults)
		{
			var text = new StringBuilder();
			text.AppendLine("-- <- komentarz");
			text.AppendLine("-- a - aukcja; k - kup teraz");
			text.AppendLine("Lp.;Tytuł;Znaleziony tytuł;Cena;Znaleziona cena;Aukcja/kup teraz;Link do aukcji;Identyfikator sprzedającego;Nick sprzedającego;Uwagi");

			foreach (var item in cleanedResults)
			{
				text.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", 
					item.ID, item.Title, item.FoundTitle,
					item.Price, item.FoundPrice, ConvertFromTypeToValue(item.Type), item.Link, 
					item.NickId, item.Nick, item.Comment));
			}

			using(var sr = Write(_resultPath))
			{
				sr.Write(text);
			}
		}

		private static string ConvertFromTypeToValue(AuctionType type)
		{
			switch (type)
			{
				case AuctionType.Auction:
					return "a";
				case AuctionType.BuyItNow:
					return "k";
				default:
					return null;
			}
		}
	}
}