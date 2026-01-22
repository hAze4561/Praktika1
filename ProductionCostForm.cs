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
    public partial class ProductionCostForm : Form
    {
        private int? recordId;
        private DataTable tableStructure;

        public ProductionCostForm()
        {
            InitializeComponent();
            recordId = null;
            LoadTableStructure();
            CreateDynamicControls();
        }

        public ProductionCostForm(int id)
        {
            InitializeComponent();
            recordId = id;
            LoadTableStructure();
            CreateDynamicControls();
            LoadRecordData();
        }

        private void LoadTableStructure()
        {
            try
            {
                // Получаем структуру таблицы
                string query = @"SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE 
                                FROM INFORMATION_SCHEMA.COLUMNS 
                                WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'ProductionCosts'
                                ORDER BY ORDINAL_POSITION";
                tableStructure = DatabaseHelper.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке структуры таблицы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateDynamicControls()
        {
            if (tableStructure == null || tableStructure.Rows.Count == 0)
                return;

            int yPos = 20;
            int labelWidth = 150;
            int textBoxWidth = 300;
            int spacing = 35;

            foreach (DataRow row in tableStructure.Rows)
            {
                string columnName = row["COLUMN_NAME"].ToString();
                string dataType = row["DATA_TYPE"].ToString();
                string isNullable = row["IS_NULLABLE"].ToString();

                // Пропускаем ID и автоматически заполняемые поля
                if (columnName.ToLower().Contains("id") && dataType.ToLower().Contains("int") && recordId == null)
                    continue;

                // Создаем Label
                Label label = new Label
                {
                    Text = columnName + ":",
                    Location = new Point(20, yPos),
                    Size = new Size(labelWidth, 20),
                    AutoSize = false
                };
                this.Controls.Add(label);

                // Создаем TextBox или другой контрол в зависимости от типа данных
                Control inputControl = null;
                if (dataType.ToLower().Contains("date"))
                {
                    DateTimePicker dtp = new DateTimePicker
                    {
                        Location = new Point(180, yPos),
                        Size = new Size(textBoxWidth, 25),
                        Format = DateTimePickerFormat.Short,
                        Name = "ctrl_" + columnName
                    };
                    inputControl = dtp;
                }
                else if (dataType.ToLower().Contains("bit") || dataType.ToLower().Contains("bool"))
                {
                    CheckBox cb = new CheckBox
                    {
                        Location = new Point(180, yPos),
                        Size = new Size(textBoxWidth, 20),
                        Name = "ctrl_" + columnName
                    };
                    inputControl = cb;
                }
                else
                {
                    TextBox tb = new TextBox
                    {
                        Location = new Point(180, yPos),
                        Size = new Size(textBoxWidth, 25),
                        Name = "ctrl_" + columnName
                    };
                    inputControl = tb;
                }

                this.Controls.Add(inputControl);
                yPos += spacing;
            }

            // Кнопки
            Button btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(180, yPos + 20),
                Size = new Size(100, 35),
                DialogResult = DialogResult.OK
            };
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            Button btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(290, yPos + 20),
                Size = new Size(100, 35),
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnCancel);

            this.Height = yPos + 120;
            this.Width = 520;
        }

        private void LoadRecordData()
        {
            if (recordId == null)
                return;

            try
            {
                // Находим имя столбца с ID
                string idColumnName = tableStructure.Rows[0]["COLUMN_NAME"].ToString();
                string query = $"SELECT * FROM [dbo].[ProductionCosts] WHERE [{idColumnName}] = @ID";
                
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ID", SqlDbType.Int) { Value = recordId.Value }
                };

                DataTable data = DatabaseHelper.ExecuteQuery(query, parameters);
                
                if (data != null && data.Rows.Count > 0)
                {
                    DataRow record = data.Rows[0];
                    
                    foreach (DataColumn column in data.Columns)
                    {
                        Control ctrl = this.Controls.Find("ctrl_" + column.ColumnName, false).FirstOrDefault();
                        if (ctrl != null)
                        {
                            object value = record[column.ColumnName];
                            if (value != DBNull.Value)
                            {
                                if (ctrl is TextBox)
                                {
                                    ((TextBox)ctrl).Text = value.ToString();
                                }
                                else if (ctrl is DateTimePicker)
                                {
                                    ((DateTimePicker)ctrl).Value = Convert.ToDateTime(value);
                                }
                                else if (ctrl is CheckBox)
                                {
                                    ((CheckBox)ctrl).Checked = Convert.ToBoolean(value);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (recordId == null)
                {
                    // Добавление новой записи
                    InsertRecord();
                }
                else
                {
                    // Обновление существующей записи
                    UpdateRecord();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }

        private void InsertRecord()
        {
            List<string> columns = new List<string>();
            List<string> values = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (DataRow row in tableStructure.Rows)
            {
                string columnName = row["COLUMN_NAME"].ToString();
                string dataType = row["DATA_TYPE"].ToString();
                string isNullable = row["IS_NULLABLE"].ToString();

                // Пропускаем ID и автоматически заполняемые поля
                if (columnName.ToLower().Contains("id") && dataType.ToLower().Contains("int"))
                    continue;

                Control ctrl = this.Controls.Find("ctrl_" + columnName, false).FirstOrDefault();
                if (ctrl != null)
                {
                    columns.Add($"[{columnName}]");
                    string paramName = "@" + columnName;
                    values.Add(paramName);

                    object value = GetControlValue(ctrl, dataType);
                    if (value != null)
                    {
                        parameters.Add(new SqlParameter(paramName, GetSqlDbType(dataType)) { Value = value });
                    }
                    else if (isNullable == "YES")
                    {
                        parameters.Add(new SqlParameter(paramName, GetSqlDbType(dataType)) { Value = DBNull.Value });
                    }
                }
            }

            string query = $"INSERT INTO [dbo].[ProductionCosts] ({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)})";
            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters.ToArray());

            if (rowsAffected > 0)
            {
                MessageBox.Show("Запись успешно добавлена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateRecord()
        {
            string idColumnName = tableStructure.Rows[0]["COLUMN_NAME"].ToString();
            List<string> setClauses = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (DataRow row in tableStructure.Rows)
            {
                string columnName = row["COLUMN_NAME"].ToString();
                string dataType = row["DATA_TYPE"].ToString();

                // Пропускаем ID
                if (columnName == idColumnName)
                    continue;

                Control ctrl = this.Controls.Find("ctrl_" + columnName, false).FirstOrDefault();
                if (ctrl != null)
                {
                    string paramName = "@" + columnName;
                    setClauses.Add($"[{columnName}] = {paramName}");

                    object value = GetControlValue(ctrl, dataType);
                    if (value != null)
                    {
                        parameters.Add(new SqlParameter(paramName, GetSqlDbType(dataType)) { Value = value });
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(paramName, GetSqlDbType(dataType)) { Value = DBNull.Value });
                    }
                }
            }

            parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = recordId.Value });
            string query = $"UPDATE [dbo].[ProductionCosts] SET {string.Join(", ", setClauses)} WHERE [{idColumnName}] = @ID";
            
            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters.ToArray());

            if (rowsAffected > 0)
            {
                MessageBox.Show("Запись успешно обновлена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private object GetControlValue(Control ctrl, string dataType)
        {
            if (ctrl is TextBox)
            {
                string text = ((TextBox)ctrl).Text.Trim();
                return string.IsNullOrEmpty(text) ? null : text;
            }
            else if (ctrl is DateTimePicker)
            {
                return ((DateTimePicker)ctrl).Value;
            }
            else if (ctrl is CheckBox)
            {
                return ((CheckBox)ctrl).Checked;
            }
            return null;
        }

        private SqlDbType GetSqlDbType(string dataType)
        {
            switch (dataType.ToLower())
            {
                case "int":
                case "integer":
                    return SqlDbType.Int;
                case "bigint":
                    return SqlDbType.BigInt;
                case "decimal":
                case "money":
                    return SqlDbType.Decimal;
                case "float":
                    return SqlDbType.Float;
                case "datetime":
                case "date":
                    return SqlDbType.DateTime;
                case "bit":
                    return SqlDbType.Bit;
                case "nvarchar":
                case "varchar":
                case "text":
                    return SqlDbType.NVarChar;
                default:
                    return SqlDbType.NVarChar;
            }
        }
    }
}
