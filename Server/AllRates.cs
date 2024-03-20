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
    public partial class AllRates : Form
    {
        List<Currency> currencies = new List<Currency>();
        List<Rate> rates = new List<Rate>();
        static bool radio = true;
        private bool cur1;
        private bool cur2;
        private int trigger = 0;
        public AllRates()
        {
            InitializeComponent();

            comboBox4.Visible = false;
            comboBox4.Items.Add(0);
            comboBox4.Items.Add(1);

            radioButton1.Checked = true;
            dateTimePicker1.Enabled = false;
            Add_Row_To_DataGridView_Using_TextBoxes_Load();
            getCur();
            
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

        private void Add_Row_To_DataGridView_Using_TextBoxes_Load()
        {
            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.ColumnCount = 5;

            dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;

            dataGridView1.Columns[0].Name = "Исходная валюта";
            dataGridView1.Columns[1].Name = "Целевая валюта";
            dataGridView1.Columns[2].Name = "Курс обмена";
            dataGridView1.Columns[3].Name = "Коэффициент";
            dataGridView1.Columns[4].Name = "Дата последнего изменения";
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
        }


        private async Task getExchangesByUser()
        {
            dataGridView1.Rows.Clear();
            if (rates != null)
            {
                try
                {
                    for (int i = 0; i < rates.Count; i++)
                    {
                        ArrayList al = new ArrayList();
                        al.Add(rates[i].CurrencyFrom);
                        al.Add(rates[i].CurrencyTo);
                        al.Add(rates[i].ExchangeRate);
                        al.Add(rates[i].Scale);
                        al.Add(rates[i].Date.ToString("dd.MM.yyyy hh:mm"));
                        dataGridView1.Rows.Add(al.ToArray());
                    }
                }
                catch(Exception e)
                {

                }

            }
        }

        private void radioButtonPresed(object sender, EventArgs e)
        {
            radio = !radio;
            radioButton1.Checked = radio;
            if (radioButton1.Checked)
            {
                dateTimePicker1.Enabled = false;
            }
            else
            {
                dateTimePicker1.Enabled = true;
            }
            triggerAkt();
        }

        private async void selectedUserChangedAsync(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1)
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

                switch (!radioButton1.Checked)
                {
                    case true:
                        switch (cur1)
                        {
                            case true:
                                switch (cur2)
                                {
                                    case true:
                                        rates = await Rate.getAllRatesByDateCur1Cur2(dateTimePicker1.Value.Date, comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString());
                                        break;
                                    case false:
                                        rates = await Rate.getAllRatesByDateCur1(dateTimePicker1.Value.Date, comboBox2.SelectedItem.ToString());
                                        break;
                                }
                                break;
                            case false:
                                switch (cur2)
                                {
                                    case true:
                                        rates = await Rate.getAllRatesByDateCur2(dateTimePicker1.Value.Date, comboBox3.SelectedItem.ToString());
                                        break;
                                    case false:
                                        rates = await Rate.getAllRatesByDate(dateTimePicker1.Value.Date);
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
                                        rates = await Rate.getAllRatesByCur1Cur2(comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString());
                                        break;
                                    case false:
                                        rates = await Rate.getAllRatesByCur1(comboBox2.SelectedItem.ToString());
                                        break;
                                }
                                break;
                            case false:
                                switch (cur2)
                                {
                                    case true:
                                        rates = await Rate.getAllRatesByCur2(comboBox3.SelectedItem.ToString());
                                        break;
                                    case false:
                                        rates = await Rate.getAllRatesWithDate();
                                        break;
                                }
                                break;
                        }
                        break;
                }
                getExchangesByUser();
            }
                
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Exchange register = new Exchange();
            register.StartPosition = FormStartPosition.Manual;
            register.Location = this.Location;
            register.Show();
            this.Close();
        }

        private void AllRates_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
