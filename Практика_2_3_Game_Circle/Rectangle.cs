using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Практика_2_3_Game_Circle
{
    public class Square : INotifyPropertyChanged
    {
        private int x, y;
        private int side;
        public Color color;
        private Random r = new();
        private List<Circle> _circles = new List<Circle>();

        private int id;
        [DisplayName("Номер квадрата")]
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private int score;
        [DisplayName("Очки квадрата")]
        public int Score
        {
            get => score;
            set
            {
                score = value;
                OnPropertyChanged(nameof(Score));
            }
        }

        public Square()
        {
        }

        public List<Circle> circles 
        { 
            get { return _circles; }
            set { _circles = value; }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public int Side
        {
            get { return side; }
            set { side = value; }
        }
        public Square(int x, int y, int side = 40)
        {
            this.x = x;
            this.y = y;
            this.side = side;
            color = Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));
        }
    }
}
