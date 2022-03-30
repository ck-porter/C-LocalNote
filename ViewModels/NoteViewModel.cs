using LocalNote.Commands;
using LocalNote.Models;
using LocalNote.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalNote.ViewModels
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public MainPage MainPage { get; set; }
        public NotesRepo notesRepo { get; set; }
        public DataRepository dataRepository { get; set; }
        public string Title { get; set; }
        public List<NoteModel> _allNotes;        
        public ObservableCollection<NoteModel> Notes { get; set; }
        public SaveCommand SaveCommand { get; }
        public DeleteCommand DeleteCommand { get; }
        public EditCommand EditCommand { get; }
        public AddCommand AddCommand { get; }
        public AboutCommand AboutCommand { get; }
        public ExitCommand ExitCommand { get; }

        //to bind to UI
        public string vmContent { get; set; }
        public string vmTitle { get; set; }


        public string NoteContent;   

        private string _filter;
        public string Filter
        {
            get { return _filter; }
            set
            {
                //if there is nothing in filter/same as previous search, return
                //check to see if new value is same as previous value
                if (value == _filter) { return; }

                //else if it is a new value, then set it and proceed with filtering
                _filter = value;

                PerformFiltering();

                //want to trigger a re-bind because the list has been filtered, names removed, and we need
                //to reflect that in the listitem view
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Filter)));  //rekicks databinding to say filter has changed
            }
        }

        private NoteModel _selectedNote;

        public NoteModel SelectedNote
        {
            get { return _selectedNote; }

            set 
            {
                _selectedNote = value;

                if (value == null)
                {
                    vmContent = "";
                    vmTitle = "";
                }
                else
                {
                    vmContent = value.Content;
                    vmTitle = value.Title;  
                    //if user has selected a note, then make sure the textbox is in readonly mode
                    MainPage.readOnlyMode();
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("vmContent"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("vmTitle"));
            }       
        }


        public NoteViewModel()
        {
            SaveCommand = new SaveCommand(this);
            EditCommand = new EditCommand(this);
            AddCommand = new AddCommand(this);
            DeleteCommand = new DeleteCommand(this);
            AboutCommand = new AboutCommand(this);
            ExitCommand = new ExitCommand(this);
            notesRepo = new NotesRepo();
            dataRepository = new DataRepository();

            Notes = new ObservableCollection<NoteModel>();
            _allNotes = new List<NoteModel>();

            //load in the existing note files
           // notesRepo.loadFiles(this);
            dataRepository.LoadNotes(this);





            PerformFiltering();
        }


        private void PerformFiltering()
        {
            if (_filter == null)
            {
                _filter = "";
            }
         
            var lowerCaseFilter = Filter.ToLowerInvariant().Trim();
     
            var result =
                _allNotes.Where(d => d.Title.ToLowerInvariant()  
                .Contains(lowerCaseFilter)) 
                .ToList();
                    
            var toRemove = Notes.Except(result).ToList();
          
            foreach (var x in toRemove)
            {
                Notes.Remove(x);
            }

            var resultCount = result.Count;
    
            for (int i = 0; i < resultCount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > Notes.Count || !Notes[i].Equals(resultItem))
                {
                    Notes.Insert(i, resultItem);
                }
            }
        }

        public void mpContentUnlock() 
        {
            //calls function from main page to enable editing
            MainPage.editMode();        
        }

        public void mpNewNote() 
        {
            //calls function from main pain enable a new note to be created
            MainPage.newNote();           
        }

        public void mpCreateNote(string title) {

            //get the content of the note
            MainPage.getNoteContent();              
        }       
   
        public void createNewNote(string title)
        {
            NoteModel nm = new NoteModel(); 
            string Note = MainPage.getNoteContent();   
            
            //create a new note object
            nm = new Models.NoteModel(title, Note);

            //add the new note to the notes collection
            this.Notes.Add(nm);
        }

        public void loadNotes(string title, string content) 
        {
            NoteModel nm = new NoteModel();                       
            nm = new Models.NoteModel(title, content);
            this._allNotes.Add(nm);
            this.Notes.Add(nm);

        }

        public void refreshNotes(string vmContent) 
        {
            //clear the list and collection, then reload
            _allNotes.Clear();
            Notes.Clear();
            // notesRepo.loadFiles(this);
            dataRepository.LoadNotes(this);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("vmContent"));

        }

        public void removeNote() 
        {
            //clear the list and collection, then reload
            _allNotes.Clear();
            Notes.Clear();
            notesRepo.loadFiles(this);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("vmContent"));

        }

        public string getContents() 
        {
            string Note = MainPage.getNoteContent();
            return Note;
        }

        public string getTitle() 
        {
            return SelectedNote.Title;
        }

    }
}
