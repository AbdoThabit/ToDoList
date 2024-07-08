using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoList.DAL.Entites;

namespace ToDoList.DAL.DataBase
{
    public class Context : IdentityDbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        public virtual DbSet<JobTask> Tasks { get; set; }
    }
}
