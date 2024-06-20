using System.ComponentModel.DataAnnotations;

namespace LawForm.Model
{
    public class ClientePFHonorario
    {
        [Key]
        public int Id { get; set; }
        public int ClientePFId { get; set; }
        public decimal ValorTotalHonorario { get; set; }
        public int NumeroParcelas { get; set; }
        public DateTime DataInicio { get; set; }
        public virtual ClientePF ClientePF { get; set; }
        public virtual ICollection<PagamentoHonorario> Pagamentos { get; set; }
    }
}
