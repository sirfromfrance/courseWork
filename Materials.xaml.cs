using System;
using System.Collections.Generic;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using System.Windows;
using System.Data;

namespace corseWork
{
    public partial class Materials : Window
    {
        private const string ConnectionString = @"Server=LAPTOP-ACER; Database=distantMaterial; Integrated Security=True; Encrypt=False;";

        public Materials(string title,string description, string year, string language, string course, string mark, string comment)
        {
            InitializeComponent();
            LoadMaterials(title,description, year, language, course, mark, comment);
        }

        private void LoadMaterials(string title,string description, string year, string language, string course, string mark, string comment)
        {
            string query = @"
    SELECT  
        m.title_material AS 'Title', 
m.description_material AS 'Description',
        m.language_material AS 'Language', 
        m.year_material AS 'Year', 
        m.course_material AS 'Course',
        c.content_comment AS 'Comment',
        mk.mark AS 'Mark'
    FROM
        material m
    LEFT JOIN
        comment c ON m.id_material = c.id_material
    LEFT JOIN
        mark mk ON m.id_material = mk.id_material
    WHERE
        m.title_material LIKE @Title
        AND m.language_material = @Language
        AND m.year_material = @Year
        AND m.course_material = @Course;
";
            ;


            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", "%" + title + "%");
                    command.Parameters.AddWithValue("@Language", language);
                    command.Parameters.AddWithValue("@Year", int.Parse(year));
                    command.Parameters.AddWithValue("@Course", int.Parse(course));
                    command.Parameters.AddWithValue("@Mark", int.Parse(mark));
                    command.Parameters.AddWithValue("@Comment",(comment));
                    command.Parameters.AddWithValue("@Description",(description));
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

        private void Exit_btn(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
