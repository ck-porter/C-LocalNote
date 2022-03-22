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
    public class SaveCommand : ICommand
    {

        //pass in the reference to the object
        private ViewModels.NoteViewModel _noteViewModel;
        private NotesRepo _notesRepo;

        public SaveCommand(ViewModels.NoteViewModel noteViewModel)
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

            //if the user loads in a selected file and edits it, do not perform a full save with title box
            if (_noteViewModel.SelectedNote != null)
            {
                string fileName = _noteViewModel.getTitle();
                Repositories.NotesRepo.SaveExisting(_noteViewModel.SelectedNote, fileName, updateContents);

                //display the saved note message
                ContentDialog savedDialog = new ContentDialog()
                {

                    Content = "Names saved successfully to file",
                    Title = "Save Succesful",
                    PrimaryButtonText = "Ok"
                };
                await savedDialog.ShowAsync();

                //reload the list
                _noteViewModel.refreshNotes(updateContents);            
            }

            else { 

            //instantiate new instance of our dialog box
            SaveNoteDialog saveDialog = new SaveNoteDialog();
           
            ContentDialogResult result = await saveDialog.ShowAsync();  

            string newContent = _noteViewModel.getContents();

                if (result == ContentDialogResult.Primary) 
                {
                    //flag to control if file name already exists
                    bool fileAlreadyExists = false;

                    if (fileAlreadyExists == false) 
                    {
                        //load in the path
                        string path = ApplicationData.Current.LocalFolder.Path;
                        DirectoryInfo dinfo = new DirectoryInfo(@path);
                        FileInfo[] Files = dinfo.GetFiles("*");

                        //load in the files 
                        foreach (FileInfo file in Files)
                        {
                            //look grab the title without the extension
                            string title = System.IO.Path.GetFileNameWithoutExtension(file.Name);

                            //if a file with the same name already exists, don't save
                            //return a failed to save message
                            if (title == saveDialog.UserNote)
                            {
                                ContentDialog savedDialog = new ContentDialog()
                                {
                                    Content = "Sorry, a note with that name already exists! Try again",
                                    Title = "Save Unsuccesful",
                                    PrimaryButtonText = "Ok"
                                };
                                await savedDialog.ShowAsync();       
                                fileAlreadyExists = true;
                            }
                        }
                    }

                    //if there is no file with that name already, then do a save
                    if (fileAlreadyExists == false) 
                    {
                        try
                        {
                            //execture the save method from the NotesRepo class
                            Repositories.NotesRepo.SaveNoteToFile(_noteViewModel.SelectedNote, saveDialog.UserNote, newContent);

                            //display a saved note message to user
                            ContentDialog savedDialog = new ContentDialog()
                            {
                                Content = "Note saved successfully to file",
                                Title = "Save Succesful",
                                PrimaryButtonText = "Ok"
                            };
                            await savedDialog.ShowAsync();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Error when attempting to save to file");
                        }

                        _noteViewModel.createNewNote(saveDialog.UserNote);
                    }                  
                }
            }
        }
    }
}
