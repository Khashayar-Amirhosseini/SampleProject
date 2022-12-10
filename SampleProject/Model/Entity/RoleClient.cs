using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleProject.Model.Entity
{
    [Table(name:"ROLES_CLIENTS")]
    public class RoleClient
    {
        [Key]
        public long ID { get; set; }
        [Column(name:"CLIENT_ID")]
        public long ClientId { get; set; }
        public Client client { get; set; }
        [Column(name:"ROLE_ID")]
        public long RoleId { get; set; }
        public Role Role { get; set; }

    }
}
