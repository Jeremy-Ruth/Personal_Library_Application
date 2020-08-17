using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalLibraryApp
{
    /// <summary>
    /// This class is used to build and work with items for the list of objects to display in the dynamic areas of the GUI.
    /// </summary>
    class DispListObject
    {
        private string listItemISBN;
        private string listItemTitle;
        private string[] listItemAuthor;
        private int listItemStarRating;

        public DispListObject(Book bookToAdd)
        {
            listItemISBN = bookToAdd.getISBN();
            listItemTitle = bookToAdd.getBookTitle();
            listItemAuthor = bookToAdd.getAuthors();
            listItemStarRating = bookToAdd.getStarRank();
        }//END DispListObject CONSTRUCTOR
        

        // Gets the ISBN of a book on the display list. ISBNs can contain numbers and letters, thus the type must be a string
        public string GetISBN()
        {
            return listItemISBN;
        }//END getISBN


        // Gets the title from an item on the display list
        public string GetTitle()
        {
            return listItemTitle;
        }//END getTitle


        // Gets the authors from a book in the display list, formatted so that they are appended as one comma seperated string
        public string GetAuthorsList()
        {
            string listOfAuthors = null;
            for(int i = 0; i < listItemAuthor.Length; i++)          // Add the name of each author into a formatted list if a name exists
            {                                                       
                if (listItemAuthor[i] != null)                      
                    listOfAuthors += listItemAuthor[i] + ", ";
            }

            return listOfAuthors;
        }//END GetAuthorsList


        // Gets the user star rating for an item in the display list
        public int GetStarRating()
        {
            return listItemStarRating;
        }//END GetStarRating
    }//END DispListObject CLASS
}//END NAMESPACE
