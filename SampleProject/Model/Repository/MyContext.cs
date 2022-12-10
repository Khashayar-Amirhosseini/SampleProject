using Microsoft.EntityFrameworkCore;
using SampleProject.Model.Entity;


namespace SampleProject.Model.Repository
{
    public class MyContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle("Data Source=localhost;User ID=university;Password=1111;", b =>
            b.UseOracleSQLCompatibility("11"));
        }
         
        
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Password> Password { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleClient> RoleClients { get; set; }
        
        
      
    }
}
