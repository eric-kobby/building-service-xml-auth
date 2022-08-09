using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwareDevelopmentTest.DAL.Entities
{
    #nullable disable
    public class Fixture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int Xaxis { get; set; }

        [Required]
        public int Yaxis { get; set; }

        [Required]
        [StringLength(50)]
        public string MacAddress { get; set; }

        [Required]
        public int GroupId { get; set; }

        public int? AreaId { get; set; }

        [Required]
        public int FloorId { get; set; }

        [ForeignKey(nameof(FloorId))]

        public Floor Floor { get; set; }
    }
}