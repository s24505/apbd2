using LegacyApp;
using System.ComponentModel.DataAnnotations;

namespace LegacyAppTest
{
    public class UserServiceTest
    {
        [Fact]
        public void AddUser_Should_Return_False_When_Email_Without_At_And_Dopt()
        {
            //arrange
            string firstName = "John";
            string lastName = "Doe";
            DateTime birthDate = new DateTime(year: 1990, month: 1, day: 1);
            int clientId = 1;
            string email = "doe";
            var service = new UserService();

            //act
            bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);

            //assert
            Assert.Equal(false, result);
        }
        [Fact]
        public void AddUser_Should_Return_False_When_Age_Less_Than_21()
        {
            string firstName = "John";
            string lastName = "Doe";
            DateTime birthDate = new DateTime(year: 2005, month: 1, day: 1);
            int clientId = 1;
            string email = "doe@gmail.com";
            var service = new UserService();

            bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);

            Assert.Equal(false, result);
        }
    }
}