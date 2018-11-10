using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ConsumeWebApi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Load += async (s, e) =>
            {
                await  GetAllProducts();
            };
        }


        //READ
        private async Task GetAllProducts()
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://localhost:44386/api/demo"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var productJsonString = await response.Content.ReadAsStringAsync();
                        gridControl1.DataSource = JsonConvert.DeserializeObject<List<PersonDto>>(productJsonString).ToList();

                    }
                    
                }
            }
        }

        //CREATE
        private async Task AddPerson()
        {
            PersonDto p = new PersonDto();
       
            p.FirstName = "ABC";
            p.LasteName = "DEF";
         
            using (var client = new HttpClient())
            {
                var serializedProduct = JsonConvert.SerializeObject(p);
                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                var result = await client.PostAsync("YOUR URI HERE", content);
            }
           await GetAllProducts();
        }

        //UPDATE
        private async Task UpdatePerson()
        {
            PersonDto p = new PersonDto();
            //add an id for update here
            p.FirstName = "ABC";
            p.LasteName = "DEFFFFF";  //changed property

            using (var client = new HttpClient())
            {
                var serializedProduct = JsonConvert.SerializeObject(p);
                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                var result = await client.PutAsync(String.Format("{0}/{1}", "YOUR URI HERE", "YOUR ID HERE"), content);
            }
            await GetAllProducts();
        }


        private async Task DeletePerson()
        {
            using (var client = new HttpClient())
            {
                var result = await client.DeleteAsync(String.Format("{0}/{1}", "URI HERE", "ID HERE"));
            }
           await GetAllProducts();
        }



    }
    public class PersonDto
    {
        public string FirstName { get; set; }
        public string LasteName { get; set; }
    }
}
