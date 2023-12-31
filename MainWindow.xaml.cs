﻿using Net_Test_1_V4___Jaza_Arif.AppData;
using Net_Test_1_V4___Jaza_Arif.Model;
using Net_Test_1_V4___Jaza_Arif.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Net_Test_1_V4___Jaza_Arif
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int PageNo = 1;
        List<int> PageNoList = new List<int>();
        public MainWindow()
        {
            InitializeComponent();
            BindGender();
            BindStatus();
            txtPageNo.Text = PageNo.ToString();
        }
        public void BindGender()
        {
            List<ComboData> ListData = new List<ComboData>();
            ListData.Add(new ComboData { Id = "female", Value = "Female" });
            ListData.Add(new ComboData { Id = "male", Value = "Male" });
            Common.BindComboBox(cmbGender, ListData);
            Common.BindComboBox(cmbGenderAdd, ListData, false);
        }
        public void BindStatus()
        {
            List<ComboData> ListData = new List<ComboData>();
            ListData.Add(new ComboData { Id = "active", Value = "Active" });
            ListData.Add(new ComboData { Id = "inactive", Value = "In Active" });
            Common.BindComboBox(cmbStatus, ListData);
            Common.BindComboBox(cmbStatusAdd, ListData, false);
        }
        public async void BindUsers()
        {
            var userService = new UserService();
            string filters = GetFilters();
            List<User> users = await userService.GetUsersAsync(PageNo, filters);

            if (users != null && users.Count > 0)
            {
                // Bind the list of users to your ListBox
                gridUsers.ItemsSource = users;
                PageNoList.Add(PageNo);
            }
            else
            {
                PageNo = PageNoList.Last();
                txtPageNo.Text = PageNo.ToString();
                if (users.Count == 0)
                {
                    gridUsers.ItemsSource = users;
                }
                // Handle error when fetching the user list
            }


        }

        private void btnPagePrev_Click(object sender, RoutedEventArgs e)
        {
            PageNo = PageNo > 0 ? PageNo-- : 1;
            txtPageNo.Text = PageNo.ToString();
        }

        private void btnPageNext_Click(object sender, RoutedEventArgs e)
        {
            PageNo++;
            txtPageNo.Text = PageNo.ToString();
        }

        private async void txtPageNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Delay for 500 milliseconds (adjust as needed)
            await Task.Delay(1500);
            if (!string.IsNullOrEmpty(txtPageNo.Text))
            {
                PageNo = Convert.ToInt16(txtPageNo.Text);
                BindUsers();
            }

        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            ResetAddControl();
            registrationPopup.IsOpen = true;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            BindUsers();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            BindGender();
            BindStatus();
            ResetSearchControl();
            PageNo = 1;
            txtPageNo.Text = PageNo.ToString();
            BindUsers();
        }

        private void gridUsers_AutoGeneratedColumns(object sender, EventArgs e)
        {
            var grid = (DataGrid)sender;
            foreach (var item in grid.Columns)
            {
                if (item.Header.ToString() == "Delete")
                {
                    item.DisplayIndex = grid.Columns.Count - 1;

                }
                if (item.Header.ToString() == "Update")
                {
                    item.DisplayIndex = grid.Columns.Count - 1;

                }
            }
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Perform validation (e.g., check if the input is not empty)
            if (ValidateControls().Length == 0)
            {
                var userService = new UserService();

                User user = new User();
                user.name = txtNameAdd.Text.Trim();
                user.email = txtEmailAdd.Text.Trim();
                user.gender = cmbGenderAdd.SelectedValue.ToString();
                user.status = cmbStatusAdd.SelectedValue.ToString();
                if (hfID.Text == "0")
                {
                    //user.id = Convert.ToInt32(txtIDAdd.Text.Trim());

                    User users = await userService.CreateUserAsync(user);
                    MessageBox.Show("Record saved!");
                }
                else
                {
                    user.id = Convert.ToInt32(hfID.Text.Trim());
                    User users = await userService.UpdateUserAsync(user.id, user);

                    MessageBox.Show("Record updated!");
                }

                BindUsers();
            }
            else
            {
                // Validation failed, display an error message

                MessageBox.Show(ValidateControls(), "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void gridUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if an item is selected
            if (gridUsers.SelectedItem != null)
            {
                // Cast the selected item to your data type (replace YourDataType with your actual data type)
                User selectedData = (User)gridUsers.SelectedItem;

                // Now, you can access properties of the selected data
                txtEmailAdd.Text = selectedData.email;
                txtIDAdd.Text = selectedData.id.ToString();
                txtNameAdd.Text = selectedData.name;
                cmbGenderAdd.SelectedValue = selectedData.gender;
                cmbStatusAdd.SelectedValue = selectedData.status;
                registrationPopup.IsOpen = true;
                // Use the selected data as needed
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            registrationPopup.IsOpen = false;
        }

        private async void btnDelete_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // User clicked "Yes," perform the action
                System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
                // Cast the selected item to your data type (replace YourDataType with your actual data type)
                User user = (User)button.DataContext;

                var userService = new UserService();
                bool res = await userService.DeleteUserAsync(user.id);
                if (res)
                {
                    MessageBox.Show("Record deleted!");
                    BindUsers();
                }

            }
            else
            {
                // User clicked "No" or closed the dialog
            }
        }

        private void btnUpdate_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to update this record?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                registrationPopup.IsOpen = true;
                // User clicked "Yes," perform the action

                System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
                // Cast the selected item to your data type (replace YourDataType with your actual data type)
                User selectedData = (User)button.DataContext;

                // Now, you can access properties of the selected data
                hfID.Text = selectedData.id.ToString();
                txtEmailAdd.Text = selectedData.email;
                txtIDAdd.Text = selectedData.id.ToString();
                txtNameAdd.Text = selectedData.name;
                cmbGenderAdd.SelectedValue = selectedData.gender;
                cmbStatusAdd.SelectedValue = selectedData.status;
                registrationPopup.IsOpen = true;
                // Use the selected data as needed
            }
            else
            {
                // User clicked "No" or closed the dialog
            }
        }

        public void ResetAddControl()
        {
            txtEmailAdd.Text = "";
            txtIDAdd.Text = "";
            txtNameAdd.Text = "";
            cmbGenderAdd.SelectedValue = "0";
            cmbStatusAdd.SelectedValue = "0";
            hfID.Text = "0";
        }
        public void ResetSearchControl()
        {
            txtEmail.Text = "";
            txtID.Text = "";
            txtName.Text = "";
            cmbGender.SelectedValue = "0";
            cmbStatus.SelectedValue = "0";
        }

        public string GetFilters()
        {

            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(txtID.Text.Trim()))
            {
                sb.Append("id=");
                sb.Append(txtID.Text);
                sb.Append("&");

            }
            if (!string.IsNullOrEmpty(txtName.Text.Trim()))
            {

                sb.Append("name=");
                sb.Append(txtName.Text);
                sb.Append("&");

            }
            if (!string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {

                sb.Append("email=");
                sb.Append(txtEmail.Text);
                sb.Append("&");

            }
            if (cmbGender.SelectedValue != "0")
            {

                sb.Append("gender=");
                sb.Append(cmbGender.SelectedValue);
                sb.Append("&");

            }
            if (cmbStatus.SelectedValue != "0")
            {
                sb.Append("status=");
                sb.Append(cmbStatus.SelectedValue);

            }
            return sb.ToString();
        }

        public string ValidateControls()
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(txtEmailAdd.Text.Trim()))
            {
                sb.Append("Email is missing! ");
            }
            if (string.IsNullOrEmpty(txtNameAdd.Text.Trim()))
            {
                sb.Append(" Name is missing! ");
            }
            if (cmbGenderAdd.SelectedValue == "0")
            {
                sb.Append(" Gender is missing! ");
            }
            if (cmbStatusAdd.SelectedValue == "0")
            {
                sb.Append(" Status is missing! ");
            }

            return sb.ToString();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create a StringBuilder to build the CSV content
                StringBuilder csvContent = new StringBuilder();

                // Add headers
                csvContent.AppendLine("ID,Name,Email,Gender,Status");

                // Add data
                foreach (User user in gridUsers.Items)
                {
                    csvContent.AppendLine($"{user.id},{user.name},{user.email},{user.gender},{user.status}");
                }

                // Save to a CSV file
                File.WriteAllText("ExportedData.csv", csvContent.ToString());

                MessageBox.Show("File has been exported successfully please find the file in <bin<Debug<net7.0-windows folder!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

        }

    }
}
