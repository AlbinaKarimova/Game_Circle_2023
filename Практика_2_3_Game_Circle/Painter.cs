namespace Практика_2_3_Game_Circle
{
    public class Painter
    {
        private List<Square> rects = new List<Square>();
        private List<Animator> animators = new List<Animator>();
        private static List<Circle> circles = new List<Circle>(); // Список всех созданных шаров
        private Graphics g;
        private BufferedGraphics bg;
        private object locker = new object();
        private Size ContSize;
        private Pen pen = new Pen(Color.DarkGray);
        
        public List<Square> rec
        {
            get
            {
                return rects;
            }
            set
            {
                rects = value;
            }
        }

        public Size ContainerSize
        {
            get { return ContSize; }
            set { ContSize = value; }
        }
        public Graphics MainGraphics
        {
            get { return g; }
            set 
            {
                g = value;
                ContainerSize = g.VisibleClipBounds.Size.ToSize();
                bg = BufferedGraphicsManager.Current.Allocate(g, new Rectangle(new Point(0, 0), ContainerSize));
            }
        }
        public Painter(Graphics g)
        {
            this.MainGraphics = g;
            //var dbh = DBHelper.GetInstance();
        }

        public bool IsAlive(Animator animator)
        {
            int x = animator._circle.X;
            int y = animator._circle.Y;
            int r = animator._circle.Radius;
            if(x + r < 0 || x - r > ContainerSize.Width)
            {
                animators.Remove(animator);
                return false;
            }
            if (y + r < 0 || y - r > ContainerSize.Height)
            {
                animators.Remove(animator);
                return false;
            }
            else return true;
        }
        public void Add_Rect(MouseEventArgs e)
        {
            Square r = new Square(e.X, e.Y);
            rects.Add(r);
            if (rects.Count > 1)
            {
                r.Id = rects[^2].Id + 1;
            }
            else
                r.Id = 1;
            CirclePaint(r);
            DBHelper.GetInstance().InsertNew(r);
        }

        public void CirclePaint(Square r)
        {
            Circle c = new Circle(r.X, r.Y);
            circles.Add(c); 
            c.color = r.color;
            Animator anim = new Animator(c, ContainerSize);
            animators.Add(anim);
            anim.Go(circles, rects);
            DBHelper.GetInstance().Update_values(r);
        }

        public void Show()
        {
            Thread t = new Thread(new ThreadStart(Moving));
            t.Start();
        }

        public void Moving()
        {
            // Здесь идет главный поток, нужно було чтобы созданный поток не заходил сюда
            while (true)
            {
                Draw();
                int count = animators.Count;
                //animators.Remove(Is_circle(circles));
                if (count > 0)
                {
                        foreach (var a in animators.ToList())
                        {
                            a.Move();

                            //наверное здесь надо добавлять новые шарики к квадратам (вызывать CirclePaint)
                            if (IsAlive(a) == false)
                            {
                                try
                                {
                                    Thread.CurrentThread.Abort();
                                    Thread.Sleep(3000);
                                }
                                catch { }
                                //Thread.CurrentThread.Interrupt();
                            }
                        }
                }
            }
        }

        // Вся отрисовка здесь
        public void Draw()
        {
            lock (locker)
            {
                bg.Graphics.Clear(Color.White);
                
                foreach (var anim in animators.ToList())
                {
                    int r = anim._circle.Radius;
                    bg.Graphics.FillEllipse(new SolidBrush(anim._circle.color), anim._circle.X - r, anim._circle.Y - r, 2 * r, 2 * r);
                }
                foreach (var rec in rects)
                {
                    bg.Graphics.FillRectangle(new SolidBrush(rec.color), rec.X - rec.Side / 2, rec.Y - rec.Side / 2, rec.Side, rec.Side);
                }
                bg.Render(g);
                //Thread.Sleep(30);
            }
        }
    }
}
