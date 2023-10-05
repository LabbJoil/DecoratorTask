using DecoratorTask.Enums;
using DecoratorTask.Interfaces;

namespace DecoratorTask.Entities;

public class BasicTask : ITask
{
    private readonly int Id;

    public string Title { get; set; }
    public string Description { get; set; }
    public State StateTask { get; private set; }

    public BasicTask()
    {
        Title = "New Task";
        Description = string.Empty;
        Id = GetHashCode();
        StateTask = State.Expectation;
    }

    public BasicTask(string name, string description, State state = State.Expectation)
    {
        Title = name;
        Description = description;
        Id = GetHashCode();
        StateTask = state;
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    public int GetId() => Id;
    public State GetState() => StateTask;

    public string Info()
        => $"Id: {Id}, Titel: {Title}, Description: {Description}, State: {StateTask} | ";

    public void CompleteTask()
        => ChangeState(State.Complete);

    public void ChangeState(State newState)
        => StateTask = newState;
}
