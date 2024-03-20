using Server.Entity;
using System;
using System.Collections;
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
    public partial class AdminTransactionsScreen : Form
    {
        List<ExchangeHistory> exchangeHistories = new List<ExchangeHistory>();
        List<User> users = new List<User>();
        List<Currency> currencies = new List<Currency>();
        Dictionary<int, string> usersNames = new Dictionary<int, string>();
        private bool user;
        static bool radio = true;
        private bool cur1;
        private bool cur2;
        private int trigger = 0;
        public AdminTransactionsScreen()
        {
            InitializeComponent();

            comboBox4.Visible = false;
            comboBox4.Items.Add(0);
            comboBox4.Items.Add(1);

            label4.Visible = false;
            label5.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;

            radioButton1.Checked = true;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            getUsers();
            Add_Row_To_DataGridView_Using_TextBoxes_Load();
            getCur();
            setUserList();
        }

        private void triggerAkt()
        {
            if (trigger == 0)
            {
                trigger = 1;
                comboBox4.SelectedIndex = 1;
            }
            else
            {
                trigger = 0;
                comboBox4.SelectedIndex = 0;
            }
        }

        private void radioButtonPresed(object sender, EventArgs e)
        {
            radio = !radio;
            radioButton1.Checked = radio;
            if (radioButton1.Checked)
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
            }
            else
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
            }
            triggerAkt();
        }

        private void setUserList()
        {
            comboBox1.Items.Clear();

            comboBox1.Items.Add("Все");
            for (int i = 0; i < usersNames.Count; i++)
            {
                comboBox1.Items.Add(usersNames[users[i].userId]);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void Add_Row_To_DataGridView_Using_TextBoxes_Load()
        {
            dataGridView1.AllowUserToAddRows = false;


            dataGridView1.ColumnCount = 9;

            for (int i = 0; i < 9; i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;

            dataGridView1.Columns[0].Name = "Исходная валюта";
            dataGridView1.Columns[1].Name = "Количество";
            dataGridView1.Columns[2].Name = "Целевая валюта";
            dataGridView1.Columns[3].Name = "Количество";
            dataGridView1.Columns[4].Name = "Курс";
            dataGridView1.Columns[5].Name = "Коэфициент";
            dataGridView1.Columns[6].Name = "Сдача";
            dataGridView1.Columns[7].Name = "Дата и время";
            dataGridView1.Columns[8].Name = "Кассир";
            getExchanges();

        }

        private async Task getExchanges()
        {
            exchangeHistories = await ExchangeHistory.getAllExchangeHistory();
            dataGridView1.Rows.Clear();
            for (int i = 0; i < exchangeHistories.Count; i++)
            {
                ArrayList al = new ArrayList();
                al.Add(exchangeHistories[i].CurrencyFrom);
                al.Add(exchangeHistories[i].AmountFrom);
                al.Add(exchangeHistories[i].CurrencyTo);
                al.Add(exchangeHistories[i].AmountTo);
                al.Add(exchangeHistories[i].ExchangeRate);
                al.Add(exchangeHistories[i].ExchangeScale);
                al.Add(exchangeHistories[i].Change);
                al.Add(exchangeHistories[i].date.ToString("dd.MM.yyyy hh:mm"));
                al.Add(usersNames[exchangeHistories[i].userId]);
                dataGridView1.Rows.Add(al.ToArray());
            }
        }

        private async Task getCur()
        {
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            currencies = await Currency.getCurrencies();
            comboBox2.Items.Add("Все");
            comboBox3.Items.Add("Все");
            for (int i = 0; i < currencies.Count; i++)
            {
                comboBox2.Items.Add(currencies[i].currencyCode);
                comboBox3.Items.Add(currencies[i].currencyCode);
            }
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
        }

        private async Task getExchangesByUser()
        {
            dataGridView1.Rows.Clear();
            if(exchangeHistories != null)
            {
                for (int i = 0; i < exchangeHistories.Count; i++)
                {
                    ArrayList al = new ArrayList();
                    al.Add(exchangeHistories[i].CurrencyFrom);
                    al.Add(exchangeHistories[i].AmountFrom);
                    al.Add(exchangeHistories[i].CurrencyTo);
                    al.Add(exchangeHistories[i].AmountTo);
                    al.Add(exchangeHistories[i].ExchangeRate);
                    al.Add(exchangeHistories[i].ExchangeScale);
                    al.Add(exchangeHistories[i].Change);
                    al.Add(exchangeHistories[i].date.ToString("dd.MM.yyyy hh:mm"));
                    al.Add(usersNames[exchangeHistories[i].userId]);
                    dataGridView1.Rows.Add(al.ToArray());
                }
            }
        }

        private async Task getUsers()
        {
            users = await Admin.getAllUsers();
            for (int i = 0; i < users.Count; i++)
            {
                usersNames.Add(users[i].userId, users[i].login);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminMain register = new AdminMain();
            register.StartPosition = FormStartPosition.Manual;
            register.Location = this.Location;
            register.Show();
            this.Close();
        }

        private async void selectedUserChangedAsync(object sender, EventArgs e)
        {
            label4.Visible = false;
            label5.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            if (comboBox2.SelectedIndex!=-1 && comboBox1.SelectedIndex!=-1 && comboBox3.SelectedIndex != -1)
            {
                if (comboBox2.SelectedIndex == 0)
                {
                    cur1 = false;
                }
                else
                {
                    cur1 = true;
                }
                if (comboBox3.SelectedIndex == 0)
                {
                    cur2 = false;
                }
                else
                {
                    cur2 = true;
                }
                if (comboBox1.SelectedIndex == 0)
                {
                    user = false;
                }
                else
                {
                    user = true;
                }
                switch (user)
                {
                    case true:
                        switch (!radioButton1.Checked)
                        {
                            case true:
                                switch (cur1)
                                {
                                    case true:
                                        switch (cur2)
                                        {
                                            case true:
                                                exchangeHistories = await ExchangeHistory.getExchangeByDateUserAndCur1Cur2(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date, users[comboBox1.SelectedIndex - 1].userId, comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString());
                                                break;
                                            case false:
                                                exchangeHistories = await ExchangeHistory.getExchangeByDateUserAndCurrency1(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date, users[comboBox1.SelectedIndex - 1].userId, comboBox2.SelectedItem.ToString());
                                                break;
                                        }
                                        break;
                                    case false:
                                        switch (cur2)
                                        {
                                            case true:
                                                exchangeHistories = await ExchangeHistory.getExchangeByDateUserAndCurrency2(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date, users[comboBox1.SelectedIndex - 1].userId, comboBox3.SelectedItem.ToString());
                                                break;
                                            case false:
                                                exchangeHistories = await ExchangeHistory.getExchangeByDateAndUser(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date, users[comboBox1.SelectedIndex - 1].userId);
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case false:
                                switch (cur1)
                                {
                                    case true:
                                        switch (cur2)
                                        {
                                            case true:
                                                exchangeHistories = await ExchangeHistory.getExchangeByUserAndCur1Cur2(users[comboBox1.SelectedIndex - 1].userId, comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString());
                                                break;
                                            case false:
                                                exchangeHistories = await ExchangeHistory.getExchangeByUserAndCur1(users[comboBox1.SelectedIndex - 1].userId, comboBox2.SelectedItem.ToString());
                                                break;
                                        }
                                        break;
                                    case false:
                                        switch (cur2)
                                        {
                                            case true:
                                                exchangeHistories = await ExchangeHistory.getExchangeByUserAndCur2(users[comboBox1.SelectedIndex - 1].userId, comboBox3.SelectedItem.ToString());
                                                break;
                                            case false:
                                                exchangeHistories = await ExchangeHistory.getExchangeByUser(users[comboBox1.SelectedIndex - 1].userId);
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                    case false:
                        switch (!radioButton1.Checked)
                        {
                            case true:
                                switch (cur1)
                                {
                                    case true:
                                        switch (cur2)
                                        {
                                            case true:
                                                exchangeHistories = await ExchangeHistory.getExchangeByDateCur1Cur2(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date, comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString());
                                                break;
                                            case false:
                                                exchangeHistories = await ExchangeHistory.getExchangeByDateCur1(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date, comboBox2.SelectedItem.ToString());
                                                break;
                                        }
                                        break;
                                    case false:
                                        switch (cur2)
                                        {
                                            case true:
                                                exchangeHistories = await ExchangeHistory.getExchangeByDateCur2(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date, comboBox3.SelectedItem.ToString());
                                                break;
                                            case false:
                                                exchangeHistories = await ExchangeHistory.getExchangeByDate(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date);
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case false:
                                switch (cur1)
                                {
                                    case true:
                                        switch (cur2)
                                        {
                                            case true:
                                                exchangeHistories = await ExchangeHistory.getExchangeByCur1Cur2(comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString());
                                                break;
                                            case false:
                                                exchangeHistories = await ExchangeHistory.getExchangeByCur1(comboBox2.SelectedItem.ToString());
                                                break;
                                        }
                                        break;
                                    case false:
                                        switch (cur2)
                                        {
                                            case true:
                                                exchangeHistories = await ExchangeHistory.getExchangeByCur2(comboBox3.SelectedItem.ToString());
                                                break;
                                            case false:
                                                exchangeHistories = await ExchangeHistory.getAllExchangeHistory();
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                }
                getExchangesByUser();
                if (cur1)
                {
                    label4.Visible = true;
                    label4.Text = comboBox2.SelectedItem.ToString() + ": +";
                    decimal rez = 0;
                    for (int i=0;i<exchangeHistories.Count;i++)
                    {
                        rez = rez + exchangeHistories[i].AmountFrom - exchangeHistories[i].Change;
                    }
                    textBox1.Visible = true;
                    textBox1.Text = rez.ToString();
                    
                }
                if (cur2)
                {
                    label5.Visible = true;
                    label5.Text = comboBox3.SelectedItem.ToString() + ": -";
                    decimal rez = 0;
                    for (int i = 0; i < exchangeHistories.Count; i++)
                    {
                        rez = rez + exchangeHistories[i].AmountTo;
                    }
                    textBox2.Visible = true;
                    textBox2.Text = rez.ToString();
                    
                }
            }
        }
    }
}
