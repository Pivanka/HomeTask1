
namespace EtlLibrary.Strategy.Context
{
    public class Reader
    {
        IReader reader;
        public Dictionary<string, int> Run(string pathExtract, string pathLoad)
        {
            Dictionary<string, int> info = new Dictionary<string, int>();
            if (pathExtract.Contains(".csv"))
            {
                reader = new CsvReader();
                info = reader.Run(pathExtract, pathLoad);
            }
            else if (pathExtract.Contains(".txt"))
            {
                reader = new TxtReader();
                info = reader.Run(pathExtract, pathLoad);
            }
            return info;
        }
    }
}
