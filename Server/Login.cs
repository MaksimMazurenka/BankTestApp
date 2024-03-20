
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
    public partial class Login : Form
    {
        public static User user;
        public static int curUserId;
        public Login()
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
            catch(Exception er)
            {
                Console.WriteLine(er);
            }
            
        }

        private void ExchangeWindow(object sender, EventArgs e)
        {
            Exchange register = new Exchange();
            register.StartPosition = FormStartPosition.Manual;
            register.Location = this.Location;
            register.Show();
            this.Hide();
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            String email = textBox1.Text;
            String pass1 = textBox2.Text;
            if (pass1 != null && email != null && pass1.Length>0 && email.Length>0)
            {

                User pass2 = await DBController.getUserAndRole(email, pass1);

                if (pass2.login != null)
                {
                    if (pass2 is Admin)
                    {
                        await DBController.writeLog("Администратор авторизировался", Login.curUserId);
                        AdminWindow(sender, e);
                    }
                    else
                    {
                        await DBController.writeLog("Кассир авторизировался", Login.curUserId);
                        ExchangeWindow(sender,e);
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

    }
}
