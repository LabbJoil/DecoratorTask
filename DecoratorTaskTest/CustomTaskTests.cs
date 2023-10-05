using DecoratorTask.Decorators;
using DecoratorTask.Enums;
using DecoratorTask.Interfaces;
using DecoratorTask.Entities;

namespace DecoratorTaskTest;

[TestClass]
public class CustomTaskTests
{
    private static readonly ITask Task1 = new ExecutionDate(
        new CustomTask(
            new BasicTask("Task 1", "Description about task 1", State.Complete),
            Priority.Priority)
        );

    private static readonly ITask Task2 = new CustomTask(
        new ExecutionDate(
            new BasicTask("Task 2", "Description about task 2", State.Expectation)),
            Priority.Standard
        );

    private static readonly ITask Task3 = new CustomTask(
        new BasicTask("Task 3", "Description about task 3", State.InProcess),
        Priority.NotNecessary
        );

    private static readonly ITask Task4 = new ExecutionDate(
        new CustomTask(
            new BasicTask("Task 4", "Description about task 4", State.Complete),
            Priority.Priority),
        Repeat.EveryWeek,
        DateTime.ParseExact("21.09.2050 12:33", "d.M.yyyy HH:mm", null),
        DateTime.ParseExact("25.09.2050 12:34", "d.M.yyyy HH:mm", null)
        );

    private static readonly ITask Task5 = new CustomTask(
        new ExecutionDate(
            new BasicTask("Task 5", "Description about task 5", State.Complete),
            Repeat.EveryMonth,
            DateTime.ParseExact("21.09.2050 12:33", "d.M.yyyy HH:mm", null),
            DateTime.ParseExact("16.10.2050 12:34", "d.M.yyyy HH:mm", null)
            ), 
        Priority.Priority
        );

    private static readonly List<ITask> TestTasks = new() { Task1, Task2, Task3, Task4, Task5 };

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    [TestMethod]
    public void Constructor_DefaultValues()
    {
        // Arrange
        Priority priorityTask = Priority.Standard;
        bool isArchived = false;

        // Act
        ITask basicTask = new BasicTask();
        CustomTask customTask = new(basicTask);

        // Assert
        Assert.AreEqual(priorityTask, customTask.ConditionPriority);
        Assert.AreEqual(isArchived, customTask.IsArchived);
    }

    [TestMethod]
    public void Constructor_WithValues()
    {
        // Arrange
        Priority priorityTask = Priority.Priority;
        bool isArchived = false;

        // Act
        ITask basicTask = new BasicTask();
        CustomTask customTask = new(basicTask, priorityTask);

        // Assert
        Assert.AreEqual(priorityTask, customTask.ConditionPriority);
        Assert.AreEqual(isArchived, customTask.IsArchived);
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    [TestMethod]
    public void ArchivedTask_Valid()
    {
        // Arrange
        string filePath = AppDomain.CurrentDomain.BaseDirectory;
        string fileName = "testArchivedTask";

        ITask basicTask = new BasicTask();
        ITask executionDate = new ExecutionDate( basicTask);
        CustomTask customTask = new(executionDate);

        // Act
        customTask.ArchivedTask(filePath, fileName);
        File.Delete($"{filePath}\\{fileName}.json");

        // Assert
        Assert.AreEqual(customTask.IsArchived, true);
        Assert.AreEqual(customTask.ArchivedFilePath, $"{filePath}\\{fileName}.json");
    }

    [TestMethod]
    public void GetArchivedTask_ReturnJSON()
    {
        // Arrange
        string filePath = AppDomain.CurrentDomain.BaseDirectory;
        string fileName = "testArchivedTask";
        string expectedRespons = "{\"ConditionPriority\":1,\"IsArchived\":false,\"ArchivedFilePath\":null,\"Task\":{\"Title\":\"New Task\",\"Description\":\"\",\"StateTask\":0},\"Title\":\"New Task\",\"Description\":\"\"}";

        ITask basicTask = new BasicTask();
        CustomTask customTask = new(basicTask);

        // Act
        customTask.ArchivedTask(filePath, fileName);
        string archiveTask = customTask.GetArchivedTask();
        File.Delete($"{filePath}\\{fileName}.json");

        // Assert
        Assert.AreEqual(archiveTask, expectedRespons);
    }

    [TestMethod]
    public void DeleteArchivedTask_Valid()
    {
        string filePath = AppDomain.CurrentDomain.BaseDirectory;
        string fileName = "testArchivedTask";

        ITask basicTask = new BasicTask();
        CustomTask customTask = new(basicTask);

        // Act
        customTask.ArchivedTask(filePath, fileName);
        customTask.ClearArchivedTask();

        // Assert
        Assert.AreEqual(customTask.IsArchived, false);
        Assert.AreEqual(customTask.ArchivedFilePath, string.Empty);
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    [TestMethod]
    public void FilterTasks_ByPriority_ReturnsMatchingTasks()
    {
        // Arrange
        List<ITask> correctTasks = new() { Task1, Task4, Task5 };

        // Act
        List<ITask> filteredTasks = CustomTask.FilterTasksByPriority(TestTasks, Priority.Priority);

        // Assert
        Assert.AreEqual(correctTasks.Count, filteredTasks.Count);
        Assert.IsTrue(correctTasks.SequenceEqual(filteredTasks));
    }

    [TestMethod]
    public void FilterTasks_ByState_ReturnsMatchingTasks()
    {
        // Arrange
        List<ITask> correctTasks = new() { Task3 };

        // Act
        List<ITask> filteredTasks = CustomTask.FilterTasksByStatus(TestTasks, State.InProcess);

        // Assert
        Assert.AreEqual(correctTasks.Count, filteredTasks.Count);
        Assert.IsTrue(correctTasks.SequenceEqual(filteredTasks));
    }

    [TestMethod]
    public void FilterTasks_ByIsArchived_ReturnsMatchingTasks()
    {
        // Arrange
        string filePath = AppDomain.CurrentDomain.BaseDirectory;
        string fileName = "testArchivedTask";

        ITask task = new BasicTask("My Task", "Description about My task", State.Expectation);
        task = new ExecutionDate( task);
        CustomTask customTask = new( task, Priority.Standard);
        customTask.ArchivedTask(filePath, fileName);

        List<ITask> correctTasks = new() { customTask };
        List<ITask> testTasks = correctTasks.Concat(TestTasks).ToList();

        // Act
        List<ITask> filteredTasks = CustomTask.FilterTasksByIsArchived(testTasks, true);
        File.Delete($"{filePath}\\{fileName}.json");

        // Assert
        Assert.AreEqual(correctTasks.Count, filteredTasks.Count);
        Assert.IsTrue(correctTasks.SequenceEqual(filteredTasks));
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    [TestMethod]
    public void FilterTasks_ByStartDate_ReturnsMatchingTasks()
    {
        // Arrange
        DateTime startDate = DateTime.ParseExact("21.09.2050 12:33", "d.M.yyyy HH:mm", null);
        List<ITask> correctTasks = new() { Task4, Task5 };

        // Act
        List<ITask> filteredTasks = CustomTask.FilterTasksByStartDate(TestTasks, startDate);

        // Assert
        Assert.AreEqual(correctTasks.Count, filteredTasks.Count);
        Assert.IsTrue(correctTasks.SequenceEqual(filteredTasks));
    }

    [TestMethod]
    public void FilterTasks_ByEndDate_ReturnsMatchingTasks()
    {
        // Arrange
        DateTime endDate = DateTime.Now.AddHours(2);
        List<ITask> correctTasks = new() { Task1, Task2 };

        // Act
        List<ITask> filteredTasks = CustomTask.FilterTasksByEndDate(TestTasks, endDate);

        // Assert
        Assert.AreEqual(correctTasks.Count, filteredTasks.Count);
        Assert.IsTrue(correctTasks.SequenceEqual(filteredTasks));
    }

    [TestMethod]
    public void FilterTasks_ByRepeatTask_ReturnsMatchingTasks()
    {
        // Arrange
        Repeat oftenRepeat = Repeat.EveryMonth;
        List<ITask> correctTasks = new() { Task5 };

        // Act
        List<ITask> filteredTasks = CustomTask.FilterTasksByRepeatTask(TestTasks, oftenRepeat);

        // Assert
        Assert.AreEqual(correctTasks.Count, filteredTasks.Count);
        Assert.IsTrue(correctTasks.SequenceEqual(filteredTasks));
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    [TestMethod]
    public void Info_ReturnsMatchingTasks()
    {
        // Arrange
        DateTime nowDateTime = DateTime.Now;
        DateTime dateStartTask = new DateTime(nowDateTime.Year, nowDateTime.Month, nowDateTime.Day, nowDateTime.Hour, nowDateTime.Minute, 0).AddHours(1);
        DateTime dateEndTask = dateStartTask.AddHours(1);

        string expectedInfoTask2 = $"Priority: Standard, Is Archived: False, Archived File Path:  | " +
            $"TimeStart: {dateStartTask}, TimeEnd: {dateEndTask}, OftenRepeat: None | " +
            $"Id: {Task2.GetId()}, Titel: Task 2, Description: Description about task 2, State: Expectation | ";

        // Act
        string returnInfoTask2 = Task2.Info();

        // Assert
        Assert.IsTrue(expectedInfoTask2 == returnInfoTask2);
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    [TestMethod]
    public void GetCustomTask_ReturnsCustomTask()
    {
        // Arrange & Act
        CustomTask? task = CustomTask.GetCustomTask(Task1);

        // Assert
        Assert.IsNotNull(task);
    }

    [TestMethod]
    public void GetExecutionDateTask_ReturnsCustomTask()
    {
        // Arrange & Act
        ExecutionDate? task = CustomTask.GetExecutionDateTask(Task2);

        // Assert
        Assert.IsNotNull(task);
    }
}