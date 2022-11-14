using polygonalGeofencing.Models;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
//using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;


namespace polygonalGeofencing.Views
{
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mapa : ContentPage
    {
        int idLogado = 0;
        string notify = "false|-1";
        bool changedMap = false;
        double latitude = 0;
        double longitude = 0;
 
        public Mapa()
        {
            InitializeComponent();
            //LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationActionTapped;


        }


        //private void OnNotificationActionTapped(NotificationEventArgs e)
        //{
        //    DisplayAlert(e.Request.Title, e.Request.Description,"OK");
        //}


        protected override async void OnAppearing() //ao inicializar a pagina, quando ela for exibida
        {
            //limpa todos elementos do mapa
            map.MapElements.Clear();

            //****************Centralizar no Usuário********************************

            var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));

            var location = await Geolocation.GetLocationAsync(request);

            Position pUsuario = new Position(location.Latitude, location.Longitude);

            MapSpan MapSpan = MapSpan.FromCenterAndRadius(pUsuario, Distance.FromKilometers(.444));
           
            map.MoveToRegion(MapSpan);

            //**********************************************************************

            //pega o usuário logado
            List<Logado> logado = await App.database.GetLogado();

            //pega as informações do usuário
            Usuario usuario = await App.database.GetByIdUsuario(logado.ToList()[0].idUsuario);

            //instancia a variavel que irá receber as areas
            List<Area> quantArea = null;

            //verifica se o usuário e o dono
            if (usuario.idDono != usuario.idUsuario)
            {
                //pega as fazendas que o usuário tem acesso
                List<Fazenda> fazendas = await App.database.GetFazendaFuncionario(usuario.idUsuario, usuario.idDono);

                string str = "";

                //colocar na variavel str todas as fazendas em que o usuário tem acesso
                foreach (Fazenda fazenda in fazendas.ToList())
                {
                    str = fazenda.idFazenda + ",";
                }

                if (str != "")
                {
                    //pega as áreas
                    quantArea = await App.database.GetAreasFazenda(str.Substring(0, str.Length - 1)); //Substring para retirar a ultima virgula 
                    
                }
            }
            else
            {
                //pega todas as áreas
                quantArea = await App.database.GetAreasUsuario(usuario.idDono);

            }

               
            //se retornou algo do banco
            if (quantArea.ToList().Count > 0)
            {
                //chama a função disparaMensagem
                disparaMensagem();
                //para cada área será criado um polígono
                foreach (Area quant in quantArea.ToList())
                {
                    List<Pontos> model = await App.database.GetPontosArea(quant.idArea);
                    if(model.ToList().Count > 0)
                    {
                        Polygon polygon1 = new Polygon
                        {
                            StrokeWidth = 8,
                            StrokeColor = Color.FromHex("#00BFFF"),
                            FillColor = Color.FromHex("#87CEFA")
                        };

                        Pin pin = new Pin
                        {
                            Label = quant.nome,
                            Address = quant.descricao,
                            Type = PinType.Place,
                        };


                        foreach (Pontos element1 in model.ToList())
                        {
                            Position p = new Position((double)element1.latitude, (double)element1.longitude);
                            polygon1.Geopath.Add(p);
                            latitude = (double)element1.latitude;
                            longitude = (double)element1.longitude;
                        };

                        pin.Position = new Position(latitude,longitude);
                        map.Pins.Add(pin);

                        map.MapElements.Add(polygon1);
                    }        
                };
            }
        }



       




        public static bool intoPolygon(Point[] poly, Point pointUsuario)
        {
           
            //calculo para ver se o usuário está dentro da área
            var coef = poly.Skip(1).Select((p, i) =>
                                            (pointUsuario.Y - poly[i].Y) * (p.X - poly[i].X)
                                          - (pointUsuario.X - poly[i].X) * (p.Y - poly[i].Y))
                                    .ToList();

            if (coef.Any(p => p == 0))
                return true;

            for (int i = 1; i < coef.Count(); i++)
            {
                if (coef[i] * coef[i - 1] < 0)
                    return false;
            }
            return true;
        }


        public void disparaMensagem()
        {
            //timer para que de tempos em tempos verifcar se o usuário entrou em alguma área
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                changedMap = true;
                varrerArea();
                return true;
            });
        }
        




        public async void varrerArea()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));

                var location = await Geolocation.GetLocationAsync(request);

                Position pUsuario = new Position(location.Latitude, location.Longitude);

                MapSpan MapSpan = MapSpan.FromCenterAndRadius(pUsuario, Distance.FromKilometers(.444));

                if (changedMap)
                {
                    //****************Centralizar no Usuário********************************    
                      map.MoveToRegion(MapSpan);
                    //**********************************************************************
                }



                if (notify.Split('|')[0] == "true" && Int32.Parse(notify.Split('|')[1]) > 0) //se o Usuário já esta dentro de uma área
                    {
                    //pega todos os pontos para ver se o usuário ainda esta dentro da área
                        List<Pontos> model = await App.database.GetPontosArea(Int32.Parse(notify.Split('|')[1]));
                        Point[] pts = new Point[model.ToList().Count];
                        int i = 0;
                        int idArea = 0;
                        foreach (Pontos element1 in model.ToList())
                        {
                        
                            pts[i] = new Point { X = (double)element1.latitude, Y = (double)element1.longitude };
                            i++;
                            idArea = element1.idArea;
                        };
                        Point posUsu = new Point { X = location.Latitude, Y = location.Longitude };

                        bool dentro = intoPolygon(pts, posUsu);

                    //se o usuário saiu da área notify reseta sua informação
                    if (!dentro)
                    {
                        notify = "false|-1";
                    }
                }
                    else
                    {
                    //pega o usuário logado
                    List<Logado> logado = await App.database.GetLogado();

                    //pega as informações do usuário
                    Usuario usuario = await App.database.GetByIdUsuario(logado.ToList()[0].idUsuario);

                    //instancia a variavel que irá receber as areas
                    List<Area> qtArea = null;

                    //verifica se o usuário e o dono
                    if (usuario.idDono != usuario.idUsuario)
                    {
                        //pega as fazendas que o usuário tem acesso
                        List<Fazenda> fazendas = await App.database.GetFazendaFuncionario(usuario.idUsuario, usuario.idDono);

                        string str = "";

                        //colocar na variavel str todas as fazendas em que o usuário tem acesso
                        foreach (Fazenda fazenda in fazendas.ToList())
                        {
                            str = fazenda.idFazenda + ",";
                        }

                        if (str != "")
                        {
                            //pega as áreas
                            qtArea = await App.database.GetAreasFazenda(str.Substring(0, str.Length - 1)); //Substring para retirar a ultima virgula 

                        }
                    }
                    else
                    {
                        //pega todas as áreas
                        qtArea = await App.database.GetAreasUsuario(usuario.idDono);

                    }

                    if (qtArea.ToList().Count > 0)
                        {
                            foreach (Area quant in qtArea.ToList())
                            {
                                List<Pontos> model = await App.database.GetPontosArea(quant.idArea);

                                if (model.Count > 3) //so verificar se esta dentro quando tiver mais de 3 pontos cadastrados
                                {
                                    Point[] pts = new Point[model.ToList().Count];
                                    int i = 0;
                                    int idArea = 0;
                                    foreach (Pontos element1 in model.ToList())
                                    {
                                        pts[i] = new Point { X = (double)element1.latitude, Y = (double)element1.longitude };
                                        i++;
                                        idArea = element1.idArea;
                                    };
                                    Point posUsu = new Point { X = location.Latitude, Y = location.Longitude };

                                    bool dentro = intoPolygon(pts, posUsu);
                                    
                                    if (dentro)// se o usuário estiver dentro da área e disparado um alerta
                                    {

                                        notify = "true|" + idArea;
                                        Area getArea = await App.database.GetByIdArea(idArea);
                                        var notification = new NotificationRequest
                                        {
                                            BadgeNumber = 1,
                                            NotificationId = 1337,
                                            Title = "Entrou na Area: " + getArea.nome,
                                            Description = getArea.descricao,
                                            ReturningData = "Dummy data", //Retorna dados quando toca na notificação
                                        };
                                        await LocalNotificationCenter.Current.Show(notification);
                                        await DisplayAlert("Entrou na Area: " + getArea.nome, getArea.descricao, "OK");

                                    }
                                }

                                
                            }
                        }
                    }
   
              }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Console.WriteLine(fnsEx.InnerException.Message);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Console.WriteLine(fneEx);
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Console.WriteLine(pEx);
            }
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine(ex);
            }
        }




        private void map_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            changedMap = false;
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