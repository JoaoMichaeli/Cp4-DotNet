using System.ComponentModel.DataAnnotations;

namespace VisionHive.API.Models
{
    public class MotoInputModel
    {

        // Placa obrigatória, validada com Regex (aceita padrão antigo e Mercosul)
        [Required, RegularExpression(@"^(?:[A-Z]{3}-?\d{4}|[A-Z]{3}\d[A-Z]\d{2})$", 
            ErrorMessage = "Placa Inválida (use AAA-1234 ou AAA1A23")]
        public required string Placa { get; set; }

        // Chassi opcional 
        public string? Chassi { get; set; }

        // NumeroMotor opcional
        public string NumeroMotor { get; set; }


        // Prioridade obrigatória, validada contra o enum Prioridade
        [Required, EnumDataType(typeof(Prioridade))]
        public Prioridade Prioridade { get; set; }

        // PatioId obrigatório, liga a moto a um pátio
        [Required]
        public Guid PatioId { get; set; }
}