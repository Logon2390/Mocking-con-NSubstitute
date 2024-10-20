public class Pedido
{
    public int Id { get; set; }
    public string ClienteNombre { get; set; }
    public List<Articulo> Articulos { get; set; }
    public string Estado { get; set; }
    public DateTime FechaCreacion { get; set; }

    public Pedido()
    {
        Articulos = new List<Articulo>();
        Estado = "Pendiente"; // Estado inicial
        FechaCreacion = DateTime.Now;
    }
}
