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
    public void GetId_ReturnsValidId()
    {
        // Arrange
        BasicTask basicTask = new();

        // Act
        int id = basicTask.GetId();

        // Assert
        Assert.IsTrue(id > 0);
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
        basicTask.ChangeState(newState);

        // Assert
        Assert.AreEqual(newState, basicTask.StateTask);
    }
}
