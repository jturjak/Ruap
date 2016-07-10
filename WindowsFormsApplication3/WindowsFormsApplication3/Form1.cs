using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;


namespace WindowsFormsApplication3
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string[] dani = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30" };
            string[] mjeseci = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            string[] godine = new string[166];
                        for (int i = 1850; i < 2016; i++)
                        
                            godine[i - 1850] = i.ToString();
                      
            foreach (string s in godine)
                comboBox3.Items.Add(s);
            comboBox3.SelectedIndex = 0;
            foreach (string s in dani)
                comboBox1.Items.Add(s);
            comboBox1.SelectedIndex = 0;
            foreach (string s in mjeseci)
                comboBox2.Items.Add(s);
            comboBox2.SelectedIndex = 0;
        }
        public class StringTable
        {
            public string[] ColumnNames { get; set; }
            public string[,] Values { get; set; }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            
            string k = textBox1.Text.ToString(), result, mx = textBox2.Text.ToString(), mi = textBox3.Text.ToString(), s = textBox4.Text.ToString();
            string datum = comboBox2.Text+"-" + comboBox2.Text + "-" + comboBox1.Text;
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, StringTable>() { 
                        { 
                            "input1", 
                            new StringTable() 
                            {
                                ColumnNames = new string[] {"dt", "LandAverageTemperature", "LandMaxTemperature", "LandMinTemperature", "LandAndOceanAverageTemperature"},
                                Values = new string[,] {  { datum, k, mx, mi, s }  }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                const string apiKey = "Sh6w1sPgtQ64mc23JyJ+useFPwMfp9pba4s1w3EggX6e5O7ozllz7ZPamdt/mwp6mwNgaPYq7SbTUPap917NAg=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/1075110f45444a87b66616d0048c5152/services/175b792875cc40dc9003bac066b75b63/execute?api-version=2.0&details=true");

                // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)


                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

                if (response.IsSuccessStatusCode)
                {
                     result = await response.Content.ReadAsStringAsync();
                    
                }
                else
                {
                   

                    // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
            

                    result = await response.Content.ReadAsStringAsync();
                    

                }

                result = transform(result);
                if (result[0] == 'F' || result[0] == 'E' || result[0] == 'P') label7.Text = "Pogrešno unešeni podaci!";
                else    
                //label7.Text = pomoc.ToString();
                    label7.Text = result;
            } 
        }
       public string transform (string result) 
        {
            string rezultat = "", help = "";
            int pomoc = 0;
             for (int i = result.Length-1; i > 0; i--)
                            {
                   
                                if (result[i] == '"'&& pomoc==1 )
                                {
                                    pomoc = 0;
                       
                                    break;
                                }
                                if (pomoc == 1 ) 
                                {
                                    help += result[i];
                        
                                }
                                if (result[i] == '"' &&  pomoc == 0) 
                                {                       
                                    pomoc = 1;
                       
                                }

                            }
                            
              
                            for (int i = help.Length - 1; i > 0; i--)
                            {
                                rezultat+=help[i];
                   
                            }

            return rezultat;
        }

    }
}
