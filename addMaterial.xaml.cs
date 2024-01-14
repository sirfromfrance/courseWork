using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
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
using static corseWork.student;

namespace corseWork
{
    /// <summary>
    /// Interaction logic for addMaterial.xaml
    /// </summary>
    public partial class addMaterial : Window
    {
        private const string ConnectionString = @"Server=LAPTOP-ACER; Database=distantMaterial; Integrated Security=True; Encrypt=False;";
        private readonly int teacherId;
        public addMaterial(int teacherId)
        {
            InitializeComponent();
            this.teacherId = teacherId;
            ComboBoxHelper.MaterialYears(YearBox);
            ComboBoxHelper.MaterialLanguages(LanguageBox);
            ComboBoxHelper.MaterialCourses(CourseBox);
        }

        private void Button_addMaterial(object sender, RoutedEventArgs e)
        {
            try
            {
                string title = TextBox_AddTitle.Text;
                string description = TextBox_AddDescription.Text;
                string language =(string)LanguageBox.SelectedValue ;
                string year =(string)YearBox.SelectedValue;
                string course = (string)CourseBox.SelectedValue;
                
                // string comment = "";
                int languageId;
                int yearId;
                int courseId; 
                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(language) || string.IsNullOrEmpty(year) || string.IsNullOrEmpty(course))
                {
                    MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string queryLanguage = @"select id_language  
                  from language
                  where language.language =@Language";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand usercommand = new SqlCommand(queryLanguage, connection);
                    usercommand.Parameters.AddWithValue("@Language", language);

                    connection.Open();

                    object result = usercommand.ExecuteScalar();
                    languageId = Convert.ToInt32(result);
                   
                }
                string queryYear = @"select id_year
                   from yearMaterial
                   where yearMaterials = @Year";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand usercommand = new SqlCommand(queryYear, connection);
                    usercommand.Parameters.AddWithValue("@Year",year);
                    connection.Open();
                    object result = usercommand.ExecuteScalar();
                    yearId = Convert.ToInt32(result);   
                }
                string queryCourse = @"select id_course
                  from course
                  where courseMaterial = @Course";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand usercommand = new SqlCommand(queryCourse, connection);
                    usercommand.Parameters.AddWithValue("@Course", course);
                    connection.Open();
                    object result = usercommand.ExecuteScalar();
                    courseId = Convert.ToInt32(result);
                }
                string insertQuery = @"
                  INSERT INTO material (title_material, description_material,id_course,id_language,id_year, id_teacher)
                  VALUES (@Title, @Description,@Course,@Language,@Year, @TeacherId);
                  DECLARE @MaterialId INT;
                  SET @MaterialId = SCOPE_IDENTITY();
                    ";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@TeacherId", teacherId);
                        command.Parameters.AddWithValue("@Language", languageId);
                        command.Parameters.AddWithValue("@Year", yearId);
                        command.Parameters.AddWithValue("@Course", courseId);
                        //command.Parameters.AddWithValue("@Comment", comment);


                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Material added successfully.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            
                           ResetFields();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add material.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       
        public void ResetFields()
        {
            TextBox_AddTitle.Text = " ";
            TextBox_AddDescription.Text = "";
            LanguageBox.Text = "";
            YearBox.Text = "";
            CourseBox.Text = "";
        }
        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditMaterialWindow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        private void TextBox_AddTtitle(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_DescriptionAdd(object sender, TextChangedEventArgs e)
        {

        }

       
        private void ComboBox_Language(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_Year(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_Course(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
