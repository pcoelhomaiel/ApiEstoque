namespace ApiEstoque.Models
{
    public class ItemEstoque
    {
        //Chave Composta
        public int IdProduto { get; set; }
        public Produto Produto { get; set; } = null!;
        public int IdLoja { get; set; }
        public Loja Loja { get; set; } = null!;


        public int Quantidade { get; set; }
    }
}