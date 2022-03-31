using LocalNote.Models;
using LocalNote.Repositories;
using LocalNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



namespace LocalNote
{
 
    public sealed partial class MainPage : Page
    {
        public DataRepository dataRepository;

        public NoteViewModel viewModel;
               
        public string NoteContent { get; set; }
        public string NoteTitle { get; set; }

        public ViewModels.NoteViewModel NoteViewModel { get; set; }


        public MainPage()
        {
            this.InitializeComponent();  
            
            this.dataRepository = new DataRepository();
            dataRepository.InitializeDatabase();

            this.NoteViewModel = new ViewModels.NoteViewModel();
            NoteViewModel.MainPage = this;
            //starting postion
            newNote();
            this.NoteContent = ContentBox.Text;
           
        }
                            

        public void readOnlyMode() 
        {
            
            ContentBox.IsReadOnly = true;
            SaveIcon.IsEnabled = false;
            Edit.IsEnabled = true;
            Delete.IsEnabled = true;
            About.IsEnabled = true;

        }

        public void editMode() 
        {
            //disable edit, enable typing in content box
            ContentBox.IsReadOnly = false;
            SaveIcon.IsEnabled = true;
            Edit.IsEnabled = false;
            ContentBox.IsReadOnly = false;
            Delete.IsEnabled = false;
            About.IsEnabled = true;

        }

        public void newNote() 
        {
            //deselect currently selected note, change button availability 
            titleListView.SelectedIndex = -1;
            SaveIcon.IsEnabled = true;
            Edit.IsEnabled = false;
            ContentBox.IsReadOnly = false;   
            Delete.IsEnabled= false;
            About.IsEnabled = true;

        }

        public string getNoteContent()
        {
            if (ContentBox.Text == null) 
            {
                throw new ArgumentException("No note selected.");
            }
            return ContentBox.Text;

        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }
    }
}
