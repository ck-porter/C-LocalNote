using LocalNote.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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

            //this forces the program to wait until the dialog box is complete

            ContentDialogResult result = await saveDialog.ShowAsync();   //store the button value into result

            string newContent = _noteViewModel.getContents();


                if (result == ContentDialogResult.Primary) //primary is the save button on the dialog box
                {
                    //do the save using windows storage

                    try
                    {
                        Repositories.NotesRepo.SaveNameDaysToFile(_noteViewModel.SelectedNote, saveDialog.UserNote, newContent);

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
