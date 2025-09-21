using System.ComponentModel.DataAnnotations;

namespace VisionHive.API.Models;

public class PatioInputModel
{
    // Nome -> obrigatório, minimo 3 e máximo 100 caracters
    [Required, StringLength(100, MinimumLength = 3, 
         ErrorMessage = "Nome deve ter entre 3 e 100 caracters")]
    public string Nome {get; set;}
    
    // Localização -> obrigatória
    [Required]
    public required string Localizacao {get; set;}
    
    // Capacidade -> obrigatório, deve ser no minimo 1
    [Required, Range(1, int.MaxValue, 
         ErrorMessage = "Capacidade deve ser maior que zero")]
    public int Capacidade {get; set;}
    
    // FilialId -> obrigatório, relaciona o pátio a uma filial
    public Guid FilialId {get; set;}
}