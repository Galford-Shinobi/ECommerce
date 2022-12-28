using Dapper;
using ECommerce.App.Helpers.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Data;
using ECommerce.Common.Responses;
using NLog.Fluent;

namespace ECommerce.App.Helpers.Repositories
{
    public class DapperRepository : IDapperRepository
    {
        private readonly IConfiguration _config;
        private string Connectionstring = "DefaultConnection";
        public DapperRepository(IConfiguration config)
        {
            _config = config;
        }
        public void Dispose()
        { }

        public async Task<int> ExecuteAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            int affectedRows = 0;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                affectedRows = await db.ExecuteAsync(sp, parms, commandType: commandType);
            }
            catch (Exception)
            {

                affectedRows = -1;
            }
            finally {
                db.Close();
                db.Dispose();
            }
            return affectedRows;
        }

        public List<T> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            return db.Query<T>(sp, parms, commandType: commandType).ToList();
        }

        public T GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }

        public DbConnection GetDbconnection()
        {
            return new SqlConnection(_config.GetConnectionString(Connectionstring));
        }

        public async Task<GenericResponse<T>> GetFullAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var list = await db.QueryAsync<T>(sp, parms, commandType: commandType);
                return new GenericResponse<T>
                {
                    IsSuccess = true,
                    ListResults = list.ToList(),
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<T>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
        }

        public GenericResponse<T> GetOnlyAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                result = db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new GenericResponse<T>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return new GenericResponse<T>
            {
                IsSuccess = true,
                Result = result,
            };
        }

        public GenericResponse<T> GetOnlyAvatarAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                result = db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new GenericResponse<T>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return new GenericResponse<T>
            {
                IsSuccess = true,
                Result = result,
            };
        }

        public GenericResponse<T> InsertAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return new GenericResponse<T>
                    {
                        IsSuccess = false,
                        Message = ex.Message,
                    };
                }
            }
            catch (Exception ex)
            {
                return new GenericResponse<T>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return new GenericResponse<T>
            {
                IsSuccess = true,
                Result = result,
            };
        }

        public GenericResponse<T> UpdateAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return new GenericResponse<T>
                    {
                        IsSuccess = false,
                        Message = ex.Message,
                    };
                }
            }
            catch (Exception ex)
            {
                return new GenericResponse<T>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return new GenericResponse<T>
            {
                IsSuccess = true,
                Result = result,
            };
        }
        //public async Task NuevoEmpleado(Empleado e)
        //{
        //    SqlConnection sqlConexion = conexion();
        //    try
        //    {
        //        sqlConexion.Open();
        //        var param = new DynamicParameters();
        //        param.Add("@Nombre", e.Nombre, DbType.String, ParameterDirection.Input, 500);
        //        param.Add("@CodEmpleado", e.CodEmpleado, DbType.String, ParameterDirection.Input, 4);
        //        param.Add("@Email", e.Email, DbType.String, ParameterDirection.Input, 255);
        //        param.Add("@Edad", e.Edad, DbType.Int32, ParameterDirection.Input);
        //        param.Add("@FechaAlta", e.FechaAlta, DbType.DateTime, ParameterDirection.Input);
        //        await sqlConexion.ExecuteScalarAsync("EmpleadoAlta", param, commandType: CommandType.StoredProcedure);
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.LogError("ERROR: " + ex.ToString());
        //        throw new Exception("Se produjo un error al dar de alta" + ex.Message);
        //    }
        //    finally
        //    {
        //        sqlConexion.Close();
        //        sqlConexion.Dispose();
        //    }

        //}
    }
}
