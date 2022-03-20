﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalNote.Models
{
    public class NoteModel
    {
        public string Title { get; set; }
        public string Content { get; set; }

        private string _filter;
        public string Filter
        {

            get { return _filter; }
            set { _filter = value; }

        }


        //constructor
        public NoteModel(string title, string content) 
        {
            Title = title;
            Content = content;
        }

        public NoteModel() { }

        public string getTitle()
        {
             return Title; 



        }

        public string getContent()
        {
             return Content; 



        }
    }
}
