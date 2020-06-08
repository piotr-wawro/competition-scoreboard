﻿using MySql.Data.MySqlClient;
using ProjektSemestrIV.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSemestrIV.DAL.Repositories
{
    class StageRepository
    {
        #region CRUD
        public static List<Stage> GetAllStages()
        {
            List<Stage> stages = new List<Stage>();
            using (MySqlConnection connection = DatabaseConnection.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM trasa", connection);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    stages.Add(new Stage(reader));
                }
                connection.Close();
            }
            return stages;
        }

        public static Stage GetStageByIdFromDB(uint id)
        {
            string query = $"SELECT * FROM trasa WHERE trasa.id = {id}";
            Stage stage = null;
            using (MySqlConnection connection = DatabaseConnection.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    stage = new Stage(reader);
                connection.Close();
            }
            return stage;
        }

        public static Boolean AddStageToDatabase(Stage stage)
        {
            Boolean executed = false;
            using (MySqlConnection connection = DatabaseConnection.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand($"INSERT INTO trasa (`id_zawody`, `nazwa`, `zasady`) VALUES {stage.ToInsert()}", connection);
                connection.Open();
                if (command.ExecuteNonQuery() == 1) executed = true;
                connection.Close();
            }
            return executed;
        }

        public static bool EditStageInDatabase(Stage stage, UInt32 id)
        {
            Boolean executed = false;
            using (MySqlConnection connection = DatabaseConnection.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand($"UPDATE `trasa` SET `id_zawody` = '{stage.Competition_ID}', `nazwa` = '{stage.Name}', `zasady` = '{stage.Rules}' WHERE (`id` = '{id}');", connection);
                connection.Open();
                if (command.ExecuteNonQuery() == 1) executed = true;
                connection.Close();
            }
            return executed;
        }

        public static bool DeleteStageFromDatabase(UInt32 stageID)
        {
            Boolean executed = false;
            using (MySqlConnection connection = DatabaseConnection.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand($"DELETE FROM trasa WHERE (`id` = '{stageID}')", connection);
                connection.Open();
                if (command.ExecuteNonQuery() == 1) executed = true;
                connection.Close();
            }
            return executed;
        }
        #endregion

        #region Auxiliary queries
        public static uint GetNumOfTargetsOnStageFromDB(uint id)
        {
            string query = $@"SELECT count(tarcza.id) AS numOfTargets FROM trasa
                            INNER JOIN tarcza
                            ON trasa.id = tarcza.trasa_id
                            WHERE trasa.id = {id};";
            uint numOfTargets = 0;
            using (MySqlConnection connection = DatabaseConnection.Instance.Connection)
            {

                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    numOfTargets = (uint)reader.GetInt64("numOfTargets");
                connection.Close();
            }
            return numOfTargets;
        }
        
        public static double GetAverageTimeOnStageByIdFromDB(uint id)
        {
            string query = $@"SELECT avg(przebieg.czas) AS averageTime FROM przebieg
                            INNER JOIN trasa
                            ON trasa.id = przebieg.id_trasa
                            WHERE trasa.id = {id};";
            double averageTime = 0;
            using (MySqlConnection connection = DatabaseConnection.Instance.Connection)
            {

                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    averageTime = reader.GetDouble("averageTime");
                connection.Close();
            }
            return averageTime;
        }
        #endregion
    }
}
