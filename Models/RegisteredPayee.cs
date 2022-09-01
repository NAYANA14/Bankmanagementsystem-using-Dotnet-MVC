namespace BankManagement.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class RegisteredPayee
{
    public int Id { get; set; }
     [Required(ErrorMessage = "Please select bank")]
    [Display(Name = "Select Bank")]
    public string?Bank{ get; set; }
    
    [Required(ErrorMessage = "Please enter Account Number")]
    [Display(Name = "Account Number")]
     public int? AccountNumber { get; set; }
    [Required(ErrorMessage = "Please enter Account Number Again")]
    [Display(Name = "Re-enter Account Number")]
    [Compare(nameof(AccountNumber))]
    [NotMapped]
    public int? ConfirmAccountNumber { get; set; }
    [Required(ErrorMessage = "Please enter branch and city")]  
    [Display(Name = "Branch And City")]
     public string? BranchCity{ get; set; }
    [Required(ErrorMessage = "Please enter Name")]  
    [Display(Name = "Name")]
    public string? Name{ get; set; }
    [Required(ErrorMessage = "Please enter Nick Name")]  
    [Display(Name = "Nick Name")]
    public string? NickName{ get; set; }
     public Customer?Customer{get;set;}
     public int?CustomerId{ get; set; }
      public virtual ICollection<MoneyTransfer>? MoneyTransfers { get; set; }
    
}