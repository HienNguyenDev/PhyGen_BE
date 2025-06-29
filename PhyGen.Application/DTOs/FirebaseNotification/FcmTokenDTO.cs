using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.DTOs.FirebaseNotification
{
    public class FcmTokenDTO
    {
        public string UserId { get; set; }
        public string FcmToken { get; set; }
    }
}
