using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwareDevelopmentTest.DAL.Entities
{
    public class Floor
    {
        public Floor()
        {
            Fixtures = new HashSet<Fixture>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Campus { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public string FloorPlanUrl { get; set; }
        public int? ParentFloorId { get; set; }

        public ICollection<Fixture> Fixtures { get; set; }
    }
}

