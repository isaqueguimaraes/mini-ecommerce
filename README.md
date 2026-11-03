# 🛒 MiniEcommerce

Aplicação de console em **C# / .NET 10** que simula uma loja virtual: catálogo de produtos, carrinho de compras com controle de estoque e checkout com baixa automática no estoque. O projeto conta com **testes unitários em xUnit** e histórico de commits seguindo o padrão **Conventional Commits**.

Projeto desenvolvido para a disciplina **Garantia da Qualidade de Software** (Gestão e Qualidade de Software, Ânima Educação), com foco em qualidade de código, testes automatizados e versionamento organizado.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![xUnit](https://img.shields.io/badge/tests-xUnit-5E2D91)
![Conventional Commits](https://img.shields.io/badge/commits-conventional-FE5196?logo=conventionalcommits&logoColor=white)

---

## 📑 Sumário

- [Visão geral](#-visão-geral)
- [Requisitos técnicos](#-requisitos-técnicos)
- [Arquitetura e estrutura do projeto](#-arquitetura-e-estrutura-do-projeto)
- [Funcionalidades](#-funcionalidades)
- [Regras de negócio](#-regras-de-negócio)
- [Como clonar e compilar](#-como-clonar-e-compilar)
- [Como executar a aplicação](#-como-executar-a-aplicação)
- [Como executar os testes unitários](#-como-executar-os-testes-unitários)
- [Cobertura dos testes](#-cobertura-dos-testes)
- [Histórico de commits](#-histórico-de-commits)
- [Tecnologias utilizadas](#-tecnologias-utilizadas)
- [Contribuidores](#-contribuidores)

---

## 📖 Visão geral

O **MiniEcommerce** permite cadastrar produtos, consultar o catálogo, montar um carrinho de compras e finalizar o pedido. Ao concluir a compra, o estoque dos produtos adquiridos é atualizado automaticamente e o valor total do pedido é calculado com base nos itens selecionados.

Toda a interação acontece por um menu interativo no terminal.

---

## 🛠 Requisitos técnicos

- **[.NET SDK 10.0](https://dotnet.microsoft.com/download)** (ou superior)
- **[Git](https://git-scm.com/downloads)**
- Terminal (PowerShell, CMD, Bash, Zsh etc.)
- IDE opcional: Visual Studio 2022+, VS Code (com extensão C# Dev Kit) ou JetBrains Rider

Para verificar a versão do .NET instalada:

```bash
dotnet --version
```

---

## 🏗 Arquitetura e estrutura do projeto

A solução é dividida em dois projetos: a aplicação de console, que concentra as entidades, as regras de negócio e a interface, e o projeto de testes, que referencia a aplicação.

```text
MiniEcommerce/
├── MiniEcommerce.sln
├── MiniEcommerce.Console/
│   ├── MiniEcommerce.Console.csproj
│   ├── Program.cs
│   ├── Produto.cs
│   ├── CarrinhoDeCompras.cs
│   └── PedidoService.cs
├── MiniEcommerce.Tests/
│   ├── MiniEcommerce.Tests.csproj
│   ├── ProdutoTests.cs
│   ├── CarrinhoDeComprasTests.cs
│   └── PedidoTests.cs
├── .gitignore
└── README.md
```

| Componente          | Tipo             | Responsabilidade                                                                             |
| ------------------- | ---------------- | -------------------------------------------------------------------------------------------- |
| `Produto`           | Entidade         | Representa um produto (`Id`, `Nome`, `Preco`, `Estoque`), valida os dados e deduz o estoque. |
| `ItemCarrinho`      | Entidade         | Representa um item do carrinho (produto, quantidade e subtotal).                             |
| `CarrinhoDeCompras` | Regra de negócio | Adiciona itens, verifica limites de estoque e calcula o total.                               |
| `PedidoService`     | Serviço          | Processa o checkout, valida o carrinho e atualiza o estoque dos produtos.                    |
| `Program.cs`        | Interface        | Menu interativo em loop e catálogo inicial de produtos.                                      |

- **`MiniEcommerce.Console`**: projeto de console (`net10.0`).
- **`MiniEcommerce.Tests`**: projeto de testes xUnit (`net10.0`) que referencia o projeto `MiniEcommerce.Console`.

---

## 🎯 Funcionalidades

O menu interativo oferece as seguintes opções:

| Opção | Funcionalidade                | Descrição                                                                                 |
| :---: | ----------------------------- | ----------------------------------------------------------------------------------------- |
|   1   | Listar Produtos               | Exibe o catálogo com nome, preço e estoque atual de cada produto.                         |
|   2   | Cadastrar Novo Produto        | Cadastra um produto informando nome, preço e estoque inicial, com validação dos dados.    |
|   3   | Adicionar Produto ao Carrinho | Seleciona um produto do catálogo e a quantidade desejada, validando o estoque disponível. |
|   4   | Ver Carrinho                  | Exibe os itens, os subtotais e o total do carrinho.                                       |
|   5   | Finalizar Pedido (Checkout)   | Processa a compra, dá baixa no estoque e esvazia o carrinho.                              |
|   6   | Sair                          | Encerra a aplicação.                                                                      |

O catálogo inicia com três produtos de exemplo: **Notebook**, **Mouse Gamer** e **Teclado Mecânico**.

---

## 💼 Regras de negócio

### `Produto`

| Regra                                                               | Comportamento                     |
| ------------------------------------------------------------------- | --------------------------------- |
| Nome nulo, vazio ou só com espaços                                  | Lança `ArgumentException`         |
| Preço negativo                                                      | Lança `ArgumentException`         |
| Estoque menor que zero                                              | Lança `ArgumentException`         |
| Nome com espaços nas extremidades                                   | Os espaços são removidos          |
| `DeduzirEstoque(int quantidade)` com quantidade maior que o estoque | Lança `InvalidOperationException` |

Todo produto recebe um `Id` único (`Guid`) gerado automaticamente.

### `CarrinhoDeCompras`

| Membro                                           | Descrição                                                                                                                    |
| ------------------------------------------------ | ---------------------------------------------------------------------------------------------------------------------------- |
| `Itens`                                          | Coleção somente leitura dos itens do carrinho.                                                                               |
| `Total`                                          | Soma dos subtotais de todos os itens (`Preço × Quantidade`).                                                                 |
| `AdicionarItem(Produto produto, int quantidade)` | Adiciona o item e retorna `true`. Retorna `false` se a quantidade for menor ou igual a zero ou exceder o estoque disponível. |
| `Limpar()`                                       | Remove todos os itens do carrinho.                                                                                           |

Ao adicionar um produto que já está no carrinho, a quantidade é somada ao item existente, e a soma também não pode ultrapassar o estoque disponível.

### `PedidoService`

`ProcessarCheckout(CarrinhoDeCompras carrinho)` retorna `true` e conclui a compra somente quando:

1. O carrinho **não está vazio**.
2. Todos os itens têm quantidade **dentro do estoque** disponível.

Nesse caso, o estoque de cada produto é decrementado e o carrinho é esvaziado. Caso contrário, retorna `false` e nenhuma baixa de estoque é realizada.

---

## 🚀 Como clonar e compilar

### 1. Clonar o repositório

```bash
git clone <URL_DO_SEU_REPOSITORIO_GITHUB>
```

### 2. Acessar a pasta do projeto

```bash
cd MiniEcommerce
```

### 3. Restaurar as dependências

```bash
dotnet restore
```

### 4. Compilar a solução

```bash
dotnet build
```

Se tudo estiver correto, a saída final indicará **Build succeeded** (compilação bem-sucedida), com 0 erros.

---

## ▶️ Como executar a aplicação

Na raiz do repositório, execute:

```bash
dotnet run --project MiniEcommerce.Console/MiniEcommerce.Console.csproj
```

O menu interativo será exibido no terminal:

```text
=== MINI E-COMMERCE ===
1. Listar Produtos
2. Cadastrar Novo Produto
3. Adicionar Produto ao Carrinho
4. Ver Carrinho
5. Finalizar Pedido (Checkout)
6. Sair
Escolha uma opção:
```

---

## ✅ Como executar os testes unitários

Na raiz do repositório (onde está o arquivo `MiniEcommerce.sln`), execute:

```bash
dotnet test
```

Para uma saída mais detalhada, com o nome de cada teste executado:

```bash
dotnet test --logger "console;verbosity=detailed"
```

Para executar apenas o projeto de testes:

```bash
dotnet test MiniEcommerce.Tests/MiniEcommerce.Tests.csproj
```

Ao final da execução, todos os testes devem ser aprovados, com uma saída semelhante a:

```text
Aprovado!  – Com falha: 0, Aprovado: 5, Ignorado: 0, Total: 5
```

---

## 🧪 Cobertura dos testes

Os testes utilizam o atributo `[Fact]` do xUnit e estão divididos em três classes, uma para cada etapa de regras de negócio. No total, são **5 testes**.

### `ProdutoTests`

| Teste                                                      | Cenário validado                                                 |
| ---------------------------------------------------------- | ---------------------------------------------------------------- |
| `Nao_Deve_Permitir_Cadastrar_Produto_Com_Preco_Negativo`   | Criar um produto com preço negativo lança `ArgumentException`.   |
| `Nao_Deve_Permitir_Cadastrar_Produto_Com_Estoque_Invalido` | Criar um produto com estoque negativo lança `ArgumentException`. |

### `CarrinhoDeComprasTests`

| Teste                                                 | Cenário validado                                                                                               |
| ----------------------------------------------------- | -------------------------------------------------------------------------------------------------------------- |
| `Deve_Adicionar_Item_Ao_Carrinho_Com_Sucesso`         | Adicionar 2 unidades de um produto de R$ 800,00 retorna `true`, o carrinho tem 1 item e o total é R$ 1.600,00. |
| `Nao_Deve_Adicionar_Item_Acima_Do_Estoque_Disponivel` | Adicionar 5 unidades de um produto com estoque 3 retorna `false` e o carrinho permanece vazio.                 |

### `PedidoTests`

| Teste                                         | Cenário validado                                                                                                                  |
| --------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------- |
| `Deve_Processar_Checkout_E_Atualizar_Estoque` | Após comprar 4 unidades de um produto com estoque 10, o checkout retorna `true`, o estoque passa a ser 6 e o carrinho fica vazio. |

---

## 🌿 Histórico de commits

O repositório segue rigorosamente o padrão **[Conventional Commits](https://www.conventionalcommits.org/)** (`feat`, `fix`, `test`, `refactor`, `docs`, `chore`), com os testes escritos antes das funcionalidades (TDD básico). A sequência de commits do projeto é:

| Etapa                 | Tipo       | Commit                                                                 |
| --------------------- | ---------- | ---------------------------------------------------------------------- |
| Estrutura inicial     | `chore`    | `chore: initial project structure with console and xunit apps`         |
| Etapa 1: Produtos     | `test`     | `test(produto): add unit tests for product validation rules`           |
| Etapa 1: Produtos     | `feat`     | `feat(produto): implement product entity and validation logic`         |
| Etapa 2: Carrinho     | `test`     | `test(carrinho): add unit tests for cart addition and stock limits`    |
| Etapa 2: Carrinho     | `feat`     | `feat(carrinho): implement shopping cart logic and stock verification` |
| Etapa 3: Pedido       | `test`     | `test(pedido): add unit tests for checkout and stock deduction`        |
| Etapa 3: Pedido       | `feat`     | `feat(pedido): implement order processing and inventory update`        |
| Etapa 4: Console      | `feat`     | `feat(console): implement interactive menu for e-commerce store`       |
| Etapa 5: Refatoração  | `refactor` | `refactor(core): improve code readability and organize namespaces`     |
| Etapa 5: Documentação | `docs`     | `docs: add comprehensive instructions in README.md`                    |

Para visualizar o histórico:

```bash
git log --oneline
```

---

## 🧰 Tecnologias utilizadas

- [C#](https://learn.microsoft.com/dotnet/csharp/)
- [.NET 10](https://dotnet.microsoft.com/)
- [xUnit](https://xunit.net/)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Git](https://git-scm.com/) e [GitHub](https://github.com/)

---

## 👥 Autor

Isaque Guimarães
````
