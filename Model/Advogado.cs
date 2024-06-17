using System.ComponentModel.DataAnnotations;

namespace LawForm.Model
{
    public class Advogado
    {
        [Key]
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Cpf { get; set; }
        public string? Email { get; set; }
        public string? Cna { get; set; }

    }
}
