using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PhyGen.Application.Services.Firebase
{
    public class FirebaseService : IFirebaseService
    {
        private readonly HttpClient _httpClient;
        private readonly string _databaseUrl;
        private readonly string _nodePath = "fcmTokens";

        public FirebaseService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _databaseUrl = configuration["Firebase:DatabaseUrl"];
        }

        public async Task<bool> SaveTokenToFirebaseAsync(string userId, string token)
        {
            var fullPath = $"{_databaseUrl}/{_nodePath}/{userId}.json";

            var json = JsonConvert.SerializeObject(new { token });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(fullPath, content);
            return response.IsSuccessStatusCode;
        }

        public async Task SendNotificationAsync(string title, string body, string fcmToken)
        {
            var message = new Message()
            {
                Token = fcmToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
