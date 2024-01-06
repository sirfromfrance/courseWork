using System;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace corseWork
{
    public partial class loginWindow : Window
    {
        public loginWindow()
        {
            InitializeComponent();
            LoadEventLogDate();
        }

        private void LoadEventLogDate()
        {
            try
            {
                string connectionString = @"Server=LAPTOP-ACER; Database=distantMaterial; Integrated Security=True; Encrypt=False;";
                string query = "SELECT * FROM users";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);
                   // userDataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void Login_btn(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;

            try
            {
                string connectionString = @"Server=LAPTOP-ACER; Database=distantMaterial; Integrated Security=True; Encrypt=False;";
                string query = "SELECT id_role FROM users WHERE username = @Username AND userPassword = @Password";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        int roleId = Convert.ToInt32(result);

                        // Відкриваємо вікно в залежності від ролі користувача
                        if (roleId == 1) // Адміністратор
                        {
                            var adminWindow = new admin();
                            adminWindow.Show();
                        }
                        else if (roleId == 2) // Студент
                        {
                            var studentWindow = new student();
                            studentWindow.Show();
                        }
                        else if (roleId == 3) // Викладач
                        {
                            var teacherWindow = new teacher();
                            teacherWindow.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }



        private void UserName_textbox(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
