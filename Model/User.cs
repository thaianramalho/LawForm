using System.ComponentModel.DataAnnotations;

namespace LawForm.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required string Nome { get; set; }
        public string? Email { get; set; }
        public required string Senha { get; set; }
    }
}
