using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Практика_2_3_Game_Circle
{
    public partial class Result_Form : Form
    {
        BindingList<Square> square = new();
        public Result_Form(bool is_close)
        {
            if (is_close)
            {

                InitializeComponent();

            }
        }

        // После закрытия формы данные из бд удаляются
        private void Result_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            DBHelper.GetInstance().DeleteAll();
        }

        private void Result_Form_Load(object sender, EventArgs e)
        {
            var dbh = DBHelper.GetInstance(
                       "localhost",
                       3306,
                       "root",
                       "",
                       "game_circle_2023"
                       );
            square = dbh.GetSquare();

            // Привязываем список к таблице
            dataGridView1.DataSource = square;
            result.Text = Convert.ToString(dbh.Get_result());
        }
    }
}
