using System;
using System.Collections.Generic;

namespace Common.Models.Media;

public class UpdateMediaDTO
{
    public Guid MediaId { get; set; }
    public Guid UserId { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }

    public List<string> Tags { get; set; }
}