using DecoratorTask.Enums;

namespace DecoratorTask.Interfaces;

public interface ITask
{
    public int Id { get; }
    public string Title { get; set; }
    public string Description { get; set; }
    public State StateTask { get; internal set; }

    public string Info();
    public void CompleteTask();

    //internal void ChangeState(State newState);
}
