using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_2
{
    public class Juego
    {

        //propiedades que representan el tamaño y la matriz de cuadrados
        public int Filas { get; }
        public int Cols { get; }
        public TamCuadricula[,] Tam { get; }

        //direccion actual, puntaje y estado del juego
        public Direccion Dir { get; private set; }
        public int Score { get; private set; }
        public bool GameOver { get; private set; }

        //listas que almacenan direcciones para cambios y posiciones del snake
        private readonly LinkedList<Direccion> dirCambiar = new LinkedList<Direccion>();
        private readonly LinkedList<Posicion> snakePosiciones = new LinkedList<Posicion>();

        //objeto random para generar posiciones aleatorias
        private readonly Random random = new Random();


        //constructor que inicializa el juego
        public Juego (int filas, int cols)
        {
            Filas = filas;  
            Cols = cols;
            Tam = new TamCuadricula[ filas, cols];
            Dir = Direccion.Right;

            //inicializa el snake y coloca la primera comida
            addSnake(); 
            addFood();
        }

        // metodo privado que agrega la serpiente inicial al juego.
        private void addSnake ()
        {
            int r = Filas / 2;

            for (int c = 1 ; c <= 3; c++) 
            {
                Tam[r, c] = TamCuadricula.Snake;
                snakePosiciones.AddFirst (new Posicion (r, c));   
            }
        }

        // metodo privado que devuelve las posiciones vacías en el juego
        private IEnumerable<Posicion> EmptyPosiciones()
        {
            for (int r = 0; r < Filas; r++)
            {
                for ( int c = 0; c < Cols; c++)
                {
                    if (Tam[r, c ] == TamCuadricula.Empty)
                    {
                        yield return new Posicion (r, c);   
                    }
                }
            }
        }

        // metodo privado que agrega comida aleatoria al juego.
        private void addFood()
        {
            List<Posicion> empty = new List<Posicion>(EmptyPosiciones());

            if (empty.Count == 0)
            {
                return;
            }
            Posicion pos = empty[random.Next(empty.Count)];
            Tam[pos.Fila, pos.Col] = TamCuadricula.Food;
        }

        public Posicion posicionCabeza()
        {
            return snakePosiciones.First.Value;
        }

        //metodo que devuelve posicion de la cola de la serpiente
        public Posicion posicionCola()
        {
            return snakePosiciones.Last.Value;
        }

        public IEnumerable<Posicion> SnakePosiciones()
        {
            return snakePosiciones;
        }

        //metodo privado que agrega la cabeza del snake
        private void añadirCabeza(Posicion pos)
        {
            snakePosiciones.AddFirst(pos);
            Tam[pos.Fila, pos.Col] = TamCuadricula.Snake;
        }

        //metodo privado que elimina la cola del snake
        private void removerCola()
        {
            Posicion cola = snakePosiciones.Last.Value;
            Tam[cola.Fila, cola.Col] = TamCuadricula.Empty;
            snakePosiciones.RemoveLast();
        }

        //metodo que obtiene la ultima direccion de cambio
        private Direccion obteUltiDireccion()
        {
            if (dirCambiar.Count == 0)
            {
                return Dir;
            }
            return dirCambiar.Last.Value;
        }

        //metodo que verifica si se puede cambiar de direccion 
        private bool pCambiarDireccion(Direccion newDir)
        {
            if (dirCambiar.Count == 2)
            {
                return false;
            }
            Direccion ultimaDir = obteUltiDireccion();
            Direccion ultimaDirOpuesta = ultimaDir.Opuesta(); 
            return newDir != ultimaDir && newDir != ultimaDirOpuesta;
        }
    
        //metodo publico que cambia direccion del snake
        public void cambiaDireccion(Direccion dir)
        {
            if ( pCambiarDireccion(dir))
            {
                dirCambiar.AddLast(dir);
            }
            
        }

        //metodo privado que verifica si una posicion esta por fuera de los limites del juego
        private bool fueraTam(Posicion pos)
        {
            return pos.Fila < 0 || pos.Fila >= Filas || pos.Col < 0 || pos.Col >= Cols;
        }

        //metodo que verifica si hay colision en la nueva posicion de la cabeza
        private TamCuadricula choque(Posicion newCabezaPos)
        {
            if (fueraTam(newCabezaPos))
            {
                return TamCuadricula.Outside;
            }

            if (newCabezaPos == posicionCola())
            {
                return TamCuadricula.Empty;
            }

            return Tam[newCabezaPos.Fila, newCabezaPos.Col];
        }

        //metodo que realiza la posicion de la serpiente
        public void movi()
        {
            //cambia la direccion si hay cambios pendientes
            if(dirCambiar.Count > 0)
            {
                Dir = dirCambiar.First.Value;
                dirCambiar.RemoveFirst();
            }

            Posicion newCabezaPos = posicionCabeza().traduc(Dir);
            TamCuadricula hit = choque (newCabezaPos);

            if(hit == TamCuadricula.Outside || hit == TamCuadricula.Snake) 
            {
                GameOver = true;
            }
            else if (hit == TamCuadricula.Empty)
            {
                removerCola();
                añadirCabeza(newCabezaPos);
            }
            else if (hit == TamCuadricula.Food)
            {
                añadirCabeza(newCabezaPos);
                Score++;
                addFood();
            }
        }
    }
}
