using System.ComponentModel;
namespace Практика_2_3_Game_Circle
{
    public partial class Form1 : Form
    {
        // Нажимаем на кнопку, потом на экран, там появляется квардрат(не двигающийся),
        // затем из него вылетают шары с некоторой периодичностью

        bool OnClick = false;
        Painter p;
        bool Is_Close = false; //флаг для второй формы

        BindingList<Square> square = new();
        public Form1()
        {
            InitializeComponent();
            p = new Painter(mainPanel.CreateGraphics());
            timer1.Interval = 300;
        }

        private void button_Click(object sender, EventArgs e)
        {
            OnClick = true;
        }

        private void mainPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (OnClick)
            {
                p.Add_Rect(e);
                timer1.Enabled = true;
                timer1.Start();
            }
            OnClick = false;
            p.Show();
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            var dbh = DBHelper.GetInstance(
                      "localhost",
                      3306,
                      "root",
                      "",
                      "game_circle_2023"
                      );
            square = dbh.GetSquare();

        }

        // Событие происходящее по истечениее установленного времени
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                p.CirclePaint(p.rec[^1]);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Is_Close = true;
            Result_Form new_form = new Result_Form(Is_Close);
            // Если все супер с новой формой, то показываем ее
            if (new_form.ShowDialog() == DialogResult.OK)
            {
                new_form.Show();
            }
        }
    }
}