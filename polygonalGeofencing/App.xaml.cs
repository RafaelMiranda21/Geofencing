using polygonalGeofencing.Helpers;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using polygonalGeofencing.Views;
namespace polygonalGeofencing
{
    public partial class App : Application
    {

        static SQLiteDataBaseHelper dataBase;

        public static SQLiteDataBaseHelper database
        {
            get
            {
                if (dataBase == null) //se não foi criado o banco
                {
                    //irá criar o arquivo onde é armazenado os dados da aplicação
                    dataBase = new SQLiteDataBaseHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XamAppGeo.db3"));
                }

                return dataBase; //retorna o banco
            }
        }

        public App() //Construtor
        {
            InitializeComponent(); //Componente de inicialização
            MainPage = new AppShell(); //a página raiz da aplicação recebe a tela a ser exibida
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
