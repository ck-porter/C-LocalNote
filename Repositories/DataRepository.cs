using LocalNote.ViewModels;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;


namespace LocalNote.Repositories
{
    public class DataRepository  
    {

        public NoteViewModel noteViewModel { get; set; }


        public async void InitializeDatabase()
        {      

            //create db file in windows storage
            await ApplicationData.Current.LocalFolder.CreateFileAsync("note.db", CreationCollisionOption.OpenIfExists);

            //get full path
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "note.db");


            //establish connection to database
            using (SqliteConnection conn =
                     new SqliteConnection($"Filename={dbpath}"))
            {
                //open connection
                conn.Open();

                //define SQL command Create Table
                String tableComman = "CREATE TABLE IF NOT EXISTS NoteTable(NoteId INTEGER PRIMARY KEY," +
                    "Title nvarchar(100) NOT NULL, Content nvarchar(100) NOT NULL);";

                //create runnable command object
                SqliteCommand cmd = new SqliteCommand(tableComman, conn);

                //execute the sql command
                cmd.ExecuteReader();
            }
        }

        public static void SaveNewNote(string title, string content)

        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "note.db");
            using (SqliteConnection db =
            new SqliteConnection($"Filename={dbpath}"))

            {

                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();

                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO NoteTable (Title, Content) VALUES (@Title, @Content);";
                insertCommand.Parameters.AddWithValue("@Title", title);
                insertCommand.Parameters.AddWithValue("@Content", content);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        public static void UpdateNote(string title, string content)

        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "note.db");
            using (SqliteConnection db =
            new SqliteConnection($"Filename={dbpath}"))

            {
                db.Open();

                SqliteCommand updateCommand = new SqliteCommand();

                updateCommand.Connection = db;

                updateCommand.CommandText = "UPDATE NoteTable SET Content = @Content WHERE Title = @Title;";
                updateCommand.Parameters.AddWithValue("@Title", title);
                updateCommand.Parameters.AddWithValue("@Content", content);

                updateCommand.ExecuteReader();

                db.Close();
            }
        }

        public void LoadNotes(NoteViewModel noteViewModel)
        {
            List<string> TitleList = new List<string>();
            List<string> ContentList = new List<string>();


            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "note.db");

            //get all the titles
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand selectCommand =
                new SqliteCommand("SELECT Title FROM NoteTable;", db);



                SqliteDataReader query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    TitleList.Add(query.GetString(0));
                }

                query.Close();




                SqliteCommand selectCommand2 =
                   new SqliteCommand("SELECT Content FROM NoteTable;", db);

                SqliteDataReader query2 = selectCommand2.ExecuteReader();
                while (query2.Read())
                {
                    ContentList.Add(query2.GetString(0));
                }
            }

            for (var i = 0; i < TitleList.Count(); i++) 
            {
                var noteTitle = TitleList[i];
                var noteContent = ContentList[i];
                noteViewModel.loadNotes(noteTitle, noteContent);
            }
           
        }

        public bool CheckIfExisting(string search)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "note.db");
            using (SqliteConnection db =
            new SqliteConnection($"Filename={dbpath}"))

            {

                db.Open();

               SqliteCommand selectCommand =
               new SqliteCommand("SELECT COUNT(*) FROM NoteTable WHERE Title = @search;", db);
               selectCommand.Parameters.AddWithValue("@search", search);
               int count = Convert.ToInt32(selectCommand.ExecuteScalar());

                if (count == 0)
                {
                    return false;
                
                }else
                    return true;

                //selectCommand.ExecuteReader();

               //db.Close();
            }


        }
    }
}
