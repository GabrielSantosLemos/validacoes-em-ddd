# Responses de requisições POST

    Endpoinpt Produto
    
    InputModel
    
    ```c#:
    
    public class ProdutoInputModel
    {
        public string Nome { get; set; }
    }
    
    ```
    
    
    Sem objeto de corpo
    
    ```c#:
    
    ```
    
    {
      "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
      "title": "One or more validation errors occurred.",
      "status": 400,
      "traceId": "00-ae76a9371127ce947a97175113610ddd-88ddd3c188d7aa43-00",
      "errors": {
        "": [
          "A non-empty request body is required."
        ],
        "input": [
          "The input field is required."
        ]
      }
    }
    
    Faltando propiedades ou valores null
    
    ```c#:
    
    {
    
    }
    
    ```
    
    {
      "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
      "title": "One or more validation errors occurred.",
      "status": 400,
      "traceId": "00-c8ce61cd2a92097216180e082717d8b9-4c096d17df578985-00",
      "errors": {
        "Nome": [
          "The Nome field is required."
        ]
      }
    }

## Endpoinpt Pedido

    InputModel
    
    ```c#:
    
    public class InserirPedidoInputModel
    {
        [Required]
        public int ClienteId { get; set; }
    
        [Required]
        public IEnumerable<ItemInputModel> Itens { get; set; }
    }
    
    public class ItemInputModel
    {
        [Required]
        public int ProdutoId { get; set; }
    
        [Required]
        public int Quantidade { get; set; }
    }
    
    ```
    
    ### Sem objeto de corpo
    
    ```c#:
    
    ```
    
    {
      "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
      "title": "One or more validation errors occurred.",
      "status": 400,
      "traceId": "00-9de4c738355f6d6934c34a3a776f20af-f364e199fb1ca71e-00",
      "errors": {
        "": [
          "A non-empty request body is required."
        ],
        "input": [
          "The input field is required."
        ]
      }
    }
    
    Faltando propiedades ou valores null
    
    ```c#:
    
    {
      "clienteId": 0,
      "itens": [
        {
          "produtoId": null,
          "quantidade": 0
        }
      ]
    }
    
    ```

    {
      "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
      "title": "One or more validation errors occurred.",
      "status": 400,
      "traceId": "00-90fcca65674a2abe4a747029602b820b-80444e05db1d8e2b-00",
      "errors": {
        "input": [
          "The input field is required."
        ],
        "$.itens[0].produtoId": [
          "The JSON value could not be converted to System.Int32. Path: $.itens[0].produtoId | LineNumber: 4 | BytePositionInLine: 23."
        ]
      }
    }