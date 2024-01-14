using Microsoft.Data.SqlClient;
using System;
using System.Collections;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace corseWork
{
    /// <summary>
    /// Interaction logic for admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        private const string ConnectionString = @"Server=LAPTOP-ACER; Database=distantMaterial; Integrated Security=True; Encrypt=False;";
        public admin()
        {

            InitializeComponent();
            LoadMaterials("", "", "", "", "", "");
            LoadComment("", "", "");

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
                       materialsDataGrid.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
        }
        private void   LoadComment (string comment, string markMaterial, string title) {
            string queryComment = @"SELECT * from comment            ";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(queryComment, connection))
                {
                    command.Parameters.AddWithValue("@Title", "%" + title + "%");
                    command.Parameters.AddWithValue("@Mark", (markMaterial));
                    command.Parameters.AddWithValue("@Comment", (comment));
                   
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        commentDataGrid.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
        }
        private void Button_DeleteMaterial(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)materialsDataGrid.SelectedItem;

            if (selectedRow != null)
            {
                int materialId = Convert.ToInt32(selectedRow["MaterialID"]);

                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this material?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DeleteMaterial(materialId);
                    LoadMaterials("", "", "", "", "", ""); 
                }

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

        private void Button_DeleteComment(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)commentDataGrid.SelectedItem;

            if (selectedRow != null)
            {
                int commentId = Convert.ToInt32(selectedRow["id_comment"]);

                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this comment?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DeleteComment(commentId);
                    LoadComment("","","");
                }
               
            }
        }

        private void DeleteComment(int commentId)
        {
            string deleteCommentQuery = "DELETE FROM comment WHERE id_comment = @CommentId;";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(deleteCommentQuery, connection))
                {
                    command.Parameters.AddWithValue("@CommentId", commentId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected <= 0)
                    {
                        MessageBox.Show("Failed to delete comment.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
