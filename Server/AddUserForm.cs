using Server.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class AddUserForm : Form
    {
        public static User user;
        public static int curUserId;

        public AddUserForm()
        {
            InitializeComponent();
            label5.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void AdminWindow(object sender, EventArgs e)
        {
            try
            {
                AdminMain register = new AdminMain();
                register.StartPosition = FormStartPosition.Manual;
                register.Location = this.Location;
                register.Show();
                this.Hide();
            }
            catch (Exception er)
            {
                Console.WriteLine(er);
            }

        }


        private async void button1_Click(object sender, EventArgs e)
        {
            String email = textBox1.Text;
            String pass1 = textBox2.Text;
            if (pass1 != null && email != null && pass1.Length>3 && email.Length>0)
            {

                User pass2 = await DBController.getUser(email);

                if (pass2.login != null)
                {
                    label5.Show();
                    label5.BackColor = Color.Red;
                    label5.Text = "Пользователь с таким логином уже существует";
                }
                else
                {
                    if(await Admin.setUser(email, pass1))
                    {
                        label5.Show();
                        label5.BackColor = Color.Green;
                        label5.Text = "Пользователь добавлен";
                        await DBController.writeLog("Адмнистратор добавил нового пользователя " + email, Login.curUserId);
                    }
                    else
                    {
                        label5.Show();
                        label5.BackColor = Color.Red;
                        label5.Text = "Ошибка";
                    }
                }
            }
            else
            {
                label5.Show();
                label5.BackColor = Color.Red;
                label5.Text = "Логин или пароль введены неверно";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AdminMain register = new AdminMain();
            register.StartPosition = FormStartPosition.Manual;
            register.Location = this.Location;
            register.Show();
            this.Close();
        }
    }
}
