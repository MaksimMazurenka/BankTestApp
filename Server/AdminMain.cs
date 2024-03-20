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
    public partial class AdminMain : Form
    {
        public AdminMain()
        {
            InitializeComponent();
            Add_Row_To_DataGridView_Using_TextBoxes_Load();
            Add_Row_To_DataGridView_Using_TextBoxes_Load_2();
            getRates();
            getCurrencies();
        }

        List<Rate> rates = new List<Rate>();
        List<Currency> currencies = new List<Currency>();

        private void Add_Row_To_DataGridView_Using_TextBoxes_Load()
        {
            dataGridView1.AllowUserToAddRows = false;
            
            dataGridView1.ColumnCount = 4;

            dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;

            dataGridView1.Columns[0].Name = "Исходная валюта";
            dataGridView1.Columns[1].Name = "Целевая валюта";
            dataGridView1.Columns[2].Name = "Курс обмена";
            dataGridView1.Columns[3].Name = "Коэффициент";
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;

            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.HeaderText = "Изменить";
            btn.Name = "Btn";
            btn.Text = "Задать курс";
            btn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btn);
        }

        private void Add_Row_To_DataGridView_Using_TextBoxes_Load_2()
        {
            dataGridView2.AllowUserToAddRows = false;

            dataGridView2.ColumnCount = 2;

            dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView2.BackgroundColor = System.Drawing.SystemColors.Control;

            dataGridView2.Columns[0].Name = "Валюта";
            dataGridView2.Columns[1].Name = "Колво.";
            dataGridView2.Columns[0].ReadOnly = true;

            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.HeaderText = "Изменить";
            btn.Name = "Btn";
            btn.Text = "Изменить";
            btn.UseColumnTextForButtonValue = true;
            dataGridView2.Columns.Add(btn);
        }

        private async Task getRates()
        {
            rates = await Rate.getAllRates();
            for(int i = 0; i < rates.Count; i++)
            {
                ArrayList al = new ArrayList();
                al.Add(rates[i].CurrencyFrom);
                al.Add(rates[i].CurrencyTo);
                al.Add(rates[i].ExchangeRate);
                al.Add(rates[i].Scale);
                dataGridView1.Rows.Add(al.ToArray());
            }
        }

        private async Task getCurrencies()
        {
            currencies = await Currency.getCurrencies();
            for (int i = 0; i < currencies.Count; i++)
            {
                ArrayList al = new ArrayList();
                al.Add(currencies[i].currencyCode);
                al.Add(currencies[i].count);
                dataGridView2.Rows.Add(al.ToArray());
            }
        }


        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 4)
            {
                try
                {
                    int i = e.RowIndex;

                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    Decimal rate = Convert.ToDecimal(row.Cells[2].Value.ToString());
                    int scale = Int32.Parse(row.Cells[3].Value.ToString());

                    if (rates[i].ExchangeRate != rate || rates[i].Scale != scale && rate>0 && scale>=1)
                    {
                        await Admin.updateRateAsync(rates[i].CurrencyFrom, rates[i].CurrencyTo, rate, scale);
                        await DBController.writeLog("Адмнистратор обновил курс для " + rates[i].CurrencyFrom + "/" + rates[i].CurrencyTo, Login.curUserId);
                    }
                }catch(Exception exc)
                {

                }

            }
        }

        private async void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                try
                {
                    int i = e.RowIndex;

                    DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                    int rate = Convert.ToInt32(row.Cells[1].Value.ToString());

                    if (currencies[i].count != rate && rate>=0)
                    {
                        await Admin.updateCurrencyAsync(currencies[i].currencyCode, rate);
                        await DBController.writeLog("Адмнистратор изменил количество валюты " + currencies[i].currencyCode + " на " + rate, Login.curUserId);
                    }
                }
                catch (Exception exc)
                {

                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddRate register = new AddRate();
            register.StartPosition = FormStartPosition.Manual;
            register.Location = this.Location;
            register.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AdminTransactionsScreen register = new AdminTransactionsScreen();
            register.StartPosition = FormStartPosition.Manual;
            register.Location = this.Location;
            register.Show();
            this.Close();
        }

        private void AdminMain_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddUserForm register = new AddUserForm();
            register.StartPosition = FormStartPosition.Manual;
            register.Location = this.Location;
            register.Show();
            this.Close();
        }
    }
}
