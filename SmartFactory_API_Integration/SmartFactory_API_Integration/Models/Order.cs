namespace SmartFactory_ITOT_Integration.Models
{
    public class Order
    {
        public int Id { get; set; }  // Unique identifier for the order
        public int ProductId { get; set; }  // ID of the product being ordered
        public int Quantity { get; set; }  // Quantity of the product ordered
        public DateTime OrderDate { get; set; }  // Date when the order was placed
    }
}
