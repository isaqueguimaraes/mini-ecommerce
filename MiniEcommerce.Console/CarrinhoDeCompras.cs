namespace MiniEcommerce.Console;

public class ItemCarrinho
{
  public Produto Produto { get; }
  public int Quantidade { get; set; }
  public decimal Subtotal => Produto.Preco * Quantidade;

  public ItemCarrinho(Produto produto, int quantidade)
  {
    Produto = produto;
    Quantidade = quantidade;
  }
}

public class CarrinhoDeCompras
{
  private readonly List<ItemCarrinho> _itens = new();

  public IReadOnlyCollection<ItemCarrinho> Itens => _itens.AsReadOnly();
  public decimal Total => _itens.Sum(i => i.Subtotal);

  public bool AdicionarItem(Produto produto, int quantidade)
  {
    if (quantidade <= 0 || quantidade > produto.Estoque)
      return false;

    var itemExistente = _itens.FirstOrDefault(i => i.Produto.Id == produto.Id);
    if (itemExistente != null)
    {
      if (itemExistente.Quantidade + quantidade > produto.Estoque)
        return false;

      itemExistente.Quantidade += quantidade;
    }
    else
    {
      _itens.Add(new ItemCarrinho(produto, quantidade));
    }

    return true;
  }

  public void Limpar()
  {
    _itens.Clear();
  }
}