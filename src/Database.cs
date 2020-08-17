using System.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace PersonalLibraryApp
{
    /// <summary>
    /// Performs functions to work with the database
    /// The database in use for this app is currently SQLite version 3.
    /// </summary>
    class Database
    {
        private static string pathToDB;
        private static readonly string currSqLiteVer = ";Version=3";
        SQLiteConnection currDBConn;

        // Constructor for the database class. Establishes and stores the path to the local database for further interactions
        public Database()
        {
            SetDBPath();
        }//END Database CONSTRUCTOR


        // Internal setter for the path to the library database since only default location is currently allowed
        // May revise functionality later to allow user to specify a different location if it seems useful as develop features
        private static void SetDBPath()
        {
            string projectPath = Path.GetFullPath("DatabaseComms.cs");
            projectPath = Path.GetDirectoryName(projectPath);
            pathToDB = "DATASOURCE=" + projectPath + "\\database\\PersonalLibrary.db";
        }//END SetDBPath

 
        public void EstablishDBConnection()
        {
            // Connect to the local library database, specifying the current SQLite version
            currDBConn = new SQLiteConnection(pathToDB + currSqLiteVer);

            // Open the database connection. If unable to connect notify the user
            try
            {
                currDBConn.Open();
            }
            catch (Exception dbConError)
            {
                //This will need to be updated later to display a popup message to the user from the GUI
                Console.WriteLine($"An error was generated when opening the database: '{dbConError}'");
            }
        }//END EstablishConnection


        public void TerminateDBConnection()
        {
            if ((currDBConn != null) && (currDBConn.State == ConnectionState.Open))
                currDBConn.Close();
        }//END TerminateDBConnection


        // Gets the entire database collection to use for storing in the proper app libraries
        // The method requires the displaylist that will be used as the initial list in the dynamic area of the GUI (currently is wishlist items)
        public Library GetEntireDB(List<DispListObject> currViewList)
        {
            Library dbLibs = new Library();
            List<Object> currBook = new List<Object>();      

            // Create a query command object and perfom a query on the database to retrieve all book library data
            using (SQLiteCommand currCommand = currDBConn.CreateCommand())
            {
                currCommand.CommandText = "SELECT * FROM BOOKS";                // Query to retrieve all books
                currCommand.CommandType = CommandType.Text;                     // specify is a text command to reduce SQL overhead
                SQLiteDataReader bookData = currCommand.ExecuteReader();        // Create a reader to interpret the book data

                while (bookData.Read())                                         // While there is another book to ready               
                {
                    for(int i = 0; i <= 15; i++)                                // Add the details of the book to a list of book values (accounting for tracker offset)
                    {
                        currBook.Add(bookData[i]);
                    }
                    Book nextBook = new Book(currBook);                         // then create a book object with the value from the list

                    if (nextBook.getOwnedStat() == 0)
                        dbLibs.AddToLibrary(nextBook, Library.whichLibrary.Wishlist);       // If the book is not owned add it to the wishlist library
                    else if (nextBook.getOwnedStat() == 1)
                        dbLibs.AddToLibrary(nextBook, Library.whichLibrary.Owned);          // If the book is owned add it to the owned library
                    else
                        Console.WriteLine("There was an error adding the book. The owned status was empty or contained an invalid status!");

                    currBook.Clear();                                                       // empty the temp book details to prepare for the next book
                }
            }

            return dbLibs;
        }//END GetWholeDB
    }//END Database
}//END NAMESPACE
