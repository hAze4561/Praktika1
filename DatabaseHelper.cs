using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public class DatabaseHelper
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["ProizvodnayConnection"].ConnectionString;

        /// <summary>
        /// Получить строку подключения к базе данных
        /// </summary>
        public static string ConnectionString
        {
            get { return connectionString; }
        }

        /// <summary>
        /// Проверить подключение к базе данных
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Выполнить SQL запрос и вернуть результат в виде DataTable
        /// </summary>
        public static DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Ошибка выполнения запроса
            }
            return dataTable;
        }

        /// <summary>
        /// Выполнить SQL команду (INSERT, UPDATE, DELETE)
        /// </summary>
        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Выбрасываем исключение для обработки в вызывающем коде
                throw new Exception($"Ошибка выполнения команды: {ex.Message}", ex);
            }
            return rowsAffected;
        }

        /// <summary>
        /// Выполнить SQL запрос и вернуть одно значение
        /// </summary>
        public static object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            object result = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        result = command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                // Выбрасываем исключение для обработки в вызывающем коде
                throw new Exception($"Ошибка выполнения запроса: {ex.Message}", ex);
            }
            return result;
        }
    }
}
