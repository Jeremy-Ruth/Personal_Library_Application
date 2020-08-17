using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibraryApp
{
    /// <summary>
    /// Communication methods and processes for interaction between the GUI and backend classes
    /// This class acts as control or a gateway between the front end and the data storage mechanisms to help structure
    /// the program, but more importantly to provide a measure of access control to data. This program does not
    /// currently store any personal data, so the access control is primarily just for practice with this type of thinking
    /// and the resultant method duplication for "pass-through" to the GUI is probably unnecessary
    /// </summary>
    class Comms
    {
        private static Database currDatabaseInstance;
        private static List<Object> currBook = new List<Object>();
        private static List<DispListObject> listForDisplay = new List<DispListObject>();
        private static Library currLibrary;

        // Constructor to create a database instance for the app
        // Establishes connection to the library database and collects DB data into lists for initial GUI view
        public Comms()
        {
            currDatabaseInstance = new Database();
            currDatabaseInstance.EstablishDBConnection();
         }//END Comms CONSTRUCTOR


        // Collects all of the database tables into their respective libraries of the app dictionaries
        // Also creates and orders a default list of the books to display in the dynamic region (current default is wishlist books)
        public void GetDefaultLibraries()
        {
            currLibrary = currDatabaseInstance.GetEntireDB(listForDisplay);
            CompileListForDisplay(Library.whichLibrary.Wishlist);
            listForDisplay = (from listObj in listForDisplay orderby listObj.GetTitle() select listObj).ToList();
         }//END GetDefaultBookDetails


        public List<Object> QueryBookLibrary(Library.whichLibrary libraryToSearch, string bookKey)
        {
            Book foundBook = currLibrary.GetBookFromLib(libraryToSearch, bookKey);
            currBook = currLibrary.FetchBookAsList(foundBook);

            return currBook;
        }//END List


        // Gets the total number of books currently in a given library
        public int GetLibItemCount(Library.whichLibrary libToCount)
        {
            int bookTotal = 0;

            if (libToCount == Library.whichLibrary.Owned)
                bookTotal = currLibrary.OwnedTotal();
            else if (libToCount == Library.whichLibrary.Wishlist)
                bookTotal = currLibrary.WishlistTotal();
            else
                Console.WriteLine("There was an error getting the total. An invalid library was specified!");

            return bookTotal;
        }//END GetLibItemCount


        // Creates the display list based on the current view by collecting items from the appropriate library
        private static void CompileListForDisplay(Library.whichLibrary libraryToUse)
        {
            if (libraryToUse == Library.whichLibrary.Owned)
                listForDisplay = currLibrary.GetLibrarySubset(Library.whichLibrary.Owned);
            else if (libraryToUse == Library.whichLibrary.Wishlist)
                listForDisplay = currLibrary.GetLibrarySubset(Library.whichLibrary.Wishlist);
            else
                Console.WriteLine("There was an error retrieving a specific library. An improper or empty library name was specified!");
        }//END CompileListForDisplay


        // Return the total number of items in the current display list
        public int GetDisplayListCount()
        {
            return listForDisplay.Count;
        }//END GetDisplayListCount


        // Returns one item from the current display list formatted for the dynamic display region fo the GUI
        public DispListObject GetDisplayListItem(int indexOfItem)
        {            
            /* Fall back method of collecting data as a string, can probably delete after frurther testing
            string result = listForDisplay[indexOfItem].GetTitle() + "; " +
                listForDisplay[indexOfItem].GetAuthorsList() + "; " +
                listForDisplay[indexOfItem].GetISBN() + "; " +
                listForDisplay[indexOfItem].GetStarRating().ToString();
                */

            return listForDisplay[indexOfItem];
        }//END GetDisplayListItem


        // Returns the unique ID of the first item in the display list.
        // Mainly used for the initial item detail view when changing library subsets
        public string GetFirstListItemKey()
        {
            return listForDisplay[0].GetISBN();
        }//END GetFirstListItemKey


        public List<DispListObject> GetDisplayList()
        {
            return listForDisplay;
        }


        /*
        public T GetBookDetail<T>(enum detailToGet, Book bookFromLib)
        {
            T bookDetail;
        
            if(detailToGet == Book.bookSpecs.starRanking)
                bookDetail = bookFromLib.getStarRank();

            return bookDetail;
        }//END GetBookDetail        */
    }//END Comms CLASS
}//END NAMESPACE
