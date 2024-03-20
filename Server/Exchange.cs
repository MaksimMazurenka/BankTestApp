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
    public partial class Exchange : Form
    {
        List<Currency> currencies;
        Rate rate;
        static bool radio = true;
        static int first = 0;
        static int second = 0;
        public Exchange()
        {
            InitializeComponent();
            getCur();
            Add_Row_To_DataGridView_Using_TextBoxes_Load();
            

            radioButton1.Checked = radio;
        }

        DataTable table = new DataTable();
        List<ExchangeHistory> exchangeHistories = new List<ExchangeHistory>();

        private void Add_Row_To_DataGridView_Using_TextBoxes_Load()
        {           
            dataGridView1.AllowUserToAddRows = false;


            dataGridView1.ColumnCount = 8;

            for(int i = 0; i < 8;  i++) {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[i].MinimumWidth = 40;
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
            getExchanges();

        }

        private async void getExchanges()
        {
            exchangeHistories = await ExchangeHistory.getTodayExchangeByUser(Login.curUserId);
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
                dataGridView1.Rows.Add(al.ToArray());
            }
        }

        private async void getRat()
        {
            rate = await Rate.getRate(comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString());
        }

        private async Task getCurrency()
        {
            currencies = await Currency.getCurrencies();
        }

        private async void getCur()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            await getCurrency();

            for (int i = 0; i < currencies.Count; i++)
            {
                comboBox1.Items.Add(currencies[i].currencyCode);
                comboBox2.Items.Add(currencies[i].currencyCode);
            }
            comboBox1.SelectedIndex = first;
            comboBox2.SelectedIndex = second;
        }

        private async void button2_Click(object sender, EventArgs e)//готово
        {
            if (label7.Text.Length > 0 && label8.Text.Length > 0)
            {
                decimal rate; decimal.TryParse(label7.Text, out rate);
                int scale; int.TryParse(label8.Text, out scale);

                decimal userSum;
                bool userSumOk = decimal.TryParse(textBox3.Text, out userSum);
                if (userSumOk && userSum > 0)
                {
                    if (radio)
                    {
                        decimal userWant;
                        bool isNumber1 = decimal.TryParse(textBox4.Text, out userWant);
                        if (isNumber1 && userWant > 0)
                        {

                            if (userWant > currencies[comboBox2.SelectedIndex].count)
                            {
                                label4.Visible = true;
                                label4.ForeColor = Color.Red;
                                label4.Text = "Недостаточно валюты";
                            }
                            else
                            {
                                decimal need = userSum - Math.Round(convert(rate, scale, userSum, userWant), 2);
                                if (need < 0)
                                {
                                    label4.Visible = true;
                                    label4.ForeColor = Color.Red;
                                    label4.Text = "Недостаточно денег";
                                }
                                else
                                {
                                    label12.Text = need.ToString();
                                    label11.Text = userWant.ToString();
                                    if (await Cashier.setExchangeAsync(Login.curUserId, comboBox1.SelectedItem.ToString(), userSum, comboBox2.SelectedItem.ToString(), userWant, rate, scale, need))
                                    {
                                        label4.Visible = true;
                                        label4.ForeColor = Color.Green;
                                        label4.Text = "Транзакция сохранена";
                                        await DBController.writeLog("Кассир совершил транзакцию", Login.curUserId);
                                        await getCurrency();
                                        label14.Text = currencies[comboBox2.SelectedIndex].count.ToString();
                                        getExchanges();
                                    }
                                }
                            }
                        }
                        else
                        {
                            label4.Visible = true;
                            label4.ForeColor = Color.Red;
                            label4.Text = "Желаемая сумма введена некорректно";
                        }
                    }
                    else
                    {
                        decimal rezult = Math.Round(convert(rate, scale, userSum), 2);
                        if (rezult > currencies[comboBox2.SelectedIndex].count)
                        {
                            label4.Visible = true;
                            label4.ForeColor = Color.Red;
                            label4.Text = "Недостаточно валюты";
                        }
                        else
                        {
                            label11.Text = rezult.ToString();
                            if (await Cashier.setExchangeAsync(Login.curUserId, comboBox1.SelectedItem.ToString(), userSum, comboBox2.SelectedItem.ToString(), rezult, rate, scale, 0))
                            {
                                label4.Visible = true;
                                label4.ForeColor = Color.Green;
                                label4.Text = "Транзакция сохранена";
                                await DBController.writeLog("Кассир совершил транзакцию", Login.curUserId);
                                getCurrency();
                                label14.Text = currencies[comboBox2.SelectedIndex].count.ToString();
                                getExchanges();
                            }
                        }
                    }
                }
                else
                {
                    label4.Visible = true;
                    label4.ForeColor = Color.Red;
                    label4.Text = "Полученная сумма введена некорректно";
                }
            }
            else
            {
                label4.Visible = true;
                label4.ForeColor = Color.Red;
                label4.Text = "Выберите поддерживаемую валюту";
            }
        }

        private void button1_Click(object sender, EventArgs e) //рассчитать
        {

            if (label7.Text.Length > 0 && label8.Text.Length>0)
            {
                decimal rate; decimal.TryParse(label7.Text, out rate);
                int scale; int.TryParse(label8.Text, out scale);

                decimal userSum;
                bool userSumOk = decimal.TryParse(textBox3.Text, out userSum);
                if (userSumOk && userSum > 0)
                {
                    if (radio)
                    {
                        decimal userWant;
                        bool isNumber1 = decimal.TryParse(textBox4.Text, out userWant);
                        if (isNumber1 && userWant > 0)
                        {
                            if (userWant> currencies[comboBox2.SelectedIndex].count)
                            {
                                label4.Visible = true;
                                label4.ForeColor = Color.Red;
                                label4.Text = "Недостаточно валюты";
                            }
                            else
                            {
                                decimal need = userSum - Math.Round(convert(rate, scale, userSum, userWant), 2);
                                if (need < 0)
                                {
                                    label4.Visible = true;
                                    label4.ForeColor = Color.Red;
                                    label4.Text = "Недостаточно денег";
                                }
                                else
                                {
                                    label12.Text = need.ToString();
                                    label11.Text = userWant.ToString();
                                    label4.Visible = false;
                                }
                            }                
                        }
                        else
                        {
                            label4.Visible = true;
                            label4.ForeColor = Color.Red;
                            label4.Text = "Желаемая сумма введена некорректно";
                        }
                    }
                    else
                    {
                        decimal rezult = Math.Round(convert(rate,scale,userSum),2);
                        if (rezult> currencies[comboBox2.SelectedIndex].count)
                        {
                            label4.Visible = true;
                            label4.ForeColor = Color.Red;
                            label4.Text = "Недостаточно валюты";
                        }
                        else
                        {
                            label11.Text = rezult.ToString();
                            label4.Visible = false;
                        }
                    }
                }
                else
                {
                    label4.Visible = true;
                    label4.ForeColor = Color.Red;
                    label4.Text = "Полученная сумма введена некорректно";
                }
            }
            else
            {
                label4.Visible = true;
                label4.ForeColor = Color.Red;
                label4.Text = "Выберите поддерживаемую валюту";
            }
        }

        private decimal convert(decimal rate, int scale, decimal summ)
        {
           return rate * summ / scale ;
        }

        private decimal convert(decimal rate, int scale, decimal summ, decimal want)
        {
            return want  *scale / (rate );
        }

        private void radioButtonPresed(object sender, EventArgs e)
        {
            radio = !radio;
            radioButton1.Checked = radio;
            if (radioButton1.Checked)
            {
                textBox4.Enabled = true;
            }
            else
            {
                label11.Text = "";
                textBox4.Text = "";
                textBox4.Enabled = false;
            }
        }
        private async void currecncyChanged(object sender, EventArgs e)
        {
            if(first!= comboBox1.SelectedIndex || second!= comboBox2.SelectedIndex)
            {
                first = comboBox1.SelectedIndex;
                second = comboBox2.SelectedIndex;
                getCurrency();
                if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1) { }
                else
                {
                    getRat();
                    label11.Text = "";
                    label12.Text = "";
                    label7.Text = "";
                    label8.Text = "";

                    if (rate.CurrencyFrom == null)
                    {
                        label4.Visible = true;
                        label4.ForeColor = Color.Red;
                        label4.Text = "Обмен данной валюты не поддерживается";
                    }
                    else
                    {
                        label7.Text = rate.ExchangeRate.ToString();
                        label8.Text = rate.Scale.ToString();
                        label14.Text = currencies[comboBox2.SelectedIndex].count.ToString();
                        label4.Visible = false;
                    }
                }
            }           
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            AllRates register = new AllRates();
            register.StartPosition = FormStartPosition.Manual;
            register.Location = this.Location;
            register.Show();
            this.Close();
        }
    }
}
