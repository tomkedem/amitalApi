using System.Data;
using System.Threading.Tasks;
using System;
using amitalTest.Models;
using amitalTest.Helpers;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace amitalTest.Repositories
{
    public class UsersRepository : IUsersRepository
    {

        private readonly IDbConnection _dbConnection;

        public UsersRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            string queryString = @"SELECT  [id]
                                      ,[fullName]
                                  FROM [TodoSystem].[dbo].[users]";

            var todoList = await DbHelper.ExecuteReaderAsync<Users>(_dbConnection as SqlConnection, queryString);

            return todoList;
        }

    }
}
