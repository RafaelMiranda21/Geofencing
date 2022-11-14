using polygonalGeofencing.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace polygonalGeofencing.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAreaModal : INotifyPropertyChanged
    {
        public EditAreaModal()
        {
            InitializeComponent();
            BindingContext = new EditarAreaViewModel();
        }


        protected override void OnAppearing() //ao inicializar a pagina, quando ela for exibida
        {
            var vm = (EditarAreaViewModel)BindingContext;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
}