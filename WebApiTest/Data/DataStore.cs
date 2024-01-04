using WebApiTest.Model;

namespace WebApiTest.Data
{
    public class DataStore
    {
        public List<City> Cities { get; set; }

        public DataStore()
        {
            Cities = new List<City>() 
            { 
                new City()
                { 
                    Id = 1,
                    Name = "Chicago",
                    Description = "The most populous city in the Midwest region of the United States.",
                    Buildings = new List<Building>
                    {
                        new Building
                        {
                            Id = 1,
                            Name = "Willis Tower",
                            Description = "Built in 1973, the 1450ft tower was once the world's tallest building."
                        },
                        new Building
                        {
                            Id = 2,
                            Name = "Aqua",
                            Description = "Completed in 2009, the 859ft building features dramatic undulating balconies reminiscent of a waterfall."
                        }
                    }
                },
                   new City()
                {
                    Id = 2,
                    Name = "Paris",
                    Description = "The capitol and most populous city of France.",
                    Buildings = new List<Building>
                    {
                        new Building
                        {
                            Id = 1,
                            Name = "Eiffel Tower",
                            Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel."
                        }
                    }
                },
                      new City()
                {
                    Id = 3,
                    Name = "Hamburg",
                    Description = "The second largest city in Germany.",
                },
            };
        }
    }
}
