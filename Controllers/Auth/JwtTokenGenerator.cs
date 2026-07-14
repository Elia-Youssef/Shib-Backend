using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ShibAPI.Controllers.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace ShibAPI.Controllers.Auth
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _JwtSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Auth0Settings _auth0Settings;
        public JwtTokenGenerator(IHttpClientFactory httpClientFactory, IOptions<Auth0Settings> auth0Settings, IOptions<JwtSettings> jwtOptions)
        {
            _JwtSettings = jwtOptions.Value;
            _httpClientFactory = httpClientFactory;
            _auth0Settings = auth0Settings.Value;

        }

        public async Task<string> GenerateJwtTokenConnectMail(int Id, string username, string email)
        {
            var jsonContent = new
            {
                client_id = _auth0Settings.ClientId,
                name = username,
                email = email,
                client_secret = _auth0Settings.ClientSecret,
                audience = _auth0Settings.Audience,
                grant_type = "client_credentials"
            };

            // Convert the object to JSON
            var json = JsonConvert.SerializeObject(jsonContent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                using (var client = new HttpClient())
                {
                    var responseAuth = client.PostAsync("https://shibainu.us.auth0.com/oauth/token", content).Result;

                    // Ensure the request was successful
                    responseAuth.EnsureSuccessStatusCode();

                    var responseAuthContent = responseAuth.Content.ReadAsStringAsync().Result;
                    var responseAuthJson = JsonConvert.DeserializeObject<dynamic>(responseAuthContent);
                    string token = responseAuthJson.access_token;
               
                    // Return the token
                    return token;
                }
            }
            catch (HttpRequestException e)
            {
                // Handle HTTP request exceptions
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
            catch (Exception e)
            {
                // Handle other exceptions
                Console.WriteLine($"Unexpected error: {e.Message}");
                return null;
            }
        }

         async Task<string> IJwtTokenGenerator.GenerateJwtTokenConnectWallet( int Id,string username, string walletAddress)
        {
            var jsonContent = new
            {
                client_id = _auth0Settings.ClientId,
                name = username,
                email = walletAddress,
                client_secret = _auth0Settings.ClientSecret,
                audience = _auth0Settings.Audience,
                grant_type = "client_credentials"
                
            };

            // Convert the object to JSON
            var json = JsonConvert.SerializeObject(jsonContent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                using (var client = new HttpClient())
                {
                    var responseAuth = client.PostAsync("https://shibainu.us.auth0.com/oauth/token", content).Result;

                    // Ensure the request was successful
                    responseAuth.EnsureSuccessStatusCode();

                    var responseAuthContent = responseAuth.Content.ReadAsStringAsync().Result;
                    var responseAuthJson = JsonConvert.DeserializeObject<dynamic>(responseAuthContent);
                    string token = responseAuthJson.access_token;

                    // Return the token
                    return token;
                }
            }
            catch (HttpRequestException e)
            {
                // Handle HTTP request exceptions
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
            catch (Exception e)
            {
                // Handle other exceptions
                Console.WriteLine($"Unexpected error: {e.Message}");
                return null;
            }
        }
    }
}
