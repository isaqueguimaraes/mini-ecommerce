namespace MiniEcommerce.Console;

public class Produto
{
  public Guid Id { get; private set; }
  public string Nome { get; private set; }
  public decimal Preco { get; private set; }
  public int Estoque { get; private set; }

  public Produto(string nome, decimal preco, int estoque)
  {
    if (string.IsNullOrWhiteSpace(nome))
      throw new ArgumentException("O nome do produto não pode ser vazio.", nameof(nome));

    if (preco < 0)
      throw new ArgumentException("O preço não pode ser negativo.", nameof(preco));

    if (estoque < 0)
      throw new ArgumentException("O estoque não pode ser menor que zero.", nameof(estoque));

    Id = Guid.NewGuid();
    Nome = nome.Trim();
    Preco = preco;
    Estoque = estoque;
  }

  public void DeduzirEstoque(int quantidade)
  {
    if (quantidade > Estoque)
      throw new InvalidOperationException("Quantidade solicitada excede o estoque disponível.");

    Estoque -= quantidade;
  }
}