using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalLibraryApp
{
    /// <summary>
    /// Methods relating to creating and working with book objects.
    /// The use of string for "numbers" in the case of ISBN and ASIN is intentional. These identifiers, though called numbers,
    /// can have a combination of letters and numbers and often do.
    /// </summary>
    class Book
    {
        private string isbn;                                    // ISBN needs to be a string because not all ISBN are fully numerical
        private string bookTitle;
        private string author1, author2, author3, author4;
        private string asin;                                    // ASIN needs to be a string because not all ISBN are fully numerical
        private int numOfPages;
        private string bookDescription;
        private string storeLink;
        private int isOwned;
        private bool isInSeries;
        private int seriesRef;
        private int starRank;
        private string pathToCover;
        private string linkToPreview;

        // Set of enums to replace magic numbers to specify details of a book
        public enum bookSpecs
        {
            ISBN,                   // int val of 0
            titleOfbook,
            auth1,
            auth2,
            auth3,
            auth4,
            ASIN,
            pageCount,              // int val of 7
            descOfBook,
            linkToStore,
            owned,
            partOfSeries,
            starRanking,
            bookCover,
            previewLink             // int val of 14
        }

        // Constructor for a book instance 
        public Book(List<Object> currBook)
        {
            // Need to add a validation steps into this. Return to later when have more of the program working

            isbn = ReadDBString(currBook[0]);                       // Cannot be empty to insert into DB
            bookTitle = ReadDBString(currBook[1]);                  // Cannot be empty to insert into DB
            author1 = ReadDBString(currBook[2]);
            author2 = ReadDBString(currBook[3]);
            author3 = ReadDBString(currBook[4]);
            author4 = ReadDBString(currBook[5]);
            
            if (currBook[6] == null)
                asin = " ";
            else
                asin = ReadDBString(currBook[6]);
            
            numOfPages = ReadDBInt(currBook[7]);
            bookDescription = ReadDBString(currBook[8]);
            storeLink = ReadDBString(currBook[9]);
            isOwned = ReadDBInt(currBook[10]);                      // Cannot be empty to insert into DB
            isInSeries = ReadDBBool(currBook[11]);
            seriesRef = ReadDBInt(currBook[12]);
            
            if (currBook[13] == null)
                starRank = 0;
            else
                starRank = ReadDBInt(currBook[13]);
            
            pathToCover = ReadDBString(currBook[14]);
            linkToPreview = ReadDBString(currBook[15]);
        }//END Book CONSTRUCTOR


        // Convert a database string to a working string for a book field. 
        // Since not all fields are required we need to account for database null values
        private string ReadDBString(object fieldToAdd)
        {
            if (fieldToAdd == null || fieldToAdd == DBNull.Value)
                return null;
            else
                return Convert.ToString(fieldToAdd);
        }//END ReadDBString


        // Comverts a database integer to an app readable int for a book field.
        // Since not all fields are required we need to account for datbase null values
        private int ReadDBInt(object fieldToAdd)
        {
            if (fieldToAdd == null || fieldToAdd == DBNull.Value)
                return 0;
            else
                return Convert.ToInt32(fieldToAdd);
        }//END ReadDBInt


        // Since boolean fields like "is in a series" are not required for book types,
        // this method has to do a null check. If found they are defaulted to zero (no)
        private bool ReadDBBool(object fieldToAdd)
        {
            if (fieldToAdd == DBNull.Value)
                return false;
            else
                return (bool)fieldToAdd;
        }//END ReadDBBool


        public string getISBN()
        {
            return isbn;
        }//END getISBN


        public string getBookTitle()
        {
            return bookTitle;
        }//END getBookTitle
        

        public string[] getAuthors()
        {
            string[] listOfAuthors = new string[4];

            listOfAuthors[1] = author1;
            if (author2 != null)
                listOfAuthors[2] = author2;
            if (author3 != null)
                listOfAuthors[3] = author3;
            if (author4 != null)
                listOfAuthors[4] = author4;

            return listOfAuthors;
        }//END getAuthors

        
        // Status as stored in database: 0 => wishlist, 1 => owned. Others may be added in the future
        public int getOwnedStat()
        {
            return isOwned;
        }//END getOwnedStat


        // Possible book rating range is currently 0 - 5 stars
        public int getStarRank()
        {
            return starRank;
        }//END getStarRank


        // The order of the list matches the order in which values are displayed in the detail region of the GUI
        // Values that are not visually represented in the GUI are moved to the end of the list
        public List<object> ConvertBookToList()
        {
            List<Object> conversionResult = new List<Object>();
            string combineAuthors = null;

            conversionResult.Add(bookTitle);
            conversionResult.Add(isbn);
            conversionResult.Add(asin);

            if (author1 != null)
                combineAuthors += author1;
            if (author2 != null)
                combineAuthors += "/ " + author2;
            if (author3 != null)
                combineAuthors += "/ " + author3;
            if (author4 != null)
                combineAuthors += "/ " + author4;

            conversionResult.Add(combineAuthors);
            conversionResult.Add(numOfPages);
            conversionResult.Add(bookDescription);
            conversionResult.Add(seriesRef);
            conversionResult.Add(starRank);
            conversionResult.Add(pathToCover);
            conversionResult.Add(storeLink);
            conversionResult.Add(isOwned);
            conversionResult.Add(isInSeries);
            conversionResult.Add(linkToPreview);

            return conversionResult;
        }//END ConvertBookToList
    }//END Book CLASS
}//END NAMESPACE
