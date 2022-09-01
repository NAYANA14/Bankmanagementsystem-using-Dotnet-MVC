namespace BankManagement.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Customer
{   
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
    [DataType(DataType.PhoneNumber)]
    public string?  PhoneNumber { get; set; }
    [Required(ErrorMessage = "Please enter Email Address")] 
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Display(Name = "Date of Birth")]  
    [DataType(DataType.Date)]  
    [Required]
    [CustomAgeValidation(ErrorMessage = "Age less than 10 is not valid")]
     public DateTime? DateOfBirth {get; set;}
    
    [Required(ErrorMessage = "Please select Image")]
    [Display(Name = "Photo")]  
    public string? Image { get; set; }
    [Required(ErrorMessage = "Please select Signature")]
    [Display(Name = "Sign")] 
    public string? Signature{ get; set;}
    [NotMapped]
    public string FullName => FirstName + " " + LastName;
    public int?UserId{get;set;}
    public virtual User? User { get; set; }
   public virtual ICollection<RegisteredPayee>? RegisteredPayees { get; set; }
   public virtual ICollection<Account>? Accounts { get; set; }
}