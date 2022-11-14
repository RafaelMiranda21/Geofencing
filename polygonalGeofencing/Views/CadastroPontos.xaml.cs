using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using polygonalGeofencing.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using polygonalGeofencing.Models;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;

namespace polygonalGeofencing.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastroPontos : ContentPage
    {
        public CadastroPontos()
        {
            InitializeComponent();
            BindingContext = new CadastroPontosViewModel();
        }

        protected override async void OnAppearing() //ao inicializar a pagina, quando ela for exibida, carregar os pontos cadastrados
        {
            var vm = (CadastroPontosViewModel)BindingContext;

           
            //****************Centralizar no Usuário********************************

            var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));

            var location = await Geolocation.GetLocationAsync(request);

            Position pUsuario = new Position(location.Latitude, location.Longitude);

            MapSpan MapSpan = MapSpan.FromCenterAndRadius(pUsuario, Distance.FromKilometers(.444));
            map.MoveToRegion(MapSpan);
            //**********************************************************************

            //pega os pontos cadastrados no banco
            List<Pontos> model = await App.database.GetPontosArea(Convert.ToInt32(idarea.Text));
            
            if (model.Count > 0) //se retornar algum registro do banco 
            {
                Polygon polygon1 = new Polygon //cria um novo poligono
                {
                    //estilo do poligono, borda e preenchimento
                    StrokeWidth = 8, 
                    StrokeColor = Color.FromHex("#00BFFF"),  
                    FillColor = Color.FromHex("#87CEFA")
                };

                foreach (Pontos element in model.ToList())
                {
                    //para cada ponto cadastrado no banco ele e adicionado no objeto poligono
                    Position p = new Position((double)element.latitude, (double)element.longitude);
                    polygon1.Geopath.Add(p);
                };
                //adiciona o objeto poligono no mapa
                map.MapElements.Add(polygon1);
            }
            else
            {
                //se não tiver registro no banco os elementos são removidos do mapa
                map.MapElements.Clear();

            }

            
        }

        private async void  map_MapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {
            
            var vm = (CadastroPontosViewModel)BindingContext;
            //monta uma string contendo a latitude e longitude
            string posicao = Convert.ToString(e.Position.Latitude.ToString().Replace(",",".") + "," + e.Position.Longitude.ToString().Replace(",", "."));
            //chama a função passando a string contendo a localização como parametro
            vm.cadastrarPonto.Execute(posicao);
            //pega no banco todos os pontos cadastrados naquela área
            List<Pontos> model = await App.database.GetPontosArea(Convert.ToInt32(idarea.Text));
            //cria um novo objeto polígono
            Polygon polygon1 = new Polygon
            {
                StrokeWidth = 8,
                StrokeColor = Color.FromHex("#00BFFF"),
                FillColor = Color.FromHex("#87CEFA")
            };
            //para cada ponto cadastrado 
            foreach (Pontos element in model.ToList())
            {
                //adiciona o ponto cadastrado no polígono
                Position p = new Position((double)element.latitude, (double)element.longitude);
                polygon1.Geopath.Add(p);
            };
            //adiciona o polígono no mapa
            map.MapElements.Add(polygon1);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private async void iconLupa_Clicked(object sender, EventArgs e)
        {
            //recebe o endereço fornecido pelo usuárop
            string endereco = enderecoMapa.Text.Trim();

            Geocoder geoCoder = new Geocoder();

            //pega a latitude e longitude da localização procurada
            IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(endereco);
            Position position = approximateLocations.FirstOrDefault();
            
            //pega a posição
            Position pEndereco = new Position(position.Latitude, position.Longitude);

            //move para a localização inserida pelo usuário
            MapSpan MapSpan = MapSpan.FromCenterAndRadius(pEndereco, Distance.FromKilometers(.444));
            map.MoveToRegion(MapSpan);


        }
    }
}