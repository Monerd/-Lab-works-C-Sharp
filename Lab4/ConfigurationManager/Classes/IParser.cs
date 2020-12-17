namespace MyConfigurationManager
{
    public interface IParser
    {
        T Parse<T>() where T : new();
        public string GetPath();
    }
}
