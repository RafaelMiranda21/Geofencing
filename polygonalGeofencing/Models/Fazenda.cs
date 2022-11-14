using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace polygonalGeofencing.Models
{
    public class Fazenda
    {
        [PrimaryKey, AutoIncrement] //determina que a variavel a baixo será auto increment
        public int idFazenda { get; set; } //identificador unico da fazenda

        public int idUsuario { get; set; } //identificador unico do Usuario

        public string nome { get; set; } //nome da fazenda 

    }
}
