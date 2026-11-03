using MiniEcommerce.Console;

var catalogo = new List<Produto>
{
    new Produto("Notebook", 3500.00m, 10),
    new Produto("Mouse Gamer", 150.00m, 25),
    new Produto("Teclado Mecânico", 300.00m, 15)
};

var carrinho = new CarrinhoDeCompras();
var pedidoService = new PedidoService();
bool executando = true;

while (executando)
{
  Console.WriteLine("\n=== MINI E-COMMERCE ===");
  Console.WriteLine("1. Listar Produtos");
  Console.WriteLine("2. Cadastrar Novo Produto");
  Console.WriteLine("3. Adicionar Produto ao Carrinho");
  Console.WriteLine("4. Ver Carrinho");
  Console.WriteLine("5. Finalizar Pedido (Checkout)");
  Console.WriteLine("6. Sair");
  Console.Write("Escolha uma opção: ");

  string? opcao = Console.ReadLine();

  switch (opcao)
  {
    case "1":
      Console.WriteLine("\n--- CATÁLOGO DE PRODUTOS ---");
      for (int i = 0; i < catalogo.Count; i++)
      {
        var p = catalogo[i];
        Console.WriteLine($"[{i + 1}] {p.Nome} - Preço: R$ {p.Preco:F2} | Estoque: {p.Estoque}");
      }
      break;

    case "2":
      Console.Write("Nome do produto: ");
      string? nome = Console.ReadLine();
      Console.Write("Preço: ");
      decimal.TryParse(Console.ReadLine(), out decimal preco);
      Console.Write("Estoque inicial: ");
      int.TryParse(Console.ReadLine(), out int estoque);

      try
      {
        catalogo.Add(new Produto(nome ?? "", preco, estoque));
        Console.WriteLine("Produto cadastrado com sucesso!");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Erro ao cadastrar: {ex.Message}");
      }
      break;

    case "3":
      Console.Write("Digite o número do produto no catálogo: ");
      if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= catalogo.Count)
      {
        var prodSelecionado = catalogo[index - 1];
        Console.Write($"Quantidade para {prodSelecionado.Nome}: ");
        if (int.TryParse(Console.ReadLine(), out int qtd))
        {
          if (carrinho.AdicionarItem(prodSelecionado, qtd))
            Console.WriteLine("Item adicionado ao carrinho!");
          else
            Console.WriteLine("Erro: Quantidade inválida ou estoque insuficiente.");
        }
      }
      else
      {
        Console.WriteLine("Opção de produto inválida.");
      }
      break;

    case "4":
      Console.WriteLine("\n--- MEU CARRINHO ---");
      if (!carrinho.Itens.Any())
      {
        Console.WriteLine("O carrinho está vazio.");
      }
      else
      {
        foreach (var item in carrinho.Itens)
        {
          Console.WriteLine($"{item.Produto.Nome} x{item.Quantidade} - Subtotal: R$ {item.Subtotal:F2}");
        }
        Console.WriteLine($"TOTAL: R$ {carrinho.Total:F2}");
      }
      break;

    case "5":
      Console.WriteLine("\n--- CHECKOUT ---");
      decimal valorFinal = carrinho.Total;
      if (pedidoService.ProcessarCheckout(carrinho))
      {
        Console.WriteLine($"Pedido finalizado com sucesso! Valor pago: R$ {valorFinal:F2}");
      }
      else
      {
        Console.WriteLine("Falha ao processar checkout (carrinho vazio ou erro no estoque).");
      }
      break;

    case "6":
      executando = false;
      break;

    default:
      Console.WriteLine("Opção inválida.");
      break;
  }
}