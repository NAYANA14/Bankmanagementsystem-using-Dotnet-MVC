using Microsoft.AspNetCore.Http;  
using System.ComponentModel.DataAnnotations;  
  
namespace BankManagement.ViewModels  
{  
    public class UploadImageViewModel  
    {  
       [Required(ErrorMessage = "Please select Image")]   
        [Display(Name = "Photo")]  
        public IFormFile? Image{ get; set; }  
        [Required(ErrorMessage = "Please select Sign")] 
         [Display(Name = "Sign")]  
        public IFormFile? Signature { get; set; }  
    }  
}  