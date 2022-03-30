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
        private DataRepository _dataRepository;

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
            _dataRepository = new DataRepository();
            string updateContents = _noteViewModel.getContents();
            bool existingTitle=false;

            //this is to update the file
            if (_noteViewModel.SelectedNote != null)
            {
                string fileName = _noteViewModel.getTitle();


                //----------------------------------------------------Update existing note
                //pass the title to the DataRepo to check if it already exists
                existingTitle = _dataRepository.CheckIfExisting(fileName);

                if (existingTitle == true)
                {

                    Repositories.DataRepository.UpdateNote(fileName, updateContents);

                    //display the saved note message
                    ContentDialog savedDialog = new ContentDialog()
                    {

                        Content = "Note saved successfully to file",
                        Title = "Save Succesful",
                        PrimaryButtonText = "Ok"
                    };
                    await savedDialog.ShowAsync();

                    //reload the list
                    _noteViewModel.refreshNotes(updateContents);

                }
            }

            else if (_noteViewModel.SelectedNote == null) 
            {
                SaveNoteDialog saveDialog = new SaveNoteDialog();
                ContentDialogResult result = await saveDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {

                    existingTitle = _dataRepository.CheckIfExisting(saveDialog.UserNote);

                    if (existingTitle == false)
                    {
                        Repositories.DataRepository.SaveNewNote(saveDialog.UserNote, updateContents);
                        //display the saved note message
                        ContentDialog savedDialog = new ContentDialog()
                        {

                            Content = "Note saved successfully to file",
                            Title = "Save Succesful",
                            PrimaryButtonText = "Ok"
                        };
                        await savedDialog.ShowAsync();

                        //reload the list
                        _noteViewModel.refreshNotes(updateContents);

                    }
                    else if(existingTitle ==true)
                    {
                        ContentDialog savedDialog = new ContentDialog()
                        {
                            Content = "Sorry, a note with that name already exists! Try again",
                            Title = "Save Unsuccesful",
                            PrimaryButtonText = "Ok"
                        };
                        await savedDialog.ShowAsync();
                    }

                }

            }



            ////catch a new note with same name
            //else if (_noteViewModel.SelectedNote != null && existingTitle == true) 
            //{
            //    ContentDialog savedDialog = new ContentDialog()
            //    {
            //        Content = "Sorry, a note with that name already exists! Try again",
            //        Title = "Save Unsuccesful",
            //        PrimaryButtonText = "Ok"
            //    };
            //}


         

                //instantiate new instance of our dialog box
                //SaveNoteDialog saveDialog = new SaveNoteDialog();

                //ContentDialogResult result = await saveDialog.ShowAsync();  

                //string newContent = _noteViewModel.getContents();

                //    if (result == ContentDialogResult.Primary) 
                //    {
                //        //flag to control if file name already exists
                //        bool fileAlreadyExists = false;

                //        if (fileAlreadyExists == false) 
                //        {
                //            //load in the path
                //            string path = ApplicationData.Current.LocalFolder.Path;
                //            DirectoryInfo dinfo = new DirectoryInfo(@path);
                //            FileInfo[] Files = dinfo.GetFiles("*");

                //            //load in the files 
                //            foreach (FileInfo file in Files)
                //            {
                //                //look grab the title without the extension
                //                string title = System.IO.Path.GetFileNameWithoutExtension(file.Name);

                //                //if a file with the same name already exists, don't save
                //                //return a failed to save message
                //                if (title == saveDialog.UserNote)
                //                {
                //                    ContentDialog savedDialog = new ContentDialog()
                //                    {
                //                        Content = "Sorry, a note with that name already exists! Try again",
                //                        Title = "Save Unsuccesful",
                //                        PrimaryButtonText = "Ok"
                //                    };
                //                    await savedDialog.ShowAsync();       
                //                    fileAlreadyExists = true;
                //                }
                //            }
                //        }

                //if there is no file with that name already, then do a save
                //if (fileAlreadyExists == false) 
                //{
                //    try
                //    {

                //        Repositories.DataRepository.SaveNewNote(saveDialog.UserNote, newContent);


                //        //execture the save method from the NotesRepo class
                //        //Repositories.NotesRepo.SaveNoteToFile(_noteViewModel.SelectedNote, saveDialog.UserNote, newContent);

                //        //display a saved note message to user
                //        ContentDialog savedDialog = new ContentDialog()
                //        {
                //            Content = "Note saved successfully to file",
                //            Title = "Save Succesful",
                //            PrimaryButtonText = "Ok"
                //        };
                //        await savedDialog.ShowAsync();
                //    }
                //    catch (Exception ex)
                //    {
                //        Debug.WriteLine("Error when attempting to save to file");
                //    }

                //    _noteViewModel.createNewNote(saveDialog.UserNote);

                //}                  
            //}
            //}
        }
    }
}
