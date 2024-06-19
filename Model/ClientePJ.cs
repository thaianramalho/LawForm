using System.ComponentModel.DataAnnotations;

namespace LawForm.Model
{
    public class ClientePJ
    {
        [Key]
        public int Id { get; set; }
        public required string NomeEmpresa { get; set; }
        public required string Cnpj {  get; set; }
        public required string InscricaoEstadual { get; set; }
        public string? NaturezaJuridica { get; set; }
        public string? EnderecoEmpresa { get; set; }
        public string? TelefonesEmpresa { get; set; }
        public string? EmailEmpresa { get; set; }
        public required string NomeResponsavel { get; set; }
        public string? FiliacaPai { get; set; }
        public string? FiliacaoMae { get; set; }
        public required string Nacionalidade { get; set; }
        public required string EstadoCivil { get; set; }
        public string? Profissao { get; set; }
        public string? DocumentoCI { get; set; }
        public required string DocumentoCPF { get; set; }
        public string? DocumentoPIS { get; set; }
        public string? DocumentoCTPS { get; set; }
        public string? DocumentoSerie { get; set; }
        public required string Endereco { get; set; }
        public required string Telefones { get; set; }
        public required string Naturalidade { get; set; }
        public required DateOnly DataNascimento { get; set; }
        public string? Email { get; set; }
        public required string Historico { get; set; }
        public string DataNascimentoFormatada => DataNascimento.ToString("dd/MM/yyyy");

    }
}
