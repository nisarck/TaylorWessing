using System;
using System.Collections.Generic;

namespace TaylorWessing.Models;
/// <summary>
/// 
/// </summary>
public partial class Client
{
    /// <summary>
    /// 
    /// </summary>
    public int ClientId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? EmailId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Phone { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? DateAdded { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public virtual ICollection<Matter> Matters { get; set; } = new List<Matter>();
}
