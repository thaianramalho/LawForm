using System.ComponentModel.DataAnnotations;

namespace LawForm.Model
{
    public class PagamentoHonorario
    {
        [Key]
        public int Id { get; set; }
        public int HonorarioId { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal Valor { get; set; }
        public bool Pago { get; set; }
    }
}
