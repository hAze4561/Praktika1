using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class register : Form
    {
        public register()
        {
            InitializeComponent();
        }

        private void btnNazad_Click(object sender, EventArgs e)
        {
            // Возврат на форму авторизации
            avtoriz authForm = new avtoriz();
            authForm.Show();
            this.Hide();
        }

        private void btnNazad_Click_1(object sender, EventArgs e)
        {
            // Возврат на форму авторизации
            avtoriz authForm = new avtoriz();
            authForm.Show();
            this.Hide();
        }

        private void btnRegister1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Проверка на пустые поля
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Введите логин!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Подтвердите пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return;
            }

            // Проверка совпадения паролей
            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtConfirmPassword.Clear();
                txtConfirmPassword.Focus();
                return;
            }

            try
            {
                // Проверка, существует ли пользователь с таким логином
                string checkQuery = "SELECT COUNT(*) FROM [dbo].[Users] WHERE [Login] = @Login";
                SqlParameter[] checkParams = new SqlParameter[]
                {
                    new SqlParameter("@Login", SqlDbType.NVarChar) { Value = username }
                };

                object userExists = DatabaseHelper.ExecuteScalar(checkQuery, checkParams);

                if (userExists != null && Convert.ToInt32(userExists) > 0)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.Clear();
                    txtUsername.Focus();
                    return;
                }

                // Генерация случайных данных для FullName и Email
                Random random = new Random();
                string[] firstNames = { "Иванов", "Петров", "Сидоров", "Козлов", "Смирнов", "Попов", "Лебедев", "Новиков", "Морозов", "Петрова" };
                string[] lastNames = { "А.С.", "И.О.", "В.П.", "Д.М.", "С.А.", "М.В.", "А.А.", "И.И.", "П.П.", "С.С." };
                
                string randomFullName = $"{firstNames[random.Next(firstNames.Length)]} {lastNames[random.Next(lastNames.Length)]}";
                string randomEmail = $"{username.ToLower()}{random.Next(1000, 9999)}@makeevka.met";

                // Вставка нового пользователя в БД
                // Логин и пароль берутся из формы регистрации
                // FullName и Email генерируются случайно
                string insertQuery = @"INSERT INTO [dbo].[Users] 
                                      ([FullName], [Email], [Login], [PasswordHash], [Role], [IsActive], [CreatedAt]) 
                                      VALUES (@FullName, @Email, @Login, @PasswordHash, @Role, 1, GETDATE())";

                SqlParameter[] insertParams = new SqlParameter[]
                {
                    new SqlParameter("@FullName", SqlDbType.NVarChar) { Value = randomFullName },
                    new SqlParameter("@Email", SqlDbType.NVarChar) { Value = randomEmail },
                    new SqlParameter("@Login", SqlDbType.NVarChar) { Value = username },
                    new SqlParameter("@PasswordHash", SqlDbType.NVarChar) { Value = password },
                    new SqlParameter("@Role", SqlDbType.NVarChar) { Value = "Пользователь" }
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(insertQuery, insertParams);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Регистрация успешна! Теперь вы можете войти.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Возврат на форму авторизации
                    avtoriz authForm = new avtoriz();
                    authForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Ошибка при регистрации. Попробуйте еще раз.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Показываем детальную информацию об ошибке для отладки
                string errorMessage = $"Ошибка при регистрации: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nДетали: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
