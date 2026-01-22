using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class avtoriz : Form
    {
        public avtoriz()
        {
            InitializeComponent();
        }

        private void avtoriz_Load(object sender, EventArgs e)
        {
            // Инициализация: пароль по умолчанию скрыт
            txtPassword.UseSystemPasswordChar = true;
            pictureBoxGlaz.Visible = true;   // иконка "скрыто"
            pictureBoxGlaz1.Visible = false; // иконка "показать"
        }

        private void pictureBoxGlaz_Click(object sender, EventArgs e)
        {
            // Показать пароль
            txtPassword.UseSystemPasswordChar = false;
            pictureBoxGlaz.Visible = false;
            pictureBoxGlaz1.Visible = true;
        }

        private void pictureBoxGlaz1_Click(object sender, EventArgs e)
        {
            // Скрыть пароль
            txtPassword.UseSystemPasswordChar = true;
            pictureBoxGlaz.Visible = true;
            pictureBoxGlaz1.Visible = false;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Переход на форму регистрации
            register registerForm = new register();
            registerForm.Show();
            this.Hide();
        }

        private void btnVxod_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            // Проверка на пустые поля
            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Введите логин!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            try
            {
                // ВАЖНО: в вашей таблице Proizvodnay.dbo.Users поле PasswordHash хранит пароль как обычный текст
                // Поэтому сравниваем введённый пароль напрямую с полем PasswordHash (без хеширования)

                string query = "SELECT COUNT(*) FROM [dbo].[Users] WHERE [Login] = @Login AND [PasswordHash] = @Password";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Login", SqlDbType.NVarChar)   { Value = login },
                    new SqlParameter("@Password", SqlDbType.NVarChar){ Value = password }
                };

                object result = DatabaseHelper.ExecuteScalar(query, parameters);

                if (result != null && Convert.ToInt32(result) > 0)
                {
                    // Логин и пароль верны - открываем форму капчи
                    CaptchaForm captchaForm = new CaptchaForm();
                    if (captchaForm.ShowDialog() == DialogResult.OK && captchaForm.IsCaptchaSolved)
                    {
                        // Капча пройдена - переход на главное меню
                        MainMenu mainMenu = new MainMenu(login);
                        mainMenu.Show();
                        this.Hide();
                    }
                    captchaForm.Dispose();
                }
                else
                {
                    // Неверный логин или пароль
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при авторизации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Хеширование пароля с использованием SHA256
        /// </summary>
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Преобразуем пароль в байты
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Преобразуем байты в строку
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
