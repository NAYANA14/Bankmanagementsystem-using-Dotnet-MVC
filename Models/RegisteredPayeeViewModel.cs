using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
namespace BankManagement.Models
{
public class RegisteredPayeeViewModel
{ 
 public string? searchName{get;set;} 
 public List<RegisteredPayee>? RegisteredPayees { get; set; }
}
}