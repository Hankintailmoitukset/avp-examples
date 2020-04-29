using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Avp
{
    internal class Program
    {
        private static HttpRequestMessage GetRequest(HttpMethod method, HttpContent content, string uri, string apiKey)
        {
            return new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(uri),
                Headers =
                {
                    { "Ocp-Apim-Subscription-Key", apiKey }
                },
                Content = content
            };
        }

        static void Main(params string[] argv)
        {
            Parser.Default.ParseArguments<Options>(argv)
                .WithParsed(o =>
                {
                    var config = new ConfigurationBuilder()
                        .AddJsonFile(o.ConfigurationFile)
                        .Build()
                        .Get<Configuration>();

                    if (o.FetchNoticeBodies)
                    {
                        o.Select = "id";
                    }

                    RunSearch(o, config).Wait();
                });
        }

        static async Task RunSearch(Options o, Configuration config)
        {
            using var client = new HttpClient();

            var response = await client.SendAsync(GetRequest(
                HttpMethod.Post,
                new StringContent(JsonConvert.SerializeObject(new
                {
                    count = o.Count,
                    facets = o.Facets,
                    filter = o.Filter,
                    orderby = o.OrderBy,
                    search = o.Search,
                    searchFields = o.SearchFields,
                    searchMode = o.SearchMode,
                    select = o.Select,
                    skip = o.Skip,
                    top = o.Top
                }), Encoding.UTF8, "application/json"),
                config.SearchEndpoint,
                config.ApiKey));

            var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

            if (!o.FetchNoticeBodies)
            {
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine("[");
                foreach (var notice in content.value)
                {
                    var noticeRequest = await client.SendAsync(GetRequest(HttpMethod.Get, null,
                            $"{config.FetchEndpoint}{notice.id}",
                            config.ApiKey));
                        // JsonConvert deserialize to dynamic prettifies the output
                        Console.WriteLine(
                            $"{JsonConvert.DeserializeObject<dynamic>(await noticeRequest.Content.ReadAsStringAsync())},");
                }
                Console.WriteLine("]");
            }
        }
    }
}
