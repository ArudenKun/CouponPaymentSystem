using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace Domain.Configuration;

public class CpsOptions
{
    [Required]
    public int SysId { get; set; }

    [ValidateObjectMembers]
    [Required]
    public AsoDatabaseOptions Aso { get; set; } = new();

    [ValidateObjectMembers]
    [Required]
    public CpsDatabaseOptions Cps { get; set; } = new();
}
