namespace ApiEstoque.DTOs
{
    public class ItemEstoqueCreateDto
    {
        public int IdProduto { get; set; }
        public int IdLoja { get; set; }
        public int Quantidade { get; set; }
    }
}
