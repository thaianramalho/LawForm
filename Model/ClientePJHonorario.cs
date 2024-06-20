using System.ComponentModel.DataAnnotations;

namespace LawForm.Model
{
    public class ClientePJHonorario
    {
        [Key]
        public int Id { get; set; }
        public int ClientePJId { get; set; }
        public decimal ValorTotalHonorario { get; set; }
        public int NumeroParcelas { get; set; }
        public DateTime DataInicio { get; set; }
        public virtual ClientePJ ClientePJ { get; set; }
        public virtual ICollection<PagamentoHonorario> Pagamentos { get; set; }
    }
}
