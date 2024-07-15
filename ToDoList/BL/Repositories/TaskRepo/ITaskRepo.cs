using ToDoList.DTOs;

namespace ToDoList.BL.Repositories.TaskRepo
{
    public interface ITaskRepo
    {
        public Task<List<TaskdetailsDto>> GetAll();
        public Task<TaskdetailsDto> getById(int id);
        public Task<List<TaskdetailsDto>> getUserTasks(String ID);
        public Task<int> Add(string userId, TaskDto dto);
        public Task<int> Edit(int taskId, TaskUpdateDto dto);
        public Task<int> Delete(int taskId);
    }
}
