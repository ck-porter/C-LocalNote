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
       // private DataRepository _dataRepository;

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

            //instantiate new instance of our dialog box
            DeleteNoteDialog1 deleteDialog = new DeleteNoteDialog1();     
          
            ContentDialogResult result = await deleteDialog.ShowAsync();   
      
            if (result == ContentDialogResult.Primary) 
            {
                //grab the title of the selected note
                string selectedTitle = _noteViewModel.getTitle();

                Repositories.DataRepository.DeleteNote(selectedTitle);

                try
                {
                    
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

                //reset state
                _noteViewModel.mpNewNote();

            }
        }
    }
}
