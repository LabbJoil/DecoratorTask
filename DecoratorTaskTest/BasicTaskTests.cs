using DecoratorTask.Decorators;
using DecoratorTask.Entities;
using DecoratorTask.Enums;
using DecoratorTask.Interfaces;

namespace DecoratorTaskTest;
[TestClass]
public class BasicTaskTests
{
    [TestMethod]
    public void Constructor_DefaultValues()
    {
        // Act
        BasicTask basicTask = new();

        // Assert
        Assert.AreEqual("New Task", basicTask.Title);
        Assert.AreEqual(string.Empty, basicTask.Description);
        Assert.AreEqual(State.Expectation, basicTask.StateTask);
    }

    [TestMethod]
    public void Constructor_WithValues()
    {
        // Arrange
        string name = "Test Task";
        string description = "Test Description";
        State state = State.InProcess;

        // Act
        BasicTask basicTask = new(name, description, state);

        // Assert
        Assert.AreEqual(name, basicTask.Title);
        Assert.AreEqual(description, basicTask.Description);
        Assert.AreEqual(state, basicTask.StateTask);
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    [TestMethod]
    public void GetId_ReturnsId()
    {
        // Arrange
        int idTask = 11111;
        BasicTask basicTask = new("new task", "about new task", State.Expectation, idTask);

        // Act
        int id = basicTask.Id;

        // Assert
        Assert.IsTrue(id == idTask);
    }

    [TestMethod]
    public void GetState_ReturnsState()
    {
        // Arrange
        BasicTask basicTask = new();
        basicTask.CompleteTask();

        // Act
        State state = basicTask.GetState();

        // Assert
        Assert.AreEqual(state, State.Complete);
    }

    [TestMethod]
    public void CompleteTask_ChangeStateToComplete()
    {
        // Arrange
        State newState = State.Complete;
        BasicTask basicTask = new();

        // Act
        basicTask.CompleteTask();

        // Assert
        Assert.AreEqual(newState, basicTask.StateTask);
    }

    [TestMethod]
    public void ChangeState_ChangeStateInProcess()
    {
        // Arrange
        State newState = State.InProcess;
        BasicTask basicTask = new();

        // Act
        basicTask.StateTask = newState;

        // Assert
        Assert.AreEqual(newState, basicTask.StateTask);
    }
}
