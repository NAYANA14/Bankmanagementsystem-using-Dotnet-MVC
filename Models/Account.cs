namespace BankManagement.Models;

using System.ComponentModel.DataAnnotations; 
public class Account
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please enter Account Number")]  
    [Display(Name = "Account Number")]  
    public int? AccountNumber { get; set; }
    [Required(ErrorMessage = "Please enter Account Type")]  
    public string? Type { get; set; }
    [Required(ErrorMessage = "Please enter Balance")]  
     public int? Balance { get; set; }
     public int? CustomerId{get;set;}
   public Customer?Customer{get;set;}
    public virtual ICollection<Transaction>? Transactions { get; set; }
}

    
