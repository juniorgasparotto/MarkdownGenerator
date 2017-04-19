using Html2Markdown.Replacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace MarkdownGenerator.Translation
{
    public static class Translator
    {
        //private const string SubscriptionKey = "679dbef3bf6345a2a1ff66314f9f6900";   //Enter here the Key from your Microsoft Translator Text subscription on http://portal.azure.com
        private const string SubscriptionKey = "e51d6fba40a548d1b2fa6c76c428c7a6";   //Enter here the Key from your Microsoft Translator Text subscription on http://portal.azure.com
        private static string token;
        
        /// Demonstrates getting an access token and using the token to translate.
        public static string TranslateChunk(string html, string langFrom, string langTo)
        {
            var strBuilder = new StringBuilder();
            var lstStr = new List<string>() { "" };
            var doc = HtmlParser.GetHtmlDocument(html);
            foreach(var node in doc.DocumentNode.ChildNodes)
            {
                var countLast = lstStr.Last().Length;
                var countCur = node.InnerHtml.Length;
                if (countLast + countCur <= 10000)
                    lstStr[lstStr.Count - 1] += node.OuterHtml;
                else
                    lstStr.Add(node.OuterHtml);
            }

            for (var i = 0; i < lstStr.Count; i++)
                lstStr[i] = Translate(lstStr[i], langFrom, langTo);

            var ret = string.Join("", lstStr);
            return ret;
        }

        /// Demonstrates getting an access token and using the token to translate.
        public static string Translate(string html, string langFrom, string langTo)
        {
            var translatorService = new TranslatorService.LanguageServiceClient();
            var authTokenSource = new AzureAuthToken(SubscriptionKey);

            try
            {
                if (token == null)
                    token = authTokenSource.GetAccessToken();
            }
            catch (HttpRequestException)
            {
                switch (authTokenSource.RequestStatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        Console.WriteLine("Request to token service is not authorized (401). Check that the Azure subscription key is valid.");
                        break;
                    case HttpStatusCode.Forbidden:
                        Console.WriteLine("Request to token service is not authorized (403). For accounts in the free-tier, check that the account quota is not exceeded.");
                        break;
                }
                throw;
            }

            return translatorService.Translate(token, html, langFrom, langTo, "text/html", "general", string.Empty);
        }
    }
}
