using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Практика_2_3_Game_Circle
{
    public class Animator
    {
        public Circle _circle;
        private int dx, dy;// направление движения
        private Size containerSize;
        Thread? t = null;
        public Animator(Circle c, Size containerSize)
        {
            this._circle = c;
            this.containerSize = containerSize;
            //var dbh = DBHelper.GetInstance();
        }
        public void Move()
        {
            _circle.X += dx;
            _circle.Y += dy;
        }
        public bool IsAlive()
        {
            // Лево-право
            if (_circle.X - _circle.Radius < 0 || _circle.X + _circle.Radius > containerSize.Width)
            {
                return false;
            }
            // Верх-низ
            if (_circle.Y - _circle.Radius < 0 || _circle.Y + _circle.Radius > containerSize.Height)
            {
                return false;
            }

            return true;
        }
        public void Go(List<Circle> circles, List<Square> squares)
        {
            Random rnd = new Random();
            int dx, dy;
            do
            {
                dx = rnd.Next(-4, 4);
                dy = rnd.Next(-4, 4);
            } while (dx == 0 && dy == 0);

            t = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(35);
                    _circle.Move(dx, dy);
                    Is_circle(circles, squares);
                }
            }
            );
            t.IsBackground = true;
            t.Start();
        }

        private object locker = new object();
        // СТОЛКНОВЕНИЕ ШАРИКОВ
        public void Is_circle(List<Circle> cir, List<Square> squares)
        {
            lock (locker)
            {
                if (cir.Count >= 2)
                {
                    int i = 0;
                    double d;

                    while (!_circle.Equals(cir[i]) && _circle.color != cir[i].color)
                    {
                        // Проверка для выхода из цикла
                        i++;
                        int Dx = _circle.X - cir[i].X;
                        int Dy = _circle.Y - cir[i].Y;
                        d = Math.Sqrt(Dx * Dx + Dy * Dy); // Гипотенуза
                        if (d == 0) d = 0.01;

                        // Произошло ли столкновение шаров
                        if (d < _circle.Radius + cir[i].Radius)
                        {
                            // При стколкновении шарик окрашивает столкнувшийся шарик в свой цвет
                            cir[i].color = _circle.color;
                            for (int j = 0; j < squares.Count; j++)
                            {
                                if (_circle.color == squares[j].color)
                                {
                                    squares[j].Score++;
                                    //DBHelper.GetInstance().Update_values(squares[j]);
                                    break;
                                }
                            }
                            // Нужно еще удалить рисунок шарика, ну не удаляется что поделать не получается:((
                        }
                    }
                }
            }
        }
    }
}
