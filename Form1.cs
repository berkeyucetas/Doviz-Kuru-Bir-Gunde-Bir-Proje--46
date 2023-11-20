using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Döviz_Kuru
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "https://kur.doviz.com/serbest-piyasa/amerikan-dolari.xml";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode(); 

                    string xmlString = await response.Content.ReadAsStringAsync();

                    XDocument xmlDoc = XDocument.Parse(xmlString);
                    DateTime tarih = DateTime.Parse(xmlDoc.Root.Attribute("Tarih").Value);

                    string DOLAR = xmlDoc.Descendants("Currency")
                                       .First(c => c.Attribute("Kod").Value == "USD")
                                       .Element("BanknoteSelling")
                                       .Value;
                    label1.Text = $"Tarih: {tarih.ToString()} USD: {DOLAR}";

                    string EURO = xmlDoc.Descendants("Currency")
                                       .First(c => c.Attribute("Kod").Value == "EUR")
                                       .Element("BanknoteSelling")
                                       .Value;
                    label2.Text = $"Tarih: {tarih.ToString()} EUR: {EURO}";

                    string POUND = xmlDoc.Descendants("Currency")
                                       .First(c => c.Attribute("Kod").Value == "GBP")
                                       .Element("BanknoteSelling")
                                       .Value;
                    label3.Text = $"Tarih: {tarih.ToString()} GBP: {POUND}";
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show("HTTP isteği sırasında hata oluştu: " + ex.Message);
                label1.Text = "Tarih= 20.11.2023 - Dolar Kuru: 28,7499   ";
                label2.Text = "Tarih= 20.11.2023 - Euro Kuru: 31,4455   ";
                label3.Text = "Tarih= 20.11.2023 - Pound Kuru: 35,9561   ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Beklenmeyen bir hata oluştu: " + ex.Message);
                label1.Text = "Tarih= 20.11.2023 - Dolar Kuru: 28,7499   ";
                label2.Text = "Tarih= 20.11.2023 - Euro Kuru: 31,4455   ";
                label3.Text = "Tarih= 20.11.2023 - Pound Kuru: 35,9561   ";
            }
        }
    }
}
