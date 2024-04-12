using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserCreditService _userCreditService;

        public UserService()
        {
            _clientRepository = new ClientRepository();
            _userCreditService = new UserCreditService();
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!ValidateUserInput(firstName, lastName, email, dateOfBirth))
                return false;

            var client = _clientRepository.GetById(clientId);
            if (client == null)
                return false;

            var user = CreateUser(firstName, lastName, email, dateOfBirth, client);

            SetCreditLimit(user);

            if (!CheckCreditLimit(user))
                return false;

            UserDataAccess.AddUser(user);
            return true;
        }

        private bool ValidateUserInput(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                return false;

            if (!email.Contains("@") || !email.Contains("."))
                return false;

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
                age--;

            return age >= 21;
        }

        private User CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, Client client)
        {
            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else
            {
                user.HasCreditLimit = true;
            }

            return user;
        }

        private void SetCreditLimit(User user)
        {
            if (!user.HasCreditLimit)
                return;

            int creditLimit = _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
            if (user.Client == "ImportantClient")
                creditLimit *= 2;

            user.CreditLimit = creditLimit;
        }

        private bool CheckCreditLimit(User user)
        {
            return !user.HasCreditLimit || user.CreditLimit >= 500;
        }
    }
}
