using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace Modul15
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Display Form2
            Form2 form2 = new Form2();
            form2.Show();

            // Close Form1
            this.Hide();
        }

        private List<UserData> LoadUserData()
        {
            // Specify the file path
            string filePath = "userData.json";

            if (!File.Exists(filePath))
                return null;

            // Read from the file
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON data
            return JsonConvert.DeserializeObject<List<UserData>>(json);
        }

        private void SaveUserData(List<UserData> users)
        {
            // Specify the file path
            string filePath = "userData.json";

            // Serialize the list of users to JSON
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);

            // Write to the file
            File.WriteAllText(filePath, json);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            // Load user data from the JSON file
            var users = LoadUserData();
            if (users == null)
            {
                MessageBox.Show("No user data found.");
                return;
            }

            // Find the user by username
            var user = users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                MessageBox.Show("Invalid username or password.");
                return;
            }

            // Hash the entered password
            string hashedPassword = HashPassword(password);

            // Compare hashed password with the stored hashed password
            if (hashedPassword != user.Password)
            {
                MessageBox.Show("Invalid username or password.");
                return;
            }

            MessageBox.Show("Login successful!");

            // Display Form3
            Form3 form3 = new Form3();
            form3.Show();

            // Hide or close Form1
            this.Hide();
        }
    }
}
