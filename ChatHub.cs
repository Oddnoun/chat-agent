using agent.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;
using System.Collections.Concurrent;
using System.Text;

namespace agent;

public class ChatHub(Kernel kernel, GeminiPromptExecutionSettings executionSettings) : Hub
{
    private static readonly ConcurrentDictionary<string, ChatHistory> _chatHistories = new();

    public override async Task OnConnectedAsync()
    {
        _chatHistories.TryAdd(Context.ConnectionId, []);
        await Clients.Caller.SendAsync("ReceiveMessage", "Welcome to the chat!");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _chatHistories.TryRemove(Context.ConnectionId, out _);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageAsync(string message)
    {
        if (!_chatHistories.TryGetValue(Context.ConnectionId, out ChatHistory? chatHistory))
        {
            chatHistory = [];
            _chatHistories.TryAdd(Context.ConnectionId, chatHistory);
        }

        chatHistory.AddUserMessage(message);

        IChatCompletionService chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var fullMessage = new StringBuilder();
        await foreach (var chunk in chatCompletionService.GetStreamingChatMessageContentsAsync(
            chatHistory,
            executionSettings: executionSettings,
            kernel: kernel))
        {
            if (chunk.Content is { Length: > 0 })
            {
                fullMessage.Append(chunk.Content);

                await Clients.Caller.SendAsync("ReceiveMessageStream",
                new MessageStreamContent() { Message = chunk.Content, InProgress = true });
            }
        }

        await Clients.Caller.SendAsync("ReceiveMessageStream",
        new MessageStreamContent() { Message = null, InProgress = false });

        chatHistory.AddAssistantMessage(fullMessage.ToString());
    }
}
