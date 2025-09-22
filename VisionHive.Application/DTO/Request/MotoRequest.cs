using VisionHive.Application.Enums;
using VisionHive.Domain.Entities;


namespace VisionHive.Application.DTO.Request
{
    public class MotoRequest
    {
        public string? Placa { get; set; }
        public string? Chassi { get; set; }
        public string? NumeroMotor { get; set; }
        public Prioridade Prioridade { get; set; }
        public Guid PatioId { get; set; }

        public Moto toDomain()
        {
            return new Moto(
                placa: Placa,
                chassi: Chassi,
                numeroMotor: NumeroMotor,
                prioridade: Prioridade,
                patioId: PatioId
            );
        }
    }
}
