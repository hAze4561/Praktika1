namespace WindowsFormsApp1
{
    partial class avtoriz
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnVxod = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnVixod = new System.Windows.Forms.Button();
            this.pictureBoxGlaz = new System.Windows.Forms.PictureBox();
            this.pictureBoxGlaz1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGlaz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGlaz1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(226, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(318, 46);
            this.label1.TabIndex = 1;
            this.label1.Text = "Форма авторизации";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(164, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Введите логин:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(164, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(153, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Введите пароль:";
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(323, 139);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(211, 22);
            this.txtLogin.TabIndex = 6;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(323, 184);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(211, 22);
            this.txtPassword.TabIndex = 7;
            // 
            // btnVxod
            // 
            this.btnVxod.Location = new System.Drawing.Point(233, 236);
            this.btnVxod.Name = "btnVxod";
            this.btnVxod.Size = new System.Drawing.Size(119, 41);
            this.btnVxod.TabIndex = 8;
            this.btnVxod.Text = "Вход";
            this.btnVxod.UseVisualStyleBackColor = true;
            this.btnVxod.Click += new System.EventHandler(this.btnVxod_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(400, 236);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(119, 41);
            this.btnRegister.TabIndex = 9;
            this.btnRegister.Text = "Регистрация";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnVixod
            // 
            this.btnVixod.Location = new System.Drawing.Point(669, 398);
            this.btnVixod.Name = "btnVixod";
            this.btnVixod.Size = new System.Drawing.Size(119, 41);
            this.btnVixod.TabIndex = 10;
            this.btnVixod.Text = "Выход";
            this.btnVixod.UseVisualStyleBackColor = true;
            // 
            // pictureBoxGlaz
            // 
            this.pictureBoxGlaz.Image = global::WindowsFormsApp1.Properties.Resources.hidden;
            this.pictureBoxGlaz.Location = new System.Drawing.Point(540, 175);
            this.pictureBoxGlaz.Name = "pictureBoxGlaz";
            this.pictureBoxGlaz.Size = new System.Drawing.Size(45, 43);
            this.pictureBoxGlaz.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxGlaz.TabIndex = 11;
            this.pictureBoxGlaz.TabStop = false;
            this.pictureBoxGlaz.Click += new System.EventHandler(this.pictureBoxGlaz_Click);
            // 
            // pictureBoxGlaz1
            // 
            this.pictureBoxGlaz1.Image = global::WindowsFormsApp1.Properties.Resources.eye;
            this.pictureBoxGlaz1.Location = new System.Drawing.Point(540, 175);
            this.pictureBoxGlaz1.Name = "pictureBoxGlaz1";
            this.pictureBoxGlaz1.Size = new System.Drawing.Size(45, 43);
            this.pictureBoxGlaz1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxGlaz1.TabIndex = 12;
            this.pictureBoxGlaz1.TabStop = false;
            this.pictureBoxGlaz1.Click += new System.EventHandler(this.pictureBoxGlaz1_Click);
            // 
            // avtoriz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 451);
            this.Controls.Add(this.pictureBoxGlaz1);
            this.Controls.Add(this.pictureBoxGlaz);
            this.Controls.Add(this.btnVixod);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnVxod);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "avtoriz";
            this.Text = "Авторизация";
            this.Load += new System.EventHandler(this.avtoriz_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGlaz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGlaz1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnVxod;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnVixod;
        private System.Windows.Forms.PictureBox pictureBoxGlaz;
        private System.Windows.Forms.PictureBox pictureBoxGlaz1;
    }
}

