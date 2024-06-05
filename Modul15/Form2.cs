using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;


namespace Modul15
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            // Validate username and password
            if (!IsValidUsername(username))
            {
                MessageBox.Show("Invalid username. Username should only contain ASCII alphabet characters and numbers, and be between 8 and 20 characters.");
                return;
            }

            if (!IsValidPassword(password, username))
            {
                MessageBox.Show("Invalid password. Password should contain 8-20 characters, include at least one number, one unique character (!@#$%^&*), and should not contain the username.");
                return;
            }

            // Load existing users
            var users = LoadUserData() ?? new List<UserData>();

            // Check if the username already exists
            if (users.Any(u => u.Username == username))
            {
                MessageBox.Show("Username already exists.");
                return;
            }

            // Hash the password
            string hashedPassword = HashPassword(password);

            // Add the new user to the list
            users.Add(new UserData { Username = username, Password = hashedPassword });

            // Save the updated list to the file
            SaveUserData(users);

            MessageBox.Show("Registration successful!");

            // Display Form1
            Form1 form1 = new Form1();
            form1.Show();

            // Close Form2
            this.Close();
        }

        private bool IsValidUsername(string username)
        {
            if (username.Length < 8 || username.Length > 20)
                return false;

            // Check if username contains only ASCII letters and numbers
            if (!Regex.IsMatch(username, "^[a-zA-Z0-9]+$"))
                return false;

            // Check if username contains at least one number
            if (!Regex.IsMatch(username, "[0-9]"))
                return false;

            return true;
        }

        private bool IsValidPassword(string password, string username)
        {
            if (password.Length < 8 || password.Length > 20)
                return false;

            // Check if password contains at least one number
            if (!Regex.IsMatch(password, "[0-9]"))
                return false;

            // Check if password contains at least one unique character
            if (!Regex.IsMatch(password, "[!@#$%^&*]"))
                return false;

            // Check if password contains any part of the username
            if (password.Contains(username))
                return false;

            return true;
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

    }
}
