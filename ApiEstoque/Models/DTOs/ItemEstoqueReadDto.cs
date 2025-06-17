namespace ApiEstoque.Models.DTOs
{
    public class ItemEstoqueReadDto
    {
        public int IdProduto { get; set; }
        public string NomeProduto { get; set; } = null!;
        public int IdLoja { get; set; }
        public string NomeLoja { get; set; } = null!;
        public int Quantidade { get; set; }
    }
}
