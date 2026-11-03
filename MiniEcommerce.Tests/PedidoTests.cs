using Xunit;
using MiniEcommerce.Console;

namespace MiniEcommerce.Tests;

public class PedidoTests
{
  [Fact]
  public void Deve_Processar_Checkout_E_Atualizar_Estoque()
  {
    var produto = new Produto("Fone de Ouvido", 150.0m, 10);
    var carrinho = new CarrinhoDeCompras();
    carrinho.AdicionarItem(produto, 4);

    var service = new PedidoService();
    bool sucesso = service.ProcessarCheckout(carrinho);

    Assert.True(sucesso);
    Assert.Equal(6, produto.Estoque);
    Assert.Empty(carrinho.Itens);
  }
}