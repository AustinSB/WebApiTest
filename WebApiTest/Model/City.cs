namespace WebApiTest.Model
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<Building> Buildings { get; set; } = new List<Building>();
    }
}
