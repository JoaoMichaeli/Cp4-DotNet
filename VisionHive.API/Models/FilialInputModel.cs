using System.ComponentModel.DataAnnotations;
namespace VisionHive.API.Models;

public class FilialInputModel
{
    // Nome -> obrigatório, minimo 3 a 100 caracters
    [Required, StringLength(100, MinimumLength =  3, 
        ErrorMessage = "Nome deve ter entre 3 e 100 caracters")]
    public required string Nome { get; set; }
    
    // Endereço -> obrigatório 
    [Required]
    public required string Endereco { get; set; }
    
    // Cidade -> obrigatório
    [Required]
    public required string Cidade {get; set;}
    
    // Estado -> obrigatório, 2 letrar (ex: SP, RJ, MG)
    [Required, RegularExpression(@"^[A-Z]{2}$", 
            ErrorMessage= "Estado deve ter exatamente 2 letras maiúsculas")] 
    public required string Estado {get; set;}
    
}