using System;
using System.Collections.Generic;

namespace MediaCore.Models;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<MediaTag> MediaTags { get; set; }
}