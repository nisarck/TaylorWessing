using System;
using System.Collections.Generic;

namespace TaylorWessing.Models;

public partial class Matter
{
    public int MatterId { get; set; }

    public string? Name { get; set; }

    public DateTime? DateCreated { get; set; }

    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = null!;
}
