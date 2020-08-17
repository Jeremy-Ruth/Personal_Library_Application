using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalLibraryApp
{
    /// <summary>
    /// Methods relating to creating and working with collection libraries
    /// </summary>
    class Library
    {
        private static Dictionary<string, Book> wishlistLib;
        private static Dictionary<string, Book> ownedLib;
        
        // Set of enums to replace magic numbers when passing a library to work with
        public enum whichLibrary
        {
            Wishlist,           // int val of 0
            Owned,
            Series              // int val of 2
        }

        // Set of enums to replace magic numbers when specifying the data to prioritize for an algorithm
        public enum bookDataKey
        {
            Initial,            // int val of 0, currently initial is the same
            ISBN,
            Title,
            Author,
            StarRating          // int val of 4
        }

        // Constructor for a library set in the app instance. Creates an empty link list for each type of library currently available
        // Currently the available libraries are: Wishlist and Owned (books)
        // Next library to work on is the book series library
        public Library()
        {
            wishlistLib = new Dictionary<string, Book>();
            ownedLib = new Dictionary<string, Book>();
        }//END Library CONSTRUCTOR


        public void AddToLibrary(Book newBook, whichLibrary libraryToUse)
        {
            if(libraryToUse == whichLibrary.Wishlist)
            {
                wishlistLib.Add(newBook.getISBN(), newBook);
            }else if(libraryToUse == whichLibrary.Owned)
            {
                ownedLib.Add(newBook.getISBN(), newBook);
            }else
            {
                Console.WriteLine("There was an error with determining which library to add the book to!");
                // Add series option as additonal step here once feature is added
                // The console output will need to be updated to display a popup window to notify user after testing
            }
        }//END AddToLIbrary
        

        // Returns a list of items from a particular dictionary (library subset) in the library as is. Used primarily for display in the GUI.
        // Since working with a dictionary the order is never certain and must be handled at a later time to ensure consistent output.
        public List<DispListObject> GetLibrarySubset(whichLibrary subsetToRetrieve)
        {
            List<DispListObject> tempList = new List<DispListObject>();
            Dictionary<string, Book> tempDictionaryRef;                                 // Make an internal reference variable to capture the dictionary to avoid code duplication later
            DispListObject bookFromDictionary;                                          // An internal instance of the abbreviated book details used for list display in the GUI

            if (subsetToRetrieve == whichLibrary.Owned)                                 // Choose the proper library subset to access
                tempDictionaryRef = ownedLib;
            else if (subsetToRetrieve == whichLibrary.Wishlist)
                tempDictionaryRef = wishlistLib;
            else
            {
                tempDictionaryRef = null;
                Console.WriteLine("There was an error in assigning the dictionary in the library. An invalid or empty library name was used!");
            }
            
            foreach(KeyValuePair <string, Book> entry in tempDictionaryRef)             // for each book in the subset get the details needed for a list display
            {                                                                           // then add it to the current list
                tempList.Add(bookFromDictionary = new DispListObject(entry.Value));
            }

            return tempList;
        }//END GetLibrarySubset


        public int WishlistTotal()
        {
            return wishlistLib.Count;
        }//END WishListTotal


        //Get the total number of books that are owned
        public int OwnedTotal()
        {
            return ownedLib.Count;
        }//END OwnedTotal
        

        public Book GetBookFromLib(whichLibrary currLibView, string currBookKey)
        {
            Book foundBook;

            /* This will need to be modified to do a "TryGet" check later to handle cases where the ID does not exist */

            if (currLibView == whichLibrary.Owned)
                foundBook = ownedLib[currBookKey];
            else if (currLibView == whichLibrary.Wishlist)
                foundBook = wishlistLib[currBookKey];
            else
            {
                foundBook = null;
                Console.WriteLine("There was an error getting the book specified... Sure would be nice to have that try get check!");
            }

            return foundBook;
        }//END GetBookFromLib


        public List<Object> FetchBookAsList(Book bookToConvert)
        {
            List<Object> convertedBook = new List<Object>();
            return convertedBook = bookToConvert.ConvertBookToList();
        }//END ConvertBookToList
    }//END Library CLASS
}//END NAMESPACE
