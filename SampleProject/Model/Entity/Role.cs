using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleProject.Model.Entity
{
    [Table(name: "ROLES")]
    public class Role
    {
        [Key]
        public long ID { get; set; }
        [Required(ErrorMessage ="Role Name is Required")]
        [Column(name:"NAME",TypeName ="VARCHAR(100)")]
        public string Name { get; set; }
        public IList<RoleClient> RoleClients { get; set; }

    }
}
