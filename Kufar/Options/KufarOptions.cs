using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kufar.Options;

public record KufarOptions
{
    [Required]
    public string UserToken { get; init; }
    [Required]
    public string BaseUrl { get; init; }

    [Range(30, 100)]
    public int TimeoutSeconds { get; init; } = 30;
    public string TokenProvider { get; init; } = "Bearer";
}
