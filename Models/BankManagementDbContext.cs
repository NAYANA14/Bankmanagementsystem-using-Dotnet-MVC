using Microsoft.EntityFrameworkCore;
namespace BankManagement.Models
{
    public class BankManagementDbContext: DbContext
    { 
        public BankManagementDbContext(DbContextOptions<BankManagementDbContext> options) : base(options) 
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<RegisteredPayee> RegisteredPayees { get; set; }
         public DbSet<MoneyTransfer> MoneyTransfers { get; set; }
         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }
        
    }
}