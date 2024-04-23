using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniShoppingCartAPI.Entities;

namespace MiniShoppingCartAPI.nUnitTests.ServicesTests
{
    internal class UserRepository
    {
        public async Task<List<User>> GetUsersAsync()
        {
            // Simulate an asynchronous delay
            await Task.Delay(100);

            // Simulate retrieving users from a data source asynchronously
            return new List<User>
            {
                new User
                {
                    Username = "test-user1",
                    Password = "password1",
                    Uid = Guid.NewGuid()
                },
                new User
                {
                    Username = "test-user2",
                    Password = "password2",
                    Uid = Guid.NewGuid()
                },
                // Add more users as needed
            };
        }
    }
}
