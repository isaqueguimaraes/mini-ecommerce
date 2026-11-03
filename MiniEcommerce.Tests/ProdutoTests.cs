using Xunit;
using MiniEcommerce.Console;

namespace MiniEcommerce.Tests;

public class ProdutoTests
{
  [Fact]
  public void Nao_Deve_Permitir_Cadastrar_Produto_Com_Preco_Negativo()
  {
    Assert.Throws<ArgumentException>(() => new Produto("Teclado", -50.0m, 10));
  }

  [Fact]
  public void Nao_Deve_Permitir_Cadastrar_Produto_Com_Estoque_Invalido()
  {
    Assert.Throws<ArgumentException>(() => new Produto("Mouse", 100.0m, -5));
  }
}