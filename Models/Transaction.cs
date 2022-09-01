namespace BankManagement.Models;
using System.ComponentModel.DataAnnotations; 
public class Transaction
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please Enter Date Of Transaction")]  
    [DataType(DataType.Date)]
    public DateTime? Date {get; set;}
    [Required(ErrorMessage = "Please Enter Type Of Transaction")]
    public string? Type { get; set; }
    [Required(ErrorMessage = "Please Enter Amount Of Transaction")]
    public int? Amount { get; set; }
    [Required(ErrorMessage = "Please Enter Account Number")]
    public int? AccountNumber{get;set;}
    public Account?Account{get;set;}
    public int?AccountId{get;set;}
}