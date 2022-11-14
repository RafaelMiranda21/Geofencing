using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace polygonalGeofencing.Models
{
    public class Pontos
    {
        [PrimaryKey, AutoIncrement] //determina que a variavel a baixo será auto increment
        public int idPonto { get; set; } //identificador de cada ponto

        public int idArea { get; set; } //identificador da area

        public double? latitude { get; set; } //latitude do ponto

        public double? longitude { get; set; } //longitude do ponto
    }
}
