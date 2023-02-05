using Autodesk.Revit.UI;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RevitAPI_W1_MadeButton
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(ExternalCommandData commandData)
        {
            InitializeComponent();
            MainViewViewModel vm = new MainViewViewModel(commandData);
            vm.HideRequest += (s, e) => this.Hide(); //Указываем что эвент HideRequest отвечает за скрытие окна
            vm.ShowRequest += (s, e) => this.Show(); //Указываем что эвент ShowRequest отвечает за скрытие окна

            DataContext = vm; //свойство направляющее на команды
        }
    }
}
