namespace BankManagement.Models;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;
public class User
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please Enter Name")]  
    [Display(Name = "User Name")]  
    [StringLength(25)] 
    public string? UserName { get; set; }
    [Required(ErrorMessage = "Please Enter Password")] 
    [DataType(DataType.Password)]   
    [StringLength(25)] 
    public string? Password { get; set; }
    [NotMapped] 
    [Required(ErrorMessage = "Please Enter Password Again")] 
    [DataType(DataType.Password),Compare(nameof(Password))]
    [Display(Name = "Confirm Password")] 
    public string? ConfirmPassword{ get; set; } 
     public virtual Customer? Customer { get; set; }
}