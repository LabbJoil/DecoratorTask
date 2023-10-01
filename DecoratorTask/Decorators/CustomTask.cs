using DecoratorTask.Enriched;
using DecoratorTask.Enums;
using DecoratorTask.Interfaces;

namespace DecoratorTask.Decorators;

public class CustomTask : TaskEnhancer
{
    public Priority ConditionPriority { get; set; }
    public bool IsArchived;

    public CustomTask(ref ITask? task, Priority conditionPriority = Priority.Standard, bool isArchived = false) : base(task)
    {
        ConditionPriority = conditionPriority;
        IsArchived = isArchived;
        ChangeTask(task, this);
        task = null;
    }

    public static List<ITask> FilterTasksByPriority(List<ITask> sourceList, Priority necessaryPriority)
    {
        List<ITask> filterListTask = new();
        foreach (ITask task in sourceList)
        {
            CustomTask? priorityTask = GetPriorityTask(task);
            if (priorityTask == null || priorityTask.ConditionPriority != necessaryPriority) continue;
            filterListTask.Add(task);
        }
        return filterListTask;
    }

    public static List<ITask> FilterTasksByStatus(List<ITask> sourceList, State stateTask)
    {
        List<ITask> filterListTask = new();
        foreach (ITask task in sourceList)
        {
            if(task.GetState() != stateTask) continue;
            filterListTask.Add(task);
        }
        return filterListTask;
    }

    public static List<ITask> FilterTasksByIsArchived(List<ITask> sourceList, bool isArchived)
    {
        List<ITask> filterListTask = new();
        foreach (ITask task in sourceList)
        {
            CustomTask? priorityTask = GetPriorityTask(task);
            if (priorityTask == null || priorityTask.IsArchived != isArchived) continue;
            filterListTask.Add(task);
        }
        return filterListTask;
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    public static List<ITask> FilterTasksByStartDate(List<ITask> sourceList, DateTime startDate)
    {
        List<ITask> filterListTask = new();
        foreach (ITask task in sourceList)
        {
            ExecutionDate? executionDateTask = GetExecutionDateTask(task);
            if (executionDateTask == null || !EqualDateWithoutSeconds(executionDateTask.DateStartTask, startDate)) continue;
            filterListTask.Add(task);
        }
        return filterListTask;
    }

    public static List<ITask> FilterTasksByEndDate(List<ITask> sourceList, DateTime endDate)
    {
        List<ITask> filterListTask = new();
        foreach (ITask task in sourceList)
        {
            ExecutionDate? executionDateTask = GetExecutionDateTask(task);
            if (executionDateTask == null || !EqualDateWithoutSeconds(executionDateTask.DateEndTask, endDate)) continue;
            filterListTask.Add(task);
        }
        return filterListTask;
    }

    public static List<ITask> FilterTasksByRepeatTask(List<ITask> sourceList, Repeat oftenRepeat)
    {
        List<ITask> filterListTask = new();
        foreach (ITask task in sourceList)
        {
            ExecutionDate? executionDateTask = GetExecutionDateTask(task);
            if (executionDateTask == null || executionDateTask.OftenRepeat != oftenRepeat) continue;
            filterListTask.Add(task);
        }
        return filterListTask;
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    public static CustomTask? GetPriorityTask(ITask task)
    {
        if (task is CustomTask statusTask) return statusTask;
        else if (task is TaskEnhancer taskEnhancer)
        {
            return GetPriorityTask(taskEnhancer.Task);
        }
        return null;
    }

    public static ExecutionDate? GetExecutionDateTask(ITask task)
    {
        if (task is ExecutionDate statusTask) return statusTask;
        else if (task is TaskEnhancer taskEnhancer)
        {
            return GetExecutionDateTask(taskEnhancer.Task);
        }
        return null;
    }

    private static bool EqualDateWithoutSeconds(DateTime firstDate, DateTime secondDate)
    {
        bool datesEqual = firstDate.Year == secondDate.Year &&
                firstDate.Month == secondDate.Month &&
                firstDate.Day == secondDate.Day &&
                firstDate.Hour == secondDate.Hour &&
                firstDate.Minute == secondDate.Minute;
        return datesEqual;
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    public override string Info()
        => $"Priority: {ConditionPriority}, Is Archived: {IsArchived} | " + Task.Info();

    public override void CompleteTask()
        => Task.CompleteTask();
}
