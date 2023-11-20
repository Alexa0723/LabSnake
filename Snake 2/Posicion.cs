using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_2
{
    public class Posicion
    {
        public int Fila { get; }
        public int Col { get; }
        
        //constructor
        public Posicion(int fila, int col)
        {
            Fila = fila;
            Col = col;  
        }
        // metodo que traduce la posición según una dirección dada
        // devuelve una nueva posición desplazada según la dirección.
        public Posicion traduc(Direccion dir)
        {
            return new Posicion(Fila + dir.DesFilas, Col + dir.DesCol);
        }

        // metodo que verifica si dos objetos Posicion son iguales basandose en sus propiedades.

        public override bool Equals(object obj)
        {
            return obj is Posicion posicion &&
                   Fila == posicion.Fila &&
                   Col == posicion.Col;
        }

        // Método que genera un código hash único para cada objeto Posicion.

        public override int GetHashCode()
        {
            return HashCode.Combine(Fila, Col);
        }

        public static bool operator ==(Posicion left, Posicion right)
        {
            return EqualityComparer<Posicion>.Default.Equals(left, right);
        }

        public static bool operator !=(Posicion left, Posicion right)
        {
            return !(left == right);
        }
    }
}
