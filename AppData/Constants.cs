using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Net_Test_1_V4___Jaza_Arif.AppData
{
    public class Constants
    {
    }
    public class ComboData
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
    public static class Common
    {
        public static void BindComboBox(ComboBox comboBox, List<ComboData> ListData, bool AddSelectValue=true)
        {
            if(AddSelectValue)
            {
                ListData.Insert(0, new ComboData { Id = "0", Value = "--Select--" });
             
            }
            comboBox.ItemsSource = ListData;
            comboBox.DisplayMemberPath = "Value";
            comboBox.SelectedValuePath = "Id";

            comboBox.SelectedIndex = 0;
        }
    }
}
