using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //diccionario que asocia los tipos de cuadrícula con las imágenes correspondientes
        private readonly Dictionary<TamCuadricula, ImageSource> tamCuadriAImagen = new()
        {
            {TamCuadricula.Empty, Imagenes.Empty},
            {TamCuadricula.Snake, Imagenes.Cuerpo},
            {TamCuadricula.Food, Imagenes.Comida},
        };

        // diccionario que asigna direcciones a rotaciones en grados para la serpiente
        private readonly Dictionary<Direccion, int> dirARotacion = new()
        {
            { Direccion.Up, 0 },
            { Direccion.Right, 90 },
            { Direccion.Down, 180 },
            { Direccion.Left, 270 },
        };

        //numero de filas y columnas en el juego
        private readonly int filas = 15, cols = 15;
        //matriz que representa el tablero
        private readonly Image[,] tamImagenes;
        //instancia del juego
        private Juego juego;
        //indica si el juego se está ejecutando
        private bool gameRunning;


        //constructor de ventana principal
        public MainWindow()
        {
            InitializeComponent();
            tamImagenes = confiTam();
            juego = new Juego(filas, cols);
        }


        //metodo que controla el ciclo de juego
        private async Task compileJuego()
        {
            dibujar();
            await mostrarCueRegresiva();
            Overlay.Visibility = Visibility.Hidden;
            await bucle();
            await mostrarGameOver();
            juego = new Juego(filas, cols);
        }


        // manejador de eventos para la darle click a las teclas antes de que se muestre en la ventana
        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }
            // inicia el juego cuando se pulsa cualquier tecla
            if (!gameRunning)
            {
                gameRunning = true;
                await compileJuego();
                gameRunning = false;
            }
        }


        // manejador de eventos para la pulsación de teclas durante el juego
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (juego.GameOver)
            {
                return;
            }

        //cambia direccion de la serpiente segun la tecla
            switch (e.Key)
            {
                case Key.Left:
                    juego.cambiaDireccion(Direccion.Left);
                    break;
                case Key.Right:
                    juego.cambiaDireccion(Direccion.Right);
                    break;
                case Key.Up:
                    juego.cambiaDireccion(Direccion.Up);
                    break;
                case Key.Down:
                    juego.cambiaDireccion(Direccion.Down);
                    break;
            }
        }

        //metodo que controla el buscle principal del juego
        private async Task bucle()
        {
            while (!juego.GameOver)
            {
                await Task.Delay(160);
                juego.movi();
                dibujar();
            }
        }

      //metodo que configura la matriz de imágenes que representa el tablero
        private Image[,] confiTam()
        {
            Image[,] imagenes = new Image[filas, cols];
            GameGrid.Rows = filas;
            GameGrid.Columns = cols;

            for (int r = 0; r < filas; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Image imagen = new Image
                    {
                        Source = Imagenes.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    };

                    imagenes[r, c] = imagen;
                    GameGrid.Children.Add(imagen);
                }
            }
            return imagenes;
        }


        // método que actualiza la representación visual del tablero
        private void dibujar()
        {
            dibujarCuad();
            dibujarCabezaSnake();
            ScoreText.Text = $"SCORE {juego.Score}";
        }


        private void Window_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {

        }

        //metodo que actualiza la representacion de los cuadrados del tabalero
        private void dibujarCuad()
        {
            for (int r = 0; r < filas; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    TamCuadricula tamCuad = juego.Tam[r, c];
                    tamImagenes[r, c].Source = tamCuadriAImagen[tamCuad];
                    tamImagenes[r, c].RenderTransform = Transform.Identity;
                }
            }
        }


        //metodo que actualiza la representacion de la cabeza de la serpiente
        private void dibujarCabezaSnake()
        {
            Posicion cabezaPos = juego.posicionCabeza();
            Image imagen = tamImagenes[cabezaPos.Fila, cabezaPos.Col];
            imagen.Source = Imagenes.Cabeza;

            int rotacion = dirARotacion[juego.Dir];
            imagen.RenderTransform = new RotateTransform(rotacion);
        }
        // metodo que dibuja visualmente la serpiente después de que el juego ha terminado.
        private async Task dibujarDeadSanke()
        {
            List<Posicion> posicions = new List<Posicion>(juego.SnakePosiciones());

            for (int i = 0; i < posicions.Count; i++)
            {
                Posicion pos = posicions[i];
                ImageSource source = (i == 0) ? Imagenes.CabezaDead : Imagenes.CuerpoDead;
                tamImagenes[pos.Fila, pos.Col].Source = source;
                await Task.Delay(50);
            }
        }


        // metodo que muestra visualmente una cuenta regresiva antes de iniciar el juego
        private async Task mostrarCueRegresiva()
        {
            for (int i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }
        }

        private async Task mostrarGameOver()
        {
            await dibujarDeadSanke();
            await Task.Delay(1000);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "Presiona cualquier tecla para iniciar";
        }
    }
}
