using DecoratorTask.Enums;

namespace DecoratorTask.Interfaces
{
    public interface ITask
    {
        string Title { get; set; }
        string Description { get; set; }
        State StateTask { get; set; }
        int GetId();
        string Info();
        void DeleateTask(ref ITask deleteTask);
        void ChangeTask(ITask lastTask, ITask newTask);
    }
}
