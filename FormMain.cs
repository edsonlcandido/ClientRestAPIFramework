using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ClienteRestAPIFramework
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            
        }

        private async void buttonGET_Users_Click(object sender, EventArgs e)
        {
            HttpClient restAPI = new HttpClient();
            restAPI.BaseAddress = new Uri("https://reqres.in");
            HttpResponseMessage res = await restAPI.GetAsync("/api/users");
            //Debug.Write(res.Content.ReadAsStringAsync().Result);
            string resContent = res.Content.ReadAsStringAsync().Result;
            JObject resObj = JObject.Parse(resContent);
            List<JToken> resUsers = resObj["data"].Children().ToList();
            List<User> users = new List<User>();

            foreach (JToken resUser in resUsers)
            {
                User user = resUser.ToObject<User>();
                users.Add(user);
            }

            dataGridView1.DataSource = ConvertToDataTable(users);
            dataGridView1.Columns[0].Visible = false; 
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }

    class User
    {
        public int id { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string avatar { get; set; }
    }
}
