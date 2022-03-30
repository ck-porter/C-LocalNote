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
using Windows.UI.Xaml.Controls;

namespace LocalNote.Repositories
{
    public class NotesRepo
    {
        //this class is going to handle all the file saving(or ALL file related commands ie reading)

        private static StorageFolder _notesFolder = ApplicationData.Current.LocalFolder;     

        public NoteModel nm { get; }

        public static DataRepository DataRepository ; 
        public NoteViewModel noteViewModel { get; set; }
               
        public List<string> noteTitles { get; set; }

        public void loadFiles(NoteViewModel noteViewModel) 
        {
            
                                 
            string path = ApplicationData.Current.LocalFolder.Path;

            DirectoryInfo dinfo = new DirectoryInfo(@path);
            FileInfo[] Files = dinfo.GetFiles("*");

            //load in the files already existing
            foreach (FileInfo file in Files)
            {              
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
                //if file already exists, overwrite the content in the file
                string path = ApplicationData.Current.LocalFolder.Path + "\\" + fileName + ".txt";
                await File.WriteAllTextAsync(path, newContent);      
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Some error occured!!");
            }
        }                   

        public async static void SaveNoteToFile(NoteModel selected, String userNote, string newContent)  //pass in the content from the save icon
        {     





            NoteModel nm = new NoteModel();
            NoteViewModel nmViewModel = new NoteViewModel();
            String fileName = userNote + ".txt";

            try
            {
                //create a new file using the name the user entered into the save dialog
                StorageFile notesFile = await _notesFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);            
             
                await FileIO.AppendTextAsync(notesFile, newContent);         
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Some error occured!!");
            }
        }

        public async static void DeleteFile(NoteModel selected, string selectedTitle, NoteViewModel _noteViewModel)
        {
            //grab the path to the file
            string path = ApplicationData.Current.LocalFolder.Path;
            DirectoryInfo dinfo = new DirectoryInfo(@path);
            FileInfo[] Files = dinfo.GetFiles("*");
          

            //load in the files already existing
            foreach (FileInfo file in Files)
            {
                string title = System.IO.Path.GetFileNameWithoutExtension(file.Name);

                if (title == selectedTitle)
                {

                    try
                    {
                        //perform delete
                        File.Delete(path + "\\" + title + ".txt");

                        ContentDialog deletedDialog = new ContentDialog()
                        {
                            Content = "Note was deleted",
                            Title = "Delete Succesful",
                            PrimaryButtonText = "Ok"
                        };
                        await deletedDialog.ShowAsync();
                        _noteViewModel.removeNote();

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error when attempting to delete the file");
                    }
                }
            }
        }
    }
}
