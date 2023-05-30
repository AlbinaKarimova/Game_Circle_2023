using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.ComponentModel;

namespace Практика_2_3_Game_Circle
{
    public class DBHelper
    {
        private static MySqlConnection? conn = null;
        private DBHelper(
            String host,
            int port,
            String user,
            String password,
            String database
            )
        {
            // Строка подключения
            var connStr = $"Server={host};port={port};database={database};User Id={user};password={password}";
            conn = new MySqlConnection(connStr);
            // Открываем строку подключения
            conn?.Open();
        }

        private static DBHelper instance = null;
        public static DBHelper GetInstance(
                        String host = "localhost",
                        int port = 0,
                        String user = "root",
                        String password = "",
                        String database = ""
                        )
        {
            if (instance == null)
            {
                instance = new DBHelper(host, port, user, password, database);
            }
            return instance;
        }

        public BindingList<Square> GetSquare()
        {
            BindingList<Square> sq = new BindingList<Square>();
            var queryStr = "SELECT * FROM squares";

            // Создание команды
            var cmd = conn?.CreateCommand();
            cmd.CommandText = queryStr;

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        sq.Add(new Square
                        {
                            // nameof дает название того поля,
                            // который указан в параметре (возвращет строки)
                            Id = reader.GetInt32(nameof(Square.Id)),
                            Score = reader.GetInt32(nameof(Square.Score))
                        });
                    }
                }
            }
            return sq;
        }

        public string Get_result()
        {
            BindingList<Square> sq = new BindingList<Square>();
            try
            {
                var cmd = conn?.CreateCommand();
                var quarestr = $"SELECT `score`, `id` FROM `squares` WHERE `score`=(SELECT MAX(`score`) FROM `squares`)";
                cmd.CommandText = quarestr;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            sq.Add(new Square
                            {
                                Id = reader.GetInt32(nameof(Square.Id)),
                                Score = reader.GetInt32(nameof(Square.Score))
                            });
                        }
                    }
                    else return "0";
                }
                //cmd.ExecuteNonQuery();
                return $"Победил шарик № {sq[0].Id} с {sq[0].Score} очками";
            }
            catch { return "0"; }
        }

        public void InsertNew(Square new_square)
        {
            var cmd = conn.CreateCommand();
            try
            {
                cmd.CommandText = "INSERT INTO `squares` (id, score) VALUES (@id, @score);";
                cmd.Parameters.Add(new MySqlParameter("@id", new_square.Id));
                cmd.Parameters.Add(new MySqlParameter("@score", new_square.Score));
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

        public void Update_values(Square s)
        {
            var cmd = conn.CreateCommand();
            try
            {
                cmd.CommandText = $"UPDATE `squares` SET `score` = @score WHERE `squares`.`id` = {s.Id}";
                cmd.Parameters.Add(new MySqlParameter("@score", s.Score));
                cmd.ExecuteNonQuery();
            }
            catch { }
        }



        public void DeleteAll()
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM `squares`";
            cmd.ExecuteNonQuery();
        }
    }
}
