namespace ms
{
    class ManagementSystem
    {
        static void Main(string[] args)
        {
            IManagementManager mm = new ManagementManager();
            mm.startListening();
            while(true)
            {

            }
        }
    }
}
