using DecoratorTask.Decorators;
using DecoratorTask.Entities;
using DecoratorTask.Enums;
using DecoratorTask.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorTaskTest
{
    [TestClass]
    public class BasicTaskTests
    {
        [TestCleanup]
        public void Setup()
        {
            BasicTask.AllTask.Clear();
        }

        [TestMethod]
        public void Constructor_DefaultValues_AssignsCorrectly()
        {
            // Act
            BasicTask basicTask = new();

            // Assert
            Assert.AreEqual("New Task", basicTask.Title);
            Assert.AreEqual(string.Empty, basicTask.Description);
            Assert.AreEqual(State.Expectation, basicTask.StateTask);
            Assert.IsTrue(BasicTask.AllTask.Contains(basicTask));
        }

        [TestMethod]
        public void Constructor_WithValues_AssignsCorrectly()
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
            Assert.IsTrue(BasicTask.AllTask.Contains(basicTask));
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void DeleateTask_RemovesTaskFromAllTask()
        {
            // Arrange
            ITask basicTask = new BasicTask();

            // Act
            basicTask.DeleateTask(ref basicTask);

            // Assert
            Assert.IsFalse(BasicTask.AllTask.Contains(basicTask));
        }

        [TestMethod]
        public void DeleateTask_NonExistentTask_ThrowsException()
        {
            // Arrange
            ITask basicTask = new BasicTask();
            ITask customTask = new CustomTask(ref basicTask);

            // Act
            customTask.DeleateTask(ref customTask);

            // Act & Assert
            Assert.ThrowsException<NullReferenceException>(() => customTask.DeleateTask(ref basicTask));
            Assert.AreEqual(customTask, null);
            Assert.AreEqual(basicTask, null);
        }

        [TestMethod]
        public void ChangeTask_ReplacesTaskInAllTask()
        {
            // Arrange
            BasicTask oldTask = new();
            BasicTask newTask = new();

            // Act
            oldTask.ChangeTask(oldTask, newTask);

            // Assert
            Assert.IsFalse(BasicTask.AllTask.Contains(oldTask));
            Assert.IsTrue(BasicTask.AllTask.Contains(newTask)); ///////////////////////////////////
        }

        [TestMethod]
        public void ChangeTask_NonExistentOldTask_ThrowsException()
        {
            // Arrange
            ITask oldTask = new BasicTask();
            CustomTask newTask = new (ref oldTask);

            // Act & Assert
            Assert.ThrowsException<Exception>(() => newTask.ChangeTask(oldTask, newTask));
            Assert.AreEqual(oldTask, null);
        }

        [TestMethod]
        public void ChangeTask_AddsNewTaskToAllTask()
        {
            // Arrange
            BasicTask oldTask = new();
            BasicTask newTask = new();

            // Act
            oldTask.ChangeTask(oldTask, newTask);

            // Assert
            Assert.IsTrue(BasicTask.AllTask.Contains(newTask));
        }

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
    }
}
