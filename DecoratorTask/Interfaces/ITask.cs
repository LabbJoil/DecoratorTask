using DecoratorTask.Enums;

namespace DecoratorTask.Interfaces;

public interface ITask
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int GetId();
    public State GetState();

    public string Info();
    public void DeleateTask(ref ITask deleteTask);
    public void CompleteTask();

    internal void ChangeTask(ITask lastTask, ITask newTask);
    internal void ChangeState(State newState);
}
