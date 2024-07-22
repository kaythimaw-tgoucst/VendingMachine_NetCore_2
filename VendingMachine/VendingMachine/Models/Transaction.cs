using Microsoft.AspNetCore.Identity;

namespace VendingMachine.Models
{
    public class Transaction
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string UserID { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
        public IdentityUser User { get; set; }
    }

}
