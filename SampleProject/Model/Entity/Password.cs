using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleProject.Model.Entity
{
    [Table(name:"PASSWORD")]
    public class Password
    {
        [Key]     
        public long Id { get; set; }
        [Column(name:"PRIMARY_KEY",TypeName ="VARCHAR2(100)")]
        public string PrimaryKey { get; set; }
        [Column(name: "PASSWORDS", TypeName = "VARCHAR2(100)")]
        public string Passwords { get; set; }

        public Client Clients { get; set; }
    }
}
