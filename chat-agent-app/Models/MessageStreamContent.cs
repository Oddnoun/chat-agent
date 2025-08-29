using System;

namespace chat_agent.Models;

public class MessageStreamContent
{
    public string? Message { get; set; }
    public bool InProgress { get; set; } = true;

}
