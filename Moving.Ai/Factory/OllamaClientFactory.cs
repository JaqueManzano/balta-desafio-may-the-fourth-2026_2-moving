using OllamaSharp;

public static class OllamaClientFactory
{
    public static OllamaApiClient Create()
    {
        return new OllamaApiClient(new Uri("http://localhost:11434"))
        {
            SelectedModel = "phi3:mini"
        };
    }
}