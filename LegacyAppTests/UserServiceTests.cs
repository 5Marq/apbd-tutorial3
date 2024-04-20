using LegacyApp;

namespace LegacyAppTests;

public class UserServiceTests
{
    [Fact]
    public void AddUser_Should_Return_False_When_Email_Without_At_And_Dot()
    {
        //Arrange
        string firstName = "John";
        string lastName = "Doe";
        DateTime dateOfBirth = new DateTime(1990, 1, 1);
        string email = "doe";
        int clientId = 1;
        var userService = new UserService();
        //Act
        bool result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        //Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_User_Is_Under_21()
    {
        //Arrange
        string firstName = "John";
        string lastName = "Doe";
        DateTime dateOfBirth = new DateTime(2020, 1, 1); //client is 4 years old
        string email = "doe@gmail.pl";
        int clientId = 1;
        var userService = new UserService();
        //Act
        bool result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        //Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_Name_Is_Invalid()
    {
        //Arrange
        string firstName = null;
        string lastName = "Kowalski";
        DateTime dateOfBirth = new DateTime(1990, 1, 1);
        string email = "jankowals";
        var userService = new UserService();
        int clientId = 1;
        //Act
        bool result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        //Assert
        Assert.Equal(false, result);
    }
    
    [Fact]
    public void AddUser_Should_Return_True_When_Client()
    {
        //Arrange
        string firstName = "John";
        string lastName = "Doe";
        DateTime dateOfBirth = new DateTime(1990, 1, 1);
        string email = "doe@gmail.pl";
        var userService = new UserService();
        int clientId = 4;
        //Act
        bool result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        //Assert
        Assert.Equal(true, result);
    }
}