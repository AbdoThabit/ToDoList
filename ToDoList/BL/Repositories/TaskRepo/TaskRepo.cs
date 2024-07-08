using ToDoList.DTOs;
using ToDoList.DAL.DataBase;
using Microsoft.AspNetCore.Identity;
using ToDoList.DAL.Entites;
using Microsoft.EntityFrameworkCore;
namespace ToDoList.BL.Repositories.TaskRepo
{
    public class TaskRepo : ITaskRepo
    {
        private  Context _context;
        private  UserManager<ApplicationUser> _userManager;
        private ApplicationUser User;
        public TaskRepo(Context context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<int> Add(TaskDto dto)
        {
            JobTask jobTask = new JobTask()
            {
                Title = dto.Title,
                Description = dto.Description,
                Type = dto.Type,
                date = dto.Created,
                UserId = dto.UserID,
            };
            await _context.Tasks.AddAsync(jobTask);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<int> Delete(int taskId)
        {

            var TaskDB = await _context.Tasks.FindAsync(taskId);

            if (TaskDB != null)
            {
                _context.Entry(TaskDB).State = EntityState.Deleted;


                return await _context.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> Edit(int taskId,TaskUpdateDto taskdto)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task != null) 
            {
                _context.Entry(task).CurrentValues.SetValues(taskdto);
                _context.Entry(task).State = EntityState.Modified;
                return await _context.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }

        public async Task<List<TaskdetailsDto>> GetAll()
        {
            List<JobTask> allTasks = await _context.Tasks.ToListAsync();
            if(allTasks.Count > 0)
            {
                List<TaskdetailsDto> allTasksWitDetails = new List<TaskdetailsDto>();
                foreach(var task in allTasks)
                {
                    User = await _userManager.FindByIdAsync(task.UserId);
                    TaskdetailsDto taskdetailsDto = new TaskdetailsDto()
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Created = task.date,
                        FullName = User.FullName,
                        UserName = User.UserName,
                        
                    };
                    allTasksWitDetails.Add(taskdetailsDto);
                }
                return allTasksWitDetails;
            }
            else
            {
                return null;
            }
        }

        public async Task<TaskdetailsDto> getById(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                User = await _userManager.FindByIdAsync(task.UserId);
                TaskdetailsDto taskdetailsDto = new TaskdetailsDto()
                {
                    Id = task.Id,
                    UserName = User.UserName,
                    FullName = User.FullName,
                    Title = task.Title,
                    Description = task.Description,
                    Created = task.date,
                    Type =task.Type
                };
                return taskdetailsDto;
            }
            else return null;
        }

        public async Task<List<TaskdetailsDto>> getUserTasks(string ID)
        {
            List<JobTask> allTasks = await _context.Tasks.Where(t =>t.UserId ==ID).ToListAsync();
            if (allTasks.Count > 0)
            {
                List<TaskdetailsDto> allTasksWitDetails = new List<TaskdetailsDto>();
                foreach (var task in allTasks)
                {
                    User = await _userManager.FindByIdAsync(task.UserId);
                    TaskdetailsDto taskdetailsDto = new TaskdetailsDto()
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Created = task.date,
                        FullName = User.FullName,
                        UserName = User.UserName,

                    };
                    allTasksWitDetails.Add(taskdetailsDto);
                }
                return allTasksWitDetails;
            }
            else
            {
                return null;
            }
        }
    }
}
