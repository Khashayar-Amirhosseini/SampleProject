using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Collections.ObjectModel;

namespace SampleProject.Model.Entity
{
    [Table(name:"CLIENTS")]
    public class Client
    {
        
        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage ="Name is Required")]
        [Column("NAME",TypeName ="VARCHAR(100)")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Family is Required")]
        [Column("FAMILY", TypeName = "VARCHAR(100)")]
        public string Family { get; set; }
        [Required]
        [Column("EMAIL", TypeName = "VARCHAR(100)")]
      
        public string Email { get; set; }
        [Required]
        [Column("PHONE", TypeName = "VARCHAR(10)")]
        public string Phone { get; set; }
        [Column("JOIN_DATE",TypeName ="TIMESTAMP")]
        public DateTime joinDate { get; set; }
        [Column("IS_ACTIVATED",TypeName = "BINARY_DOUBLE")]
        public bool IsActivated { get; set; }
        public IList<RoleClient> RoleClients { get; set; }

    }
}
