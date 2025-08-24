# .NET AI Agent

This project is a .NET-based AI agent that uses Semantic Kernel and SignalR to create a real-time chat application with function-calling capabilities.

## Quick Start

1.  **Configure API Key**: Open the `appsettings.json` file and add your Gemini API key:

    ```json
    {
      "Gemini": {
        "ApiKey": "YOUR_API_KEY"
      }
    }
    ```

2.  **Run the Server**: Open a terminal in the project root and run the following command:

    ```bash
    dotnet run
    ```

3.  **Connect Client**: The server will be running at `https://localhost:7243`. Connect your SignalR client to the `/chat` hub.

## Features

*   **Real-time Chat**: Uses SignalR for real-time, bidirectional communication between the client and server.
*   **AI-Powered Chatbot**: Integrates with Google's Gemini large language model via Semantic Kernel to provide intelligent chat responses.
*   **Function Calling**: The chatbot can interact with a `LightsPlugin` to control a set of virtual lights. You can ask the bot to turn lights on/off, change their brightness, and more.
*   **Streaming Responses**: The chatbot's responses are streamed to the client as they are generated, providing a more responsive user experience.

## Technologies

*   **.NET 8**: The backend is built using the latest version of .NET.
*   **ASP.NET Core**: Used for hosting the SignalR hub.
*   **SignalR**: Enables real-time web functionality.
*   **Semantic Kernel**: A lightweight SDK that lets you easily build agents that can call your existing code.
*   **Google Gemini**: The large language model used for chat completions.
