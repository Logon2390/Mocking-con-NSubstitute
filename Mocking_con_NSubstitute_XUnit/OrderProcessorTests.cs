using NSubstitute;
using Xunit;

public class OrderProcessorTests
{
    [Fact]
    public void ProcesarPedido_ConInventarioSuficiente_EnvioConfirmacion()
    {
        // Arrange
        var inventoryService = Substitute.For<IInventoryService>();
        var emailService = Substitute.For<IEmailService>();
        var orderProcessor = new OrderProcessor(inventoryService, emailService);

        var pedido = new Pedido
        {
            Id = 1,
            ClienteNombre = "Cliente 1",
            Articulos = new List<Articulo>
            {
                new Articulo { Id = 1, Nombre = "Producto A", Precio = 10.0m, Cantidad = 1 }
            }
        };

        inventoryService.VerificarInventario(1, 1).Returns(true);

        // Act
        orderProcessor.ProcesarPedido(pedido);

        // Assert
        emailService.Received(1).EnviarConfirmacion(pedido);
        Assert.Equal("Procesado", pedido.Estado);
    }

    [Fact]
    public void ProcesarPedido_ConInventarioInsuficiente_LanzaExcepcion()
    {
        // Arrange
        var inventoryService = Substitute.For<IInventoryService>();
        var emailService = Substitute.For<IEmailService>();
        var orderProcessor = new OrderProcessor(inventoryService, emailService);

        var pedido = new Pedido
        {
            Id = 1,
            ClienteNombre = "Cliente 1",
            Articulos = new List<Articulo>
            {
                new Articulo { Id = 1, Nombre = "Producto A", Precio = 10.0m, Cantidad = 5 }
            }
        };

        inventoryService.VerificarInventario(1, 5).Returns(false);

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => orderProcessor.ProcesarPedido(pedido));
        Assert.Equal("Inventario insuficiente para el artículo: Producto A", exception.Message);
    }

    [Fact]
    public void ProcesarPedido_ConLímiteDeArticulos_EnvioConfirmacion()
    {
        // Arrange
        var inventoryService = Substitute.For<IInventoryService>();
        var emailService = Substitute.For<IEmailService>();
        var orderProcessor = new OrderProcessor(inventoryService, emailService);

        var pedido = new Pedido
        {
            Id = 2,
            ClienteNombre = "Cliente 2",
            Articulos = new List<Articulo>
            {
                new Articulo { Id = 1, Nombre = "Producto A", Precio = 10.0m, Cantidad = 1 },
                new Articulo { Id = 2, Nombre = "Producto B", Precio = 20.0m, Cantidad = 1 }
            }
        };

        inventoryService.VerificarInventario(1, 1).Returns(true);
        inventoryService.VerificarInventario(2, 1).Returns(true);

        // Act
        orderProcessor.ProcesarPedido(pedido);

        // Assert
        emailService.Received(1).EnviarConfirmacion(pedido);
        Assert.Equal("Procesado", pedido.Estado);
    }
}
