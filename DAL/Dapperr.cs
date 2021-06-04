using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace figma.DAL
{
    public class Dapperr : IDapper
    {
        private readonly IConfiguration _config;
        private string Connectionstring = "ShopProductContext";

        public Dapperr(IConfiguration config)
        {
            _config = config;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            using var connection = new SqlConnection(_config.GetConnectionString(Connectionstring));
            return connection.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }
        public async Task<T> GetAync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            using var connection = new SqlConnection(_config.GetConnectionString(Connectionstring));
            return await connection.QueryFirstOrDefaultAsync<T>(sp, parms, commandType: commandType);
        }

        public IEnumerable<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using var connection = new SqlConnection(_config.GetConnectionString(Connectionstring));
            return connection.Query<T>(sp, parms, commandType: commandType).ToList();
        }
        public async Task<IEnumerable<T>> GetAllAync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            //  Console.WriteLine(sp);
            using var connection = new SqlConnection(_config.GetConnectionString(Connectionstring));
            return await connection.QueryAsync<T>(sp, parms, commandType: commandType);
        }

        public DbConnection GetDbconnection()
        {
            return new SqlConnection(_config.GetConnectionString(Connectionstring));
        }

        public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using var connection = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using var tran = connection.BeginTransaction();
                try
                {
                    result = connection.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return result;
        }

        public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using var connection = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using var tran = connection.BeginTransaction();
                try
                {
                    result = connection.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return result;
        }
    }
}