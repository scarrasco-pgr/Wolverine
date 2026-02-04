using Wolverine.API.Features;

namespace WolverineAPI.Tests.Features
{
    public class CreateTodoTests
    {
        [Fact]
        public void CreateTodo_ShouldReturnCreationResponse_WithGeneratedId()
        {
            // Arrange
            var command = new CreateTodo("Buy groceries");

            // Act
            var (response, _, _) = CreateTodoEndpoint.CreateTodo(command);

            // Assert
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Equal($"/todos/{response.Id}", response.Url);
        }

        [Fact]
        public void CreateTodo_ShouldReturnTodoCreatedEvent_WithCorrectData()
        {
            // Arrange
            var description = "Complete project documentation";
            var command = new CreateTodo(description);

            // Act
            var (_, _, todoCreated) = CreateTodoEndpoint.CreateTodo(command);

            // Assert
            Assert.NotEqual(Guid.Empty, todoCreated.Id);
            Assert.Equal(description, todoCreated.Description);
            Assert.False(todoCreated.Completed);
        }

        [Fact]
        public void CreateTodo_ShouldReturnConsistentId_AcrossAllReturnValues()
        {
            // Arrange
            var command = new CreateTodo("Review pull requests");

            // Act
            var (response, _, todoCreated) = CreateTodoEndpoint.CreateTodo(command);

            // Assert
            Assert.Equal(response.Id, todoCreated.Id);
        }

        [Fact]
        public void CreateTodo_ShouldGenerateUniqueIds_ForMultipleCalls()
        {
            // Arrange
            var command1 = new CreateTodo("Task 1");
            var command2 = new CreateTodo("Task 2");

            // Act
            var (response1, _, _) = CreateTodoEndpoint.CreateTodo(command1);
            var (response2, _, _) = CreateTodoEndpoint.CreateTodo(command2);

            // Assert
            Assert.NotEqual(response1.Id, response2.Id);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Simple task")]
        [InlineData("Task with special characters: @#$%")]
        public void CreateTodo_ShouldPreserveDescription_FromCommand(string description)
        {
            // Arrange
            var command = new CreateTodo(description);

            // Act
            var (_, _, todoCreated) = CreateTodoEndpoint.CreateTodo(command);

            // Assert
            Assert.Equal(description, todoCreated.Description);
        }
    }
}
