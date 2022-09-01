using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations; 
using BankManagement.Models; 
namespace BankManagement.ViewModels;
public class CustomerViewModel:EditImageViewModel
{
    [Key]
     public int Id { get; set; }
    [Required(ErrorMessage = "Please enter first name")]  
    [Display(Name = "First Name")]  
    [StringLength(25)] 
    public string? FirstName { get; set; }
    [Required(ErrorMessage = "Please enter last name")]  
    [Display(Name = "Last Name")]  
    [StringLength(25)]   
    public string? LastName { get; set; }
    [Required(ErrorMessage = "Please enter Address")]    
    [StringLength(300)] 
    public string? Address { get; set; }
     [Required(ErrorMessage = "Please enter Adhar Number")] 
     [Display(Name = "Adhar Number")] 
     [RegularExpression(@"^(\d{12})$", ErrorMessage = "Entered adhar format is not valid.")]
    public string? AdharNumber { get; set; }
    [Required(ErrorMessage = "Please enter Pan Number")]
    [RegularExpression(@"^(\d{12})$", ErrorMessage = "Entered pan format is not valid.")]
    [Display(Name = "Pan Number")]  
    public string?  PanNumber { get; set; }
    [Required(ErrorMessage = "Please enter PhoneNumber")] 
    [Display(Name = "Phone Number")] 
    [Phone]
    public string?  PhoneNumber { get; set; }
    [Required(ErrorMessage = "Please enter Email Address")] 
    [EmailAddress]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Please enter Date Of Birth")] 
    [DataType(DataType.Date)]
    [Display(Name = "DOB")] 
     [CustomAgeValidation(ErrorMessage = "Age less than 10 is not valid")]
    public DateTime? DateOfBirth {get; set;} 
    [Required(ErrorMessage = "Please enter Account Type")] 
    [Display(Name = "Account Type")] 
    public string?AccountType{get;set;}
    public User?User{get;set;}
    public int?UserId{get;set;}
    }