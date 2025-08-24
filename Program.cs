using chat_agent;
using chat_agent.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddKernel()
    .AddGoogleAIGeminiChatCompletion(
        apiKey: builder.Configuration["Gemini:ApiKey"]!,
        modelId: builder.Configuration["Gemini:Model"]!
    ).Plugins.AddFromType<LightsPlugin>();

builder.Services.AddSingleton(new GeminiPromptExecutionSettings { ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions });

builder.Services.AddSignalR();
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

// Build the Web Application
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) { }

// Use the specific CORS policy defined above
app.UseCors("Open");
//app.UseHttpsRedirection();

app.MapHub<ChatHub>("/chat");

app.Run();

