using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace QuoteService.Utils
{
	public static class HtmlNodeListExtensionMethods
	{
		public static HtmlNode GetHtmlNodeByClass<T>(this IList<T> list, string className) where T : HtmlNode
		{
			return list.FirstOrDefault(c => c.Attributes["class"] != null && c.Attributes["class"].Value == className);
		}

		public static string ExtractTextForHtmlNodeByClass<T>(this IList<T> list, string className) where T : HtmlNode
		{
			var item = list.GetHtmlNodeByClass(className);

			return item != null ? WebUtility.HtmlDecode(item.InnerText) : string.Empty;
		}

	}
}
