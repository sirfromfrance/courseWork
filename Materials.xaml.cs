using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using static corseWork.student;

namespace corseWork
{
    public partial class Materials : Window
    {
        private const string ConnectionString = @"Server=LAPTOP-ACER; Database=distantMaterial; Integrated Security=True; Encrypt=False;";

        public Materials(string title, string description, string year, string language, string course, string comment)
        {
            InitializeComponent();
            LoadMaterials(title, description, year, language, course, comment);
            LoadGradeComboBox();
        }

        private void LoadMaterials(string title, string description, string year, string language, string course, string comment)
        {
            string query = @"
               
SELECT  
            m.title_material AS 'Title', 
            m.id_material AS 'MaterialID',
            m.description_material AS 'Description',
            l.language AS 'Language', 
            y.yearMaterials AS 'Year', 
            c.courseMaterial AS 'Course',
            com.content_comment as 'comment',
            com.markMaterial as 'mark'
           
        FROM
            material m
            LEFT JOIN comment com on com.id_material= m.id_material
            LEFT JOIN language l ON m.id_language = l.id_language 
            LEFT JOIN yearMaterial y ON m.id_year = y.id_year 
            LEFT JOIN course c ON m.id_course = c.id_course
        WHERE
            m.title_material LIKE @Title
            AND (l.language = @Language OR @Language IS NULL)
            AND (y.yearMaterials = @Year OR @Year IS NULL)
            AND (c.courseMaterial = @Course OR @Course IS NULL);
            ";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", "%" + title + "%");
                    command.Parameters.AddWithValue("@Language", string.IsNullOrEmpty(language) ? (object)DBNull.Value : language);
                    command.Parameters.AddWithValue("@Year", string.IsNullOrEmpty(year) ? (object)DBNull.Value : int.Parse(year));
                    command.Parameters.AddWithValue("@Course", string.IsNullOrEmpty(course) ? (object)DBNull.Value : int.Parse(course));
                    command.Parameters.AddWithValue("@Comment", (comment));
                    command.Parameters.AddWithValue("@Description", (description));


                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        if (dataTable.Rows.Count > 0)
                        {
                            materialGrid.ItemsSource = dataTable.DefaultView;
                        }
                        else
                        {
                            MessageBox.Show("Файлів не знайдено за вказаними критеріями.", "Повідомлення", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                }
            }
        }

        private void LoadGradeComboBox()
        {
            ComboBoxHelper.MaterialMark(GradeMaterial);
        }

        private void Exit_btn(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_Comment(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        }

        private string GetSelectedMaterialTitle()
        {
            if (materialGrid.SelectedItems.Count > 0)
            {
                DataRowView selectedRow = (DataRowView)materialGrid.SelectedItems[0];
                string materialTitle = selectedRow["Title"].ToString();
                return materialTitle;
            }

            return null;
        }

        private string GetSelectedMaterialId()
        {
            if (materialGrid.SelectedItems.Count > 0)
            {
                DataRowView selectedRow = (DataRowView)materialGrid.SelectedItems[0];
                string id_material = selectedRow["MaterialID"].ToString();
                return id_material;
            }

            return null;
        }

        private void Button_PublishComment(object sender, RoutedEventArgs e)
        {
            int markComment=0;
            string selectedMaterialTitle = GetSelectedMaterialTitle();
            string commentContent = commentGrid.Text;
            if (GradeMaterial.SelectedItem != null) {
                 markComment = int.Parse(GradeMaterial.SelectedItem.ToString());
            }


            if (string.IsNullOrEmpty(selectedMaterialTitle) || string.IsNullOrEmpty(commentContent))
            {
                MessageBox.Show("Будь ласка, заповніть всі необхідні поля.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int id_material =Convert.ToInt32(GetSelectedMaterialId());
            InsertCommentIntoDatabase(selectedMaterialTitle, commentContent, markComment,id_material);
            LoadMaterials(selectedMaterialTitle, "", "", "", "", "");
        }

        private void InsertCommentIntoDatabase(string materialTitle, string commentContent, int markComment, int id_material)
        {
            try
         
            {          
                using (SqlConnection connection = new SqlConnection(ConnectionString))
       
                {
                      connection.Open();

                     string insertCommentQuery = @"INSERT INTO comment (content_comment, markMaterial, id_material)
                      VALUES (@Content, @StudentMark,@MaterialId);
                      SELECT SCOPE_IDENTITY();
            ";

            using (SqlCommand command = new SqlCommand(insertCommentQuery, connection))
            {
                command.Parameters.AddWithValue("@Content", commentContent);
                command.Parameters.AddWithValue("@StudentMark", markComment);
                        command.Parameters.AddWithValue("@MaterialId", id_material);
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int newCommentId))
                {
                    MessageBox.Show($"Коментар успішно опубліковано з id {newCommentId}.", "Успішно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Не вдалося отримати ідентифікатор коментаря.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Помилка при роботі з базою даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
    }
        }
        private void UpdateMaterialMark(string materialTitle, double newMark)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "UPDATE mark SET mark = @Mark WHERE id_material = (SELECT id_material FROM material WHERE title_material = @Title)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", materialTitle);
                        command.Parameters.AddWithValue("@Mark", newMark);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при оновленні оцінки матеріалу: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InsertMaterialMark(string materialTitle, int grade)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO mark (id_material, mark) VALUES ((SELECT id_material FROM material WHERE title_material = @Title), @Mark)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", materialTitle);
                        command.Parameters.AddWithValue("@Mark", grade);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при додаванні оцінки матеріалу: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
       
            private void Button_GradeMaterial(object sender, RoutedEventArgs e)
            {
           
                string selectedMaterialTitle = GetSelectedMaterialTitle();
                if (string.IsNullOrEmpty(selectedMaterialTitle))
                {
                    MessageBox.Show("Будь ласка, виберіть матеріал для оцінювання.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

               
                if (GradeMaterial.SelectedItem != null)
                {
                  
                    string gradeString = GradeMaterial.SelectedItem.ToString();

                    if (int.TryParse(gradeString, out int grade))
                    {
                        
                        double currentMark = GetCurrentMaterialMark(selectedMaterialTitle);

                    
                        if (currentMark == -1)
                        {

                            InsertMaterialMark(selectedMaterialTitle, grade);
                        }
                        else
                        {
                         
                            double newMark = (currentMark + grade) / 2;

                            UpdateMaterialMark(selectedMaterialTitle, newMark);
                        }

                   
                        LoadMaterials(selectedMaterialTitle, "", "", "", "", "");
                    }
                    else
                    {
                        MessageBox.Show("Некоректна оцінка. Будь ласка, виберіть правильну оцінку зі списку.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Будь ласка, виберіть оцінку зі списку перед натисканням кнопки.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
   
        }

        private double GetCurrentMaterialMark(string materialTitle)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();

                        string query = "SELECT mark FROM mark WHERE id_material = (SELECT id_material FROM material WHERE title_material = @Title)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Title", materialTitle);

                            object result = command.ExecuteScalar();

                            if (result != null && int.TryParse(result.ToString(), out int currentMark))
                            {
                                return currentMark;
                            }
                            else
                            {
                                return -1; // Оцінки не існує
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при отриманні поточної оцінки матеріалу: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
            }

 

        private void ComboBox_GradeMaterial(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }
    }
}
