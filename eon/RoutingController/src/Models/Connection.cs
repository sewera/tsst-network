namespace RoutingController.Models
{
    public class Connection
    {
        public int Id { get; }
        public (int, int) Slots { get; }  

        public Connection(int id, (int, int) slots)
        {
            Id = id;
            Slots = slots;
        }
        
        public override string ToString()
        {
            return $"[id: {Id}, slots: {Slots.ToString()}";
        }
    }
}
