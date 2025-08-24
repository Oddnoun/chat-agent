using System;

namespace agent.Models;

public class MessageStreamContent
{
    public string? Message { get; set; }
    public bool InProgress { get; set; } = true;

}
