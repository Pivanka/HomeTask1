
namespace EtlLibrary.Strategy
{
    public interface IReader
    {
        int SkippedLines { get; set; }
        int ParsedLines { get; set; }
        Dictionary<string, int> Run(string pathExtract, string pathLoad);
    }
}
