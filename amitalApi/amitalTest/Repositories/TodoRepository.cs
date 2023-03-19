using amitalTest.Helpers;
using amitalTest.Models;
using Azure.Core;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Data.Common;
using System.Collections.Generic;

namespace amitalTest.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IDbConnection _dbConnection;

        public TodoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<IEnumerable<ItemTodo>> GetTaskList()
        {
            string queryString = @"SELECT dbo.todoList.id
	                                    ,dbo.todoList.description
	                                    ,dbo.todoList.done
	                                    ,dbo.users.fullName
                                    FROM dbo.todoList
                                    INNER JOIN dbo.users ON dbo.todoList.userId = dbo.users.id";



            var todoList = await DbHelper.ExecuteReaderAsync<ItemTodo>(_dbConnection as SqlConnection, queryString);

            return todoList;
        }
        public async Task<int> GetSumOfTaskByUserId(int userId)
        {
            string queryString = $@"SELECT COUNT(*)
                                    FROM dbo.todoList
                                    INNER JOIN dbo.users ON dbo.todoList.userId = dbo.users.id WHERE dbo.todoList.userId={userId}";



            int todoList = (int)await DbHelper.ExecuteScalarAsync(_dbConnection as SqlConnection, queryString);

            return todoList;
        }

        public async Task<IEnumerable<ItemTodo>> AddTask(ItemTodo request)
        {
            string queryString = $@"IF NOT EXISTS (
		                                            SELECT 1
		                                            FROM todoList
		                                            WHERE trim(description) = @Description
			                                            AND [userId] = {request.UserId}
		                                            )
                                     BEGIN
	                                               
                                                    INSERT INTO dbo.todoList (		                                                
		                                                [description],
		                                                [userId]
		                                                )
	                                                VALUES (
		                                                @Description,
		                                                {request.UserId}		                                               
		                                                );

	                                                SELECT dbo.todoList.id
	                                                        ,dbo.todoList.description
	                                                        ,dbo.todoList.done
	                                                        ,dbo.users.fullName
                                                        FROM dbo.todoList
                                                        INNER JOIN dbo.users ON dbo.todoList.userId = dbo.users.id;
                                                    END";



            var todoList = await DbHelper.ExecuteReaderAsync<ItemTodo>(_dbConnection as SqlConnection, queryString, new SqlParameter[] {
                    new SqlParameter{ ParameterName = "@Description", SqlDbType = SqlDbType.NVarChar, Value = request.Description.Trim() }
                });

            if (todoList == null)
            {
                throw new ApplicationException($"Todo is already exist");
            }

            return todoList;
        }

    }
}
