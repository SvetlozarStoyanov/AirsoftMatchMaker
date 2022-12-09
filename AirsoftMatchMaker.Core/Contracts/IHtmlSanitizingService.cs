namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IHtmlSanitizingService
    {
        T SanitizeObject<T>(T obj);

        string SanitizeStringProperty(string stringProperty);
    }
}
