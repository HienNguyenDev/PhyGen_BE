using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.DTOs.Image
{
    public class ImageUploadDTO
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
