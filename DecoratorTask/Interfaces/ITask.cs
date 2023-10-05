using DecoratorTask.Enums;

namespace DecoratorTask.Interfaces;

public interface ITask
{
    public string Title { get; set; }
    public string Description { get; set; }

    public int GetId();
    public State GetState();

    public string Info();
    public void CompleteTask();

    internal void ChangeState(State newState);
}
