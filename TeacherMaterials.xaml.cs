using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace corseWork
{
    /// <summary>
    /// Interaction logic for TeacherMaterials.xaml
    /// </summary>
    public partial class TeacherMaterials : Window

    {
        private const string ConnectionString = @"Server=LAPTOP-ACER; Database=distantMaterial; Integrated Security=True; Encrypt=False;";
        public string teacherUsername;

        public TeacherMaterials(string title, string description, string year, string language, string course,  string username)
        {
            InitializeComponent();
            this.teacherUsername = username;
            LoadTeacherMaterials( title,  description,  year,  language,  course,  username);
          
        }
        private void LoadTeacherMaterials(string title, string description, string year, string language, string course,  string username)
        {
            string query = @"
             SELECT  
    users.username AS 'Username',
    m.title_material AS 'Title', 
    m.id_material AS 'MaterialID',
    m.description_material AS 'Description',
    l.language AS 'Language', 
    y.yearMaterials AS 'Year', 
    c.courseMaterial AS 'Course'
  
FROM
    users
    LEFT JOIN teacher ON users.id_user = teacher.id_user
    LEFT JOIN material m ON teacher.id_teacher = m.id_teacher
   
    LEFT JOIN language l ON m.id_language = l.id_language 
    LEFT JOIN yearMaterial y ON m.id_year = y.id_year 
    LEFT JOIN course c ON m.id_course = c.id_course 
WHERE username = @TeacherUsername;
";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TeacherUsername", username);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        if (dataTable.Rows.Count > 0)
                        {
                            TeachermaterialGrid.ItemsSource = dataTable.DefaultView;
                        }
                        else
                        {
                            MessageBox.Show("No materials found for the logged-in teacher.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        
        }
        public int GetTeacherIdForUser(string username)
        {
            
            string teacherQuery = "SELECT id_user FROM users WHERE username = @Username";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand teacherCommand = new SqlCommand(teacherQuery, connection);
                teacherCommand.Parameters.AddWithValue("@Username", username);

                connection.Open();

                object result = teacherCommand.ExecuteScalar();

                if (result != null)
                {
                    return Convert.ToInt32(result);
                }

                return 0; // Якщо вчителя не знайдено
            }
        }
        private void Button_addMaterial(object sender, RoutedEventArgs e)
        {
            
            int userId = GetTeacherIdForUser(CurrentUser.username);
            string query = "select id_teacher from teacher where teacher.id_user = @UserId";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand usercommand = new SqlCommand(query, connection);
               usercommand.Parameters.AddWithValue("@UserId", userId);

                connection.Open();

                object result = usercommand.ExecuteScalar();
               int res  =  Convert.ToInt32(result);
                addMaterial addMaterialWindow = new addMaterial(res);

                addMaterialWindow.ShowDialog();

            }
                    
            LoadTeacherMaterials("", "", "", "", "",  teacherUsername);
        }

        private void Button_deleteMaterial(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)TeachermaterialGrid.SelectedItem;

            if (selectedRow != null)
            {
                int materialId = Convert.ToInt32(selectedRow["MaterialID"]);

                
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this material?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                   
                    DeleteMaterial(materialId);

                    LoadTeacherMaterials("", "", "", "", "", teacherUsername);
                }
            }
            else
            {
                MessageBox.Show("Please select a material to delete.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteMaterial(int materialId)
        {
            string deleteQuery = "DELETE FROM material WHERE id_material = @MaterialId;";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@MaterialId", materialId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected <= 0)
                    {
                        MessageBox.Show("Failed to delete material.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Button_EditMaterial(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)TeachermaterialGrid.SelectedItem;

            if (selectedRow != null)
            {
               
                int materialId = Convert.ToInt32(selectedRow["MaterialID"]);
                string title = selectedRow["Title"].ToString();
                string description = selectedRow["Description"].ToString();

                EditMaterial editMaterialWindow = new EditMaterial (materialId, title, description);

                
                editMaterialWindow.ShowDialog();

                LoadTeacherMaterials("", "", "", "", "", teacherUsername);
            }
            else
            {
                MessageBox.Show("Please select a material to edit.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TeachermaterialGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
    }


