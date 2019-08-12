using System;
using System.Net.Http;
using IdentityModel.Client;

namespace ThirdParty
{
    public class Program
    {
       public static void Main(string[] args)
        {
            var disc = DiscoveryClient.GetAsync("http://localhost:5000")
                .ConfigureAwait(false).GetAwaiter().GetResult();
            if (disc.IsError)
                Console.WriteLine(disc.Error);
            var tokenClient = new TokenClient(disc.TokenEndpoint, "client", "secret");
            var token = tokenClient.RequestClientCredentialsAsync("api").ConfigureAwait(false).GetAwaiter().GetResult();

            if (token.IsError)
            {
                Console.WriteLine(token.Error);
            }
            else
            {
                Console.WriteLine(token.Json);
            }

            var httpClient = new HttpClient();
            httpClient.SetBearerToken(token.AccessToken);
            var res = httpClient.GetAsync("http://localhost:5001/api/values").ConfigureAwait(false).GetAwaiter().GetResult();
            if (res.IsSuccessStatusCode)
                Console.WriteLine(res.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult());
            Console.ReadLine();
        }
    }
}
