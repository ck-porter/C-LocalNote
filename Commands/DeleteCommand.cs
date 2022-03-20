using LocalNote.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace LocalNote.Commands
{
    public class DeleteCommand : ICommand
    {

        //pass in the reference to the object
        private ViewModels.NoteViewModel _noteViewModel;
        private NotesRepo _notesRepo;

        public DeleteCommand(ViewModels.NoteViewModel noteViewModel)
        {
            this._noteViewModel = noteViewModel;
            this._notesRepo = new NotesRepo();
        }


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _noteViewModel != null;
        }

        public async void Execute(object parameter)
        {
            string updateContents = _noteViewModel.getContents();            

            //instantiate new instance of our dialog box
            DeleteNoteDialog1 deleteDialog = new DeleteNoteDialog1();     
          
            ContentDialogResult result = await deleteDialog.ShowAsync();   

            string newContent = _noteViewModel.getContents();


                if (result == ContentDialogResult.Primary) 
                {
                        //check to make sure there isn't a file with that name already
                        string path = ApplicationData.Current.LocalFolder.Path;

                        DirectoryInfo dinfo = new DirectoryInfo(@path);
                        FileInfo[] Files = dinfo.GetFiles("*");

                        //grab the title of the selected note, could be in try catch block
                        string selectedTitle = _noteViewModel.getTitle();

                        //load in the files already existing
                        foreach (FileInfo file in Files)
                        {
                            string title = System.IO.Path.GetFileNameWithoutExtension(file.Name);                                              
                        
                            if (title == selectedTitle)
                            {

                            //perform delete



                            ContentDialog deletedDialog = new ContentDialog()
                            {


                                Content = "Note was deleted",
                                Title = "Delete Succesful",
                                PrimaryButtonText = "Ok"
                            };

                                              
                             await deletedDialog.ShowAsync();                                   
                                                    
                        }              
                                                      
                }
            }
        }
    }
}
