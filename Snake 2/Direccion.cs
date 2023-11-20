using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_2
{
    public class Direccion
    {
        // se definen cuatro  las 4 direcciones: Izquierda, Derecha, Arriba y Abajo.
        public readonly static Direccion Left = new Direccion(0, -1);
        public readonly static Direccion Right = new Direccion(0, 1);
        public readonly static Direccion Up = new Direccion(-1, 0);
        public readonly static Direccion Down = new Direccion(1, 0);
        public int DesFilas { get; }
        public int DesCol { get; }

        //constructor
        private Direccion(int desFilas, int desCol)
        {
            DesFilas = desFilas;
            DesCol = desCol;
        }
        // metodo que devuelve la dirección opuesta invirtiendo los cambios en filas y columnas.
        public Direccion Opuesta()
        {
            return new Direccion(-DesFilas, -DesCol);   
        }
        // metodo que verifica si 2 objetos direccion son iguales
        public override bool Equals(object obj)
        {
            return obj is Direccion direccion &&
                   DesFilas == direccion.DesFilas &&
                   DesCol == direccion.DesCol;
        }
        //metodo que genera codigo hash para cada objeto
        public override int GetHashCode()
        {
            return HashCode.Combine(DesFilas, DesCol);
        }
        //compara dos objetos direccion
        public static bool operator ==(Direccion left, Direccion right)
        {
            return EqualityComparer<Direccion>.Default.Equals(left, right);
        }
        //compara 2 objetos direccion
        public static bool operator !=(Direccion left, Direccion right)
        {
            return !(left == right);
        }


    } 
}
