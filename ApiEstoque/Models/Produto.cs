using System.Text.Json.Serialization;

namespace ApiEstoque.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public decimal? PrecoUnitario { get; set; }
        public bool Excluido { get; set; } = false;

        [JsonIgnore]
        public ICollection<ItemEstoque>? Estoques { get; set; }

    }
}
