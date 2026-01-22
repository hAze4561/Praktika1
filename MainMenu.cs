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
    public partial class MainMenu : Form
    {
        private string currentUserLogin;
        private string currentUserRole;
        private DataTable productionCostsTable;

        public MainMenu(string login)
        {
            currentUserLogin = login;
            InitializeComponent();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            // Получаем роль пользователя
            GetUserRole();
            
            // Если пользователь администратор - загружаем данные
            if (currentUserRole == "Администратор")
            {
                LoadProductionCosts();
            }
            else
            {
                // Если не администратор - скрываем tabPage1
                if (tabControl1 != null && tabControl1.TabPages.Contains(tabPage1))
                {
                    tabControl1.TabPages.Remove(tabPage1);
                }
            }
        }

        private void GetUserRole()
        {
            try
            {
                string query = "SELECT [Role] FROM [dbo].[Users] WHERE [Login] = @Login";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Login", SqlDbType.NVarChar) { Value = currentUserLogin }
                };

                object result = DatabaseHelper.ExecuteScalar(query, parameters);
                if (result != null)
                {
                    currentUserRole = result.ToString();
                }
                else
                {
                    currentUserRole = "Пользователь";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении роли пользователя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                currentUserRole = "Пользователь";
            }
        }

        private void LoadProductionCosts()
        {
            try
            {
                string query = "SELECT * FROM [dbo].[ProductionCosts]";
                productionCostsTable = DatabaseHelper.ExecuteQuery(query);
                
                if (dataGridViewProductionCosts != null)
                {
                    dataGridViewProductionCosts.DataSource = productionCostsTable;
                    dataGridViewProductionCosts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewProductionCosts.AllowUserToAddRows = false;
                    dataGridViewProductionCosts.ReadOnly = true;
                    dataGridViewProductionCosts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Открываем форму для добавления новой записи
            ProductionCostForm form = new ProductionCostForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProductionCosts(); // Обновляем данные
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewProductionCosts.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewProductionCosts.SelectedRows[0];
                
                // Находим столбец с ID
                string idColumnName = null;
                object idValue = null;
                
                foreach (DataGridViewColumn col in dataGridViewProductionCosts.Columns)
                {
                    if (col.Name.ToLower().Contains("id"))
                    {
                        idColumnName = col.Name;
                        idValue = selectedRow.Cells[col.Index].Value;
                        break;
                    }
                }
                
                // Если не нашли, берем первый столбец
                if (idColumnName == null)
                {
                    idValue = selectedRow.Cells[0].Value;
                }
                
                if (idValue == null || idValue == DBNull.Value)
                {
                    MessageBox.Show("Не удалось определить ID записи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // Открываем форму для редактирования
                ProductionCostForm form = new ProductionCostForm(Convert.ToInt32(idValue));
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadProductionCosts(); // Обновляем данные
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewProductionCosts.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewProductionCosts.SelectedRows[0];
                
                // Находим столбец с ID (обычно первый столбец или содержит "ID" в названии)
                string idColumnName = null;
                object idValue = null;
                
                foreach (DataGridViewColumn col in dataGridViewProductionCosts.Columns)
                {
                    if (col.Name.ToLower().Contains("id"))
                    {
                        idColumnName = col.Name;
                        idValue = selectedRow.Cells[col.Index].Value;
                        break;
                    }
                }
                
                // Если не нашли, берем первый столбец
                if (idColumnName == null)
                {
                    idColumnName = dataGridViewProductionCosts.Columns[0].Name;
                    idValue = selectedRow.Cells[0].Value;
                }
                
                if (idValue == null || idValue == DBNull.Value)
                {
                    MessageBox.Show("Не удалось определить ID записи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string query = $"DELETE FROM [dbo].[ProductionCosts] WHERE [{idColumnName}] = @ID";
                        
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("@ID", SqlDbType.Int) { Value = Convert.ToInt32(idValue) }
                        };

                        int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
                        
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Запись успешно удалена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadProductionCosts(); // Обновляем данные
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
