using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
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
    /// Interaction logic for EditMaterial.xaml
    /// </summary>
    public partial class EditMaterial : Window
    {
        private const string ConnectionString = @"Server=LAPTOP-ACER; Database=distantMaterial; Integrated Security=True; Encrypt=False;";
        private readonly int materialId;
        public EditMaterial()
        {
            InitializeComponent();

        }

        public EditMaterial(int materialId, string title, string description)
        {
            InitializeComponent();
            this.materialId = materialId;
            TitleChange.Text= title;
            DescriptionChange.Text= description;    
        }

        private void EditMaterialWindow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Accept(object sender, RoutedEventArgs e)
        {
            try
            {
                string queryAccept = @"update material 
              set title_material =@TitleChange,
              description_material =@DescriptionChange
              where id_material = @materialId";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand usercommand = new SqlCommand(queryAccept, connection);
                    usercommand.Parameters.AddWithValue("@TitleChange", TitleChange.Text);
                    usercommand.Parameters.AddWithValue("@DescriptionChange", DescriptionChange.Text);
                    usercommand.Parameters.AddWithValue("@materialId", materialId);


                    connection.Open();
                    object result = usercommand.ExecuteScalar();

                    int rowsAffected = usercommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Material edited successfully.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                        ResetFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to edit material.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            TitleChange.Text = " ";
            DescriptionChange.Text = "";
            
        }
        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_TitleChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_DescriptionChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
