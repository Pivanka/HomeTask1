using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlLibrary.Models
{
    public class Transaction
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public decimal Payment { get; set; }
        public DateOnly Date { get; set; }
        public long AccountNumber { get; set; }
        public string Service { get; set; }
    }
}
