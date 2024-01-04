using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiTest.Data.Entities
{
    public class BuildingEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public CityEntity? City { get; set; }
        public int CityId { get; set; }

        //public string Description { get; set; } = string.Empty;

        public BuildingEntity(string name)
        {
            Name = name;
        }
    }
}
