using DecoratorTask.Enums;
using DecoratorTask.Interfaces;

namespace DecoratorTask.Enriched;

public abstract class TaskEnhancer : ITask
{
    public ITask Task { get; private set; }
    public string Title { get => Task.Title; set => Task.Title = value; }
    public string Description { get => Task.Description; set => Task.Description = value; }

    public TaskEnhancer(ITask task) => Task = task ;

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    public int GetId()
        => Task.GetId();

    public State GetState()
        => Task.GetState();

    public void ChangeState(State newState)
        => Task.ChangeState(newState);

    public abstract string Info();

    public abstract void CompleteTask();
}
