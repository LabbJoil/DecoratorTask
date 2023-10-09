using DecoratorTask.Enums;
using DecoratorTask.Interfaces;

namespace DecoratorTask.Entities;

public class BasicTask : ITask
{
    private readonly int IdTask;

    public int Id { get => IdTask; }
    public string Title { get; set; }
    public string Description { get; set; }
    public State StateTask { get; private set; }

    public BasicTask(int id = -1)
    {
        Title = "New Task";
        Description = string.Empty;
        IdTask = id == -1 ? GetHashCode() : id;
        StateTask = State.Expectation;
    }

    public BasicTask(string name, string description, State state = State.Expectation, int id = -1)
    {
        Title = name;
        Description = description;
        IdTask = id == -1 ? GetHashCode() : id;
        StateTask = state;
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    public State GetState() => StateTask;

    public string Info()
        => $"Id: {Id}, Titel: {Title}, Description: {Description}, State: {StateTask} | ";

    public void CompleteTask()
        => ChangeState(State.Complete);

    public void ChangeState(State newState)
        => StateTask = newState;
}
