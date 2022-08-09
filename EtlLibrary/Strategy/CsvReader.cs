using EtlLibrary.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EtlLibrary.Strategy
{
    public  class CsvReader : IReader
    {
        public int SkippedLines { get; set; }
        public int ParsedLines { get; set; }
        public Dictionary<string, int> Run(string pathExtract, string pathLoad)
        {
            List<Transaction> transactions = new List<Transaction>();
            string line;
            using (StreamReader file = new StreamReader(pathExtract))
            {
                file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(", ");
                    try
                    {
                        transactions.Add(new Transaction
                        {
                            FirstName = words[0],
                            LastName = words[1],
                            Address = new Address 
                            { 
                                City = Regex.Replace(words[2], "[^A-Za-z0-9 ]", ""), 
                                Street = words[3], 
                                NumberBuilding = Int32.Parse(Regex.Replace(words[4], "[^A-Za-z0-9 ]", "")) 
                            },
                            Payment = decimal.Parse(words[5].Replace(".", ",")),
                            Date = DateOnly.ParseExact(words[6], "yyyy-dd-mm", CultureInfo.InvariantCulture),
                            AccountNumber = Int64.Parse(words[7]),
                            Service = words[8]
                        });
                        ParsedLines++;
                    }
                    catch (Exception)
                    {
                        SkippedLines += 1;
                    }
                }
            }

            var viewmodel = from b in transactions
                            group b by b.Address.City into g
                            select new
                            {
                                City = g.Key,
                                Services = from i in g
                                           group i by i.Service into c
                                           select new
                                           {
                                               Name = c.Key,
                                               Payers = g.Where(b => b.Service == c.Key).Select(p => new
                                               {
                                                   Name = p.FirstName + " " + p.LastName,
                                                   Payment = p.Payment,
                                                   Date = p.Date.ToShortDateString(),
                                                   Account_Number = p.AccountNumber
                                               }),
                                               Total = g.Where(b => b.Service == c.Key).Select(x => x.Payment).Sum()
                                           },
                                Total = g.Select(x => x.Payment).Sum()
                            };

            using (StreamWriter fileLoad = File.CreateText(pathLoad))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(fileLoad, transactions);
            }

            var res = new Dictionary<string, int>()
            {
                ["parsed_lines"] = ParsedLines,
                ["found_errors"] = SkippedLines
            };

            return res;
        }
    }
}
