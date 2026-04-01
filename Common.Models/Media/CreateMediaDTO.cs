using System.Collections.Generic;
using MediaCore.Enums;
using Microsoft.AspNetCore.Http;

namespace Common.Models.Media;

public class CreateMediaDTO
{
    public IFormFile File { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
}