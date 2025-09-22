using VisionHive.Domain.Entities;

namespace VisionHive.Application.DTO.Request
{
    public class PatioRequest
    {
        public string Nome { get; set; }
        public int LimiteMotos { get; set; }
        public Guid FilialId { get; set; }

        public Patio toDomain()
        {
            return new Patio(
                nome: Nome,
                limiteMotos: LimiteMotos,
                filialId: FilialId
            );
        }
    }
}
