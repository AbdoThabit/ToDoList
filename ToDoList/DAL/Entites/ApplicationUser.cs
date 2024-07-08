using Microsoft.AspNetCore.Identity;

namespace ToDoList.DAL.Entites
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public virtual List<JobTask> tasks { get; set; } =  new List<JobTask>();

    }
}
