namespace MiniEcommerce.Console;

public class PedidoService
{
  public bool ProcessarCheckout(CarrinhoDeCompras carrinho)
  {
    if (!carrinho.Itens.Any())
      return false;

    foreach (var item in carrinho.Itens)
    {
      if (item.Quantidade > item.Produto.Estoque)
        return false;
    }

    foreach (var item in carrinho.Itens)
    {
      item.Produto.DeduzirEstoque(item.Quantidade);
    }

    carrinho.Limpar();
    return true;
  }
}