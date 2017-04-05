using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace MarkdownMerge.Translation
{
    public static class Translator
    {
        private const string SubscriptionKey = "679dbef3bf6345a2a1ff66314f9f6900";   //Enter here the Key from your Microsoft Translator Text subscription on http://portal.azure.com
        private static string token;

        /// Demonstrates getting an access token and using the token to translate.
        public static string Translate(string text, string langFrom, string langTo)
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

            return translatorService.Translate(token, text, langFrom, langTo, "text/html", "general", string.Empty);
        }
    }
}
