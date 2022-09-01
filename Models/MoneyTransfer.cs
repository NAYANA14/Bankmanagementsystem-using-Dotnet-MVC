using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BankManagement.Models;  
public class MoneyTransfer
{
    public int? Id { get; set; }
    [Required(ErrorMessage = "Please Enter Amount")]
    [Display(Name = "Amount")] 
    public int? Amount { get; set; }
    [Display(Name = "Debit Account")] 

    public int? SenderAccountNumber{get;set;}  
    [NotMapped]
    [Display(Name="Comments(Optional)")]
     public string? Comments{get;set;}  
     public int?RegisteredPayeeId{get;set;}
    public RegisteredPayee?RegisteredPayee{get;set;}
     
}