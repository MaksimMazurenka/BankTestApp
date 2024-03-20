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
    public partial class AddRate : Form
    {
        List<Currency> currencies;
        Rate rate;
        public AddRate()
        {
            InitializeComponent();
            getCur();

        }

        private async void getRat()
        {
            rate =  await Rate.getRate(comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString());
        }
        private async void getCur()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            currencies = await Currency.getCurrencies();
 
            for (int i = 0; i < currencies.Count; i++)
            {
                comboBox1.Items.Add(currencies[i].currencyCode);
                comboBox2.Items.Add(currencies[i].currencyCode);
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private async void button1_Click(object sender, EventArgs e)//Банковские данные 
        {
            if (comboBox1.SelectedItem.Equals(comboBox2.SelectedItem))
            {
                label4.Visible = true;
                label4.Text = "Валюты не должны повторяться";
            }
            else
            {
                getRat();
                if (rate.Equals(null))
                {
                    label4.Visible = true;
                    label4.Text = "Ошибка запроса";
                }
                else
                {

                    try
                    {
                        if (comboBox1.SelectedItem.Equals("BYN"))
                        {

                            nbrbAPI currency1 = await nbrbAPI.GetCatFactAsync(comboBox2.SelectedItem.ToString());
                            float rat = 1 * currency1.cur_scale  / (currency1.cur_officialRate);
                            textBox1.Text = rat.ToString();
                            textBox2.Text = 1.ToString();
                            label4.Visible = false;

                        }
                        else
                        {
                            if (comboBox2.SelectedItem.Equals("BYN"))
                            {
                                nbrbAPI currency = await nbrbAPI.GetCatFactAsync(comboBox1.SelectedItem.ToString());
                                textBox1.Text = (currency.cur_officialRate).ToString();
                                textBox2.Text = currency.cur_scale.ToString();
                                label4.Visible = false;
                                
                            }
                            else
                            {
                                nbrbAPI currency1 = await nbrbAPI.GetCatFactAsync(comboBox1.SelectedItem.ToString());
                                nbrbAPI currency2 = await nbrbAPI.GetCatFactAsync(comboBox2.SelectedItem.ToString());
                                float rat = (currency1.cur_officialRate * currency2.cur_scale) / (currency1.cur_scale * currency2.cur_officialRate);
                                textBox1.Text = rat.ToString();
                                textBox2.Text = 1.ToString();
                                label4.Visible = false;
                            }
                        }
                    }
                    catch (Exception ew)
                    {
                        label4.Visible = true;
                        label4.ForeColor = Color.Red;
                        label4.Text = "Нет ответа от ЦБРБ";
                    }
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)//Сохранить 
        {
            if(!comboBox1.SelectedItem.Equals(comboBox2.SelectedItem) && textBox1.Text.Length>0 && textBox2.Text.Length > 0)
            {
                decimal rate;
                int scale;
                bool isNumber1 = decimal.TryParse(textBox1.Text, out rate);
                bool isNumber2 = int.TryParse(textBox2.Text, out scale);
                if(isNumber1 && isNumber2)
                {
                    if(await Admin.setRateAsync(comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString(),rate, scale))
                    {
                        await DBController.writeLog("Адмнистратор добавил курс для " + comboBox1.SelectedItem.ToString() + "/"+ comboBox2.SelectedItem.ToString(), Login.curUserId);
                        label4.Visible = true;
                        label4.ForeColor = Color.Green;
                        label4.Text = "Успешно добалено";
                    }
                    else
                    {
                        label4.Visible = true;
                        label4.ForeColor = Color.Red;
                        label4.Text = "Ошибка";
                    }
                }
                else
                {
                    label4.Visible = true;
                    label4.ForeColor = Color.Red;
                    label4.Text = "Проверте поля курса и коэфициента";
                }

            }
        }

        private void AddRate_Load(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button3_Click(object sender, EventArgs e)//Добавить валюту
        {
            if(textBox3.Text.Length==3 && textBox4.Text.Length > 0)
            {
                Currency currency = await Currency.getCurrencieByCode(textBox3.Text.ToString());
                if (currency.currencyCode == null)
                {
                    if(await Admin.setCurrencyAsync(textBox3.Text.ToString(), textBox4.Text.ToString()))
                    {
                        await DBController.writeLog("Адмнистратор добавил валюту " + textBox3.Text.ToString(), Login.curUserId);
                        label4.Visible = true;
                        label4.ForeColor = Color.Green;
                        label4.Text = "Валюта добавлена";
                        getCur();
                    }
                    else
                    {
                        label4.Visible = true;
                        label4.ForeColor = Color.Red;
                        label4.Text = "Ошибка";
                    }
                }
            }
            else
            {
                label4.Visible = true;
                label4.ForeColor = Color.Red;
                label4.Text = "Проверте поля кода и названия";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AdminMain register = new AdminMain();
            register.StartPosition = FormStartPosition.Manual;
            register.Location = this.Location;
            register.Show();
            this.Close();
        }
    }
}
