using System.Text.Json.Serialization;

namespace ApiEstoque.Models
{
    public class Loja
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? CNPJ { get; set; }
        public string? Endereco { get; set; }
        public bool Excluido { get; set; } = false;

        [JsonIgnore]
        public ICollection<ItemEstoque>? Estoques { get; set; }

    }
}
