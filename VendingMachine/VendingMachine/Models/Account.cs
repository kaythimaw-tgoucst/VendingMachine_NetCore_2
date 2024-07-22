using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VendingMachine.Models
{
    public class Account
    {
        public string ID { get; set; }

        public required string Name { get; set; }

        public required string Password { get; set; }
    }
}
