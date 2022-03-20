using LocalNote.Models;
using LocalNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LocalNote.Repositories
{
    public class NotesRepo
    {
        //this class is going to handle all the file saving(or ALL file related commands ie reading)

        private static StorageFolder _namesFolder = ApplicationData.Current.LocalFolder;     

        public NoteModel nm { get; }

        public NoteViewModel noteViewModel { get; set; }

        public List<string> noteTitles { get; set; }


        public void loadFiles(NoteViewModel noteViewModel) 
        {

           List<string> noteTitles = new List<string>();
     //      noteViewModel = new NoteViewModel();

           string path = ApplicationData.Current.LocalFolder.Path;

            DirectoryInfo dinfo = new DirectoryInfo(@path);
            FileInfo[] Files = dinfo.GetFiles("*");

            foreach (FileInfo file in Files)
            {
                noteTitles.Add(file.Name);


                string title = file.Name;
                string content  = File.ReadAllText(path+ '\\' +  file.Name);
                string pause = "";

               noteViewModel.loadNotes(title, content);
            }

        }


           

        public async static void SaveNameDaysToFile(NoteModel selected, String userNote, string newContent)  //pass in the content from the save icon
        {
      
            String fileName = userNote + ".txt";
   
            NoteModel nm = new NoteModel();
            NoteViewModel nmViewModel = new NoteViewModel();                 


            try
            {
                StorageFile notesFile = await _namesFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);


                //need a try catch around this

                //if (selected.Content == null)
                //{
                await FileIO.AppendTextAsync(notesFile, newContent);

                //}
                //else {

                /* await FileIO.AppendTextAsync(notesFile, selected.Content); */  //------------------only works if note is selected
                                                                                  //}



            }
            catch (Exception ex)
            {
                Debug.WriteLine("Some error occured!!");
            }
        }
    }
}
