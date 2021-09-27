using System.ComponentModel.DataAnnotations;
using YamlDotNet.Serialization;

namespace Backend.Models
{
    public class Player : IEntity
    {
        [Key]
        [YamlIgnore]
        public int Id { get; set; }
        
        [Required, MinLength(2), MaxLength(50)]
        public string Name { get; set; }
    }
}