using Xunit;
using MiniEcommerce.Console;

namespace MiniEcommerce.Tests;

public class CarrinhoDeComprasTests
{
  [Fact]
  public void Deve_Adicionar_Item_Ao_Carrinho_Com_Sucesso()
  {
    var produto = new Produto("Monitor", 800.0m, 5);
    var carrinho = new CarrinhoDeCompras();

    bool resultado = carrinho.AdicionarItem(produto, 2);

    Assert.True(resultado);
    Assert.Single(carrinho.Itens);
    Assert.Equal(1600.0m, carrinho.Total);
  }

  [Fact]
  public void Nao_Deve_Adicionar_Item_Acima_Do_Estoque_Disponivel()
  {
    var produto = new Produto("Cadeira", 500.0m, 3);
    var carrinho = new CarrinhoDeCompras();

    bool resultado = carrinho.AdicionarItem(produto, 5);

    Assert.False(resultado);
    Assert.Empty(carrinho.Itens);
  }
}