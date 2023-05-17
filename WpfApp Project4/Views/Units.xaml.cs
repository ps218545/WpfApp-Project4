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
using System.Windows.Shapes;

namespace WpfApp_Project4.Views
{
    /// <summary>
    /// Interaction logic for Units.xaml
    /// </summary>
    public partial class Units : Window
    {
        public Units()
        {
            InitializeComponent();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
 //           if (string.IsNullOrEmpty(NewIngredient.Name))
 //           {
 //               MessageBox.Show("Vul naam van het ingrediënt in");
 //               return;
 //           }
 //           if (NewIngredient.UnitId == 0)
 //           {
 //               MessageBox.Show("Selecteer een eenheid.");
 //               return;
 //           }
 //           if (NewIngredient.Price < 0)
 //           {
 //               MessageBox.Show("Wijzig de prijs. Deze mag niet negatief zijn.");
 //               return;
 //           }

//            string dbResult = db.CreateIngredient(NewIngredient);
 //           if (dbResult == LosPollosHermanosDb.OK)
 //           {
 //               NewIngredient = new();
 //               PopulateIngredients();
 //           }
 //           else
 //           {
//                MessageBox.Show(dbResult + serviceDeskBericht);
//            }

        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
 //           if (SelectedIngredient == null)
//            {
//                MessageBox.Show("Selecteer eerst het ingredient dat u wil wijzigen.");
//                return;
 //           }
  //          if (string.IsNullOrEmpty(SelectedIngredient.Name))
  //          {
  //              MessageBox.Show("Vul naam van het ingrediënt in.");
  //              return;
 //           }
  //          if (SelectedIngredient.UnitId == 0)
 //           {
 //               MessageBox.Show("Selecteer een eenheid.");
 //               return;
 //           }
 //           if (SelectedIngredient.Price < 0)
 //           {
 //               MessageBox.Show("Wijzig de prijs. Deze mag niet negatief zijn.");
 //               return;
 //           }

//            string dbResult = db.UpdateIngredient(SelectedIngredient.IngredientId, SelectedIngredient);
//            if (dbResult == LosPollosHermanosDb.OK)
 //           {
  //              PopulateIngredients();
//            }
 //           else
  //          {
 //               MessageBox.Show(dbResult + serviceDeskBericht);
  //          }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btnDel = (Button)sender;
 //           Ingredient ingredient = (Ingredient)btnDel.DataContext;

 //           string dbResult = db.DeleteIngredient(ingredient.IngredientId);
 //           if (dbResult == LosPollosHermanosDb.OK)
 //           {
 //               PopulateIngredients();
 //           }
 //           else
  //          {
//                MessageBox.Show(dbResult + serviceDeskBericht);
   //         }
        }

        private void Mute_Click(object sender, RoutedEventArgs e)
        { }

        private void Selection_Click(object sender, RoutedEventArgs e)
        {
            new Selection().Show();
            this.Close();
        }


    }
}
