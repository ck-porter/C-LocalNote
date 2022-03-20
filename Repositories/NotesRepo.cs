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

            string path = ApplicationData.Current.LocalFolder.Path;

            DirectoryInfo dinfo = new DirectoryInfo(@path);
            FileInfo[] Files = dinfo.GetFiles("*");

            //load in the files already existing
            foreach (FileInfo file in Files)
            {
                noteTitles.Add(file.Name);

                //grab the file name without the extension 
                string title = System.IO.Path.GetFileNameWithoutExtension(file.Name);          
                string content  = File.ReadAllText(path+ '\\' +  file.Name);          

                //load in the existing notes with method from nvm
                noteViewModel.loadNotes(title, content);
            }

        }


        public async static void SaveExisting(NoteModel selected, string fileName,  string newContent) 
        {

            try
            {

                string path = ApplicationData.Current.LocalFolder.Path + "\\" + fileName + ".txt";
                await File.WriteAllTextAsync(path, newContent);
                
                              

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Some error occured!!");
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

             
                await FileIO.AppendTextAsync(notesFile, newContent);

             



            }
            catch (Exception ex)
            {
                Debug.WriteLine("Some error occured!!");
            }
        }
    }
}
