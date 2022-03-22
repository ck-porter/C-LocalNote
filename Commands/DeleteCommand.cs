﻿using LocalNote.Repositories;
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
                //grab the path to the file
                string path = ApplicationData.Current.LocalFolder.Path;

                DirectoryInfo dinfo = new DirectoryInfo(@path);
                FileInfo[] Files = dinfo.GetFiles("*");

                //grab the title of the selected note
                string selectedTitle = _noteViewModel.getTitle();

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
}
