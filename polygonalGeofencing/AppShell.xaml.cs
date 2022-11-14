using polygonalGeofencing.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace polygonalGeofencing
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("entrarModal", typeof(entrarModal));
            Routing.RegisterRoute("cadastroModal", typeof(cadastroModal));          
            Routing.RegisterRoute("AcessoFuncModal", typeof(AcessoFuncModal));          
        }

    }
}
