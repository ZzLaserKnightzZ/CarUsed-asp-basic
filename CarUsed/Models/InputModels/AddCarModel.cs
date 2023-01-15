using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CarUsed.Models.InputModels
{
    public class AddCarModel
    {
        public IFormFile? ShowImagePath { get; set; }
        public List<IFormFile?> Images { get; set; } = new List<IFormFile>();
        [Required]
        public int Price { get; set; }
        [Required]
        public int Down { get; set; }
        [Required]
        public string Bran { get; set; }
        public string Series { get; set; }
        public string Type { get; set; } //should be int
        public int Year { get; set; }
        public string Color { get; set; }
        public string Energy { get; set; }  //should be int
        public string Gear { get; set; }  //should be int
        public string Detail { get; set; }
    }
}
