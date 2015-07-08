using System.Net.Http;
using System.Threading.Tasks;
using QuoteService.Interfaces;

namespace Tests.PhoneUnit.Utils
{
	public class MockQuoteDataProvider : IQuoteDataProvider
	{
		private const string _url = "https://raw.githubusercontent.com/srmrco/winapp-cash-quotes/master/Tests/Tests.Unit/MockData/test-data.html";

		public IQueryBuilder QueryBuilder { get; set; }

		public async Task<string> GetQuoteDocumentTextAsync()
		{
			var result = await ExecuteGet(_url);

			return result;
		}

		private static async Task<string> ExecuteGet(string uri)
		{
			using (var client = new HttpClient())
			{
				var reqMsg = new HttpRequestMessage(HttpMethod.Get, uri);

				var response = await client.SendAsync(reqMsg);

				if (response.IsSuccessStatusCode)
					return await response.Content.ReadAsStringAsync();
				else
					return string.Empty;
			}
		}
	}
}
