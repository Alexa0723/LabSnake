using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Snake_2
{
    public static class Imagenes
    {
        public readonly static ImageSource Empty = LoadImage("Empty.png");
        public readonly static ImageSource Cuerpo = LoadImage("cuerpo.jpg");
        public readonly static ImageSource Cabeza = LoadImage("cabeza.jpg");
        public readonly static ImageSource Comida = LoadImage("Food.png");
        public readonly static ImageSource CuerpoDead = LoadImage("cuerpoDead.jpg");
        public readonly static ImageSource CabezaDead = LoadImage("cabezaDead.jpg");

        private static ImageSource LoadImage(string filename)
        {
            return new BitmapImage(new Uri($"Cuerpo/{filename}", UriKind.Relative));
        }
    }
}
