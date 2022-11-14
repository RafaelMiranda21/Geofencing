using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace polygonalGeofencing.Models
{
    public class Area
    {
        [PrimaryKey,AutoIncrement] //determina que a variavel a baixo será auto increment
        public int idArea { get; set; } //identificador unico da área

        public int idFazenda { get; set; } //identificador da fazenda

        public string nome { get; set; } //nome da área
        public string descricao { get; set; } //informações da área
        

    }
}
