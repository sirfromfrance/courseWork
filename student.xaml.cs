using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace corseWork
{
    public partial class student : Window
    {
        private const string ConnectionString = @"Server=LAPTOP-ACER; Database=distantMaterial; Integrated Security=True; Encrypt=False;";
      
        public student()
        {
            InitializeComponent();
           
            ComboBoxHelper.MaterialYears(yearComboBox);
            ComboBoxHelper.MaterialLanguages(languageComboBox); 
            ComboBoxHelper.MaterialCourses(courseComboBox);
            ComboBoxHelper.MaterialMark(markComboBox);

        }
        public class ComboBoxHelper
        {
            public static void MaterialYears(ComboBox comboBox)
            {
                List<string> years = new List<string> {"1984", "1986", "2015","2016","2017","2018","2019","2020", "2021","2022" };
                comboBox.ItemsSource = years;
            }

            public static void MaterialLanguages(ComboBox comboBox) {
                List<string> languages = new List<string> {"English", "Ukrainian" };
                comboBox.ItemsSource = languages;   
                }
            public static void MaterialCourses(ComboBox comboBox)
            {
                List<string> courses = new List<string> {"1","2","3","4"};
                comboBox.ItemsSource = courses; 
            }
            public static void MaterialMark(ComboBox comboBox)
            {
                List<string> mark = new List<string> { "1", "2", "3", "4", "5" };
                comboBox.ItemsSource = mark;
            }
        }
        private void Button_Search(object sender, RoutedEventArgs e)
        {

            string title = searchField.Text;
            string year = (string)yearComboBox.SelectedValue;
            string language = (string)languageComboBox.SelectedValue;
            string course = (string)courseComboBox.SelectedValue;
            string mark = (string)markComboBox.SelectedValue;

            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(title))
            {
                errors.Add("Title is null or empty");
            }
            
            
            string error = default;
            foreach (string tmp in errors)
            {
                error += $"{tmp}\n";
            }

            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error);
                return;
            }

            Materials materialsWindow = new Materials(title, "", year, language, course,  "");
            if (materialsWindow.materialGrid.ItemsSource != null)
            {
                materialsWindow.Show();
            }
        }
        private void Button_exit(object sender, RoutedEventArgs e)
        {
            Close();
        }





        private void TextBox_SearchField(object sender, TextChangedEventArgs e)
        {
           
        }    

        private void ComboBox_Year(object sender, SelectionChangedEventArgs e)
        {      
            
        }
        private void ComboBox_Language(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void ComboBox_Course(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ComboBox_Mark(object sender, SelectionChangedEventArgs e)
        {
            
        }

        
       
    }
}
