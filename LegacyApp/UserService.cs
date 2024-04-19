using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId) //cała logika w jednym miejscu z wywołaniem zewnętrznych metod
        {
            if (!IsValidName(firstName, lastName) || !IsValidEmail(email) || !IsAdult(dateOfBirth))
            {
                return false;
            }

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = CreateUser(client, firstName, lastName, email, dateOfBirth);

            SetCreditLimit(user, client);

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            SaveUser(user);
            return true;
        }

        private bool IsValidName(string firstName, string lastName)
        {
            return !string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName);
        }

        private bool IsValidEmail(string email)
        {
            return !string.IsNullOrEmpty(email) && email.Contains("@") && email.Contains(".");
        }

        private bool IsAdult(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }

            return age >= 21;
        }

        private User CreateUser(Client client, string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
            return user;
        }

        private void SetCreditLimit(User user, Client client)
        {
            using (var userCreditService = new UserCreditService())
            {
                if (client.Type == "VeryImportantClient")
                {
                    user.HasCreditLimit = false;
                }
                else
                {
                    user.HasCreditLimit = true;
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    if (client.Type == "ImportantClient")
                    {
                        creditLimit *= 2;
                    }

                    user.CreditLimit = creditLimit;
                }
            }
        }

        private void SaveUser(User user)
        {
            UserDataAccess.AddUser(user);
        }
    }
}