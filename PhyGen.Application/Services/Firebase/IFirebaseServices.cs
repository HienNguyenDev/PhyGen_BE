using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Services.Firebase
{
    public interface IFirebaseServices
    {
        Task<bool> SaveTokenToFirebaseAsync(string userId, string token);
        Task SendNotificationAsync(string title, string body, string fcmToken);
    }
}
