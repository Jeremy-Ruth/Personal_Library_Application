using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Xaml.Behaviors;

namespace PersonalLibraryApp
{
    /// <summary>
    /// Jeremy Ruth - 2020
    /// This program is intended to provide a means to manage a personal book library. A database (SQLite)
    /// is used to manage the library data. This is a single user software and does not have user accounts.
    /// The intended feature list includes:
    /// 
    /// - Manual entry and storage of books including details and book cover images
    /// - A wishlist feature to track books of interest
    /// - Connection to the Google Books API for auto filling details and finding new books
    /// - Simple star rating system
    /// - A series feature to identify and group books that are related such as a trilogy
    /// - Simple easy to use dynamic GUI that includes a detail view and filterable book list
    /// 
    /// In future evolutions I would like to extend these features to movies as well.
    /// 
    /// This file includes interaction logic for the MainWindow.xaml
    /// Is using a code behind approach rather than MVVM at this time
    /// </summary>
    public partial class MainWindow : Window
    {
        // Set of enums to replace magic numbers for sidebar menu items
        public enum selectedMenuItem
        {
            OwnBooks,           // int val of 0
            OwnMovies,
            WishBooks,
            WishMovies,
            BookSeries,
            Settings            // int val of 5
        }

        private int currMenuItem;
        private int currBookCount;
 
        private Comms currCommChannel;
        private System.Data.DataTable collectionListTable;

        public MainWindow()
        {
            InitializeComponent();
            /* 
             * DoInitialSetup means:
             * Create an empty library (through creating a comms instance)
             * Populate Library (import database into the library dictionaries)
             * Set GUI style for the default menu selection in the sidebar to focused (default is Wishlist)
             * Populate the list for the current view (view list is an ordered version of the current library dictionary)
             * Update the title of the top menu region to that of the current library title (default currently is "My Wishlist Library")
             * Output the entire list to the dynamic list view region of the GUI
             * Pull in an initial book for the library (default order is by title and the first book is used on) and update detail region
             * Set first item in the dynamic library view list's GUI graphic style to focused
             */
            DoInitialSetup();
        }//END MainWindow


        // Need to re-enable dragging capability since default menu bar is removed
        private void MakeAppDraggable(object winToDrag, MouseButtonEventArgs winDragEvent)
        {
            if (winDragEvent.ChangedButton == MouseButton.Left)
                this.DragMove();
        }//END MakeAppDraggable


        // Custom minimize button behavior
        private void MinimizeAppClick(object minBttn, RoutedEventArgs clickToMinEvent)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }//END MinimizeAppClick


        // Custom maximize button behavior
        private void MaximizeAppClick(object maxBttn, RoutedEventArgs clickToMaxEvent)
        {
            if (this.WindowState != System.Windows.WindowState.Maximized)
                this.WindowState = System.Windows.WindowState.Maximized;
            else
                this.WindowState = System.Windows.WindowState.Normal;
        }//END MaximizeAppClick


        // Custom close button behavior
        private void CloseAppClick(object closeBttn, RoutedEventArgs clickToCloseEvent)
        {
            Close();
        }//END CloseAppClicl


        // Sets up a table structure for current view's GUI list
        private void MakeCollectionListTable()
        {
            collectionListTable = new DataTable("ParentTable");
            DataColumn listColumn; 

            listColumn = new DataColumn();
            listColumn.DataType = System.Type.GetType("System.String");
            listColumn.ColumnName = "ObjUniqueID";
            listColumn.ReadOnly = true;
            listColumn.Unique = true;
            collectionListTable.Columns.Add(listColumn);

            listColumn = new DataColumn();
            listColumn.DataType = System.Type.GetType("System.String");
            listColumn.ColumnName = "ObjTitle";
            listColumn.ReadOnly = true;
            listColumn.Unique = false;
            collectionListTable.Columns.Add(listColumn);

            listColumn = new DataColumn();
            listColumn.DataType = System.Type.GetType("System.String");
            listColumn.ColumnName = "ObjCreator";
            listColumn.ReadOnly = true;
            listColumn.Unique = false;
            collectionListTable.Columns.Add(listColumn);

            listColumn = new DataColumn();
            listColumn.DataType = System.Type.GetType("System.Int32");
            listColumn.ColumnName = "ObjStarRating";
            listColumn.ReadOnly = true;
            listColumn.Unique = false;
            collectionListTable.Columns.Add(listColumn);
        }//END MakeCollectionListTable


        // Performs the rating update in database when user makes a change from the GUI
        private void UpdateStarRating(object currStar, RoutedEventArgs starMouseOverEvent)
        {
            int rating = int.Parse(string.Format("{0}", (currStar as ToggleButton).Tag));

            RatingChange(rating);
            // **********Call method to update rating in database here********

        }//END UpdateStarRating


        // Updates the star rating on the GUI
        private void RatingChange(int ratingToUse)
        {
            UIElementCollection starSet = RatingSet.Children;
            int rating = ratingToUse;
            ToggleButton currBttn = null;
            bool doReset = false;

            // Star rating "6" is actually the clear button in the GUI
            if (rating >= 6)
            {
                rating = 0;
                doReset = true;
                currBttn = starSet[5] as ToggleButton;                  
                currBttn.IsChecked = false;
            }

            if (!doReset)
            {
                for (int i = 0; i < rating; i++)
                {
                    currBttn = starSet[i] as ToggleButton;
                    if (currBttn != null)
                        currBttn.IsChecked = true;
                }
            }
            for (int i = rating; i < 5; i++)
            {
                currBttn = starSet[i] as ToggleButton;
                if (currBttn != null)
                    currBttn.IsChecked = false;
            }
        }//END RatingChange


        // Performs tasks to put the app in its initial view and data state. Default view is wishlist currently
        private void DoInitialSetup()
        {
            MakeCollectionListTable();                                                                      // Create the shell for displaying the list of current collection items
            string initialBookID;                                                                           // Need the ISBN of the initial book to display which is the first book from the wishlist sorted by ascending title
            List<Object> bookToView;                                                                        // and will also need to get the book to view details for 

            currCommChannel = new Comms();                                                                  // Initalize the data communication pathway and retrieves the entire library databse into app libraries
            currCommChannel.GetDefaultLibraries();                                                          // Load all of the libraries stored in the database into their respective in app libarires
            SetInitialContext();                                                                            // Update the menu selection and dynamic items to represent the initial state of the app
            DisplayList();                                                                                  // Pupulate the list area in the dynamic region with the wishlist books
            initialBookID = currCommChannel.GetFirstListItemKey();                                          // Get the unique ID of the initial book to display in the detail region then get the book details as a list
            bookToView = currCommChannel.QueryBookLibrary(Library.whichLibrary.Wishlist, initialBookID);     
            UpdateDetailRegion(bookToView);                                                                 // Populate the detail region with the default book 
        }//END DoInitialSetup


        // Sets the GUI view and state variables for the initial loading state
        public void SetInitialContext()
        {
            currMenuItem = (int)selectedMenuItem.WishBooks;                                         // Update the current menu tracker to indicate the wishlist is selected
            WishMainBttn.Focus();                                                                   // and update the sidebar book wishlist menu focused state 
            ViewTitle.Text = "My Book Wishlist";                                                    // Update the title of the view to indicate user is viewing the book wishlist
            currBookCount = currCommChannel.GetLibItemCount(Library.whichLibrary.Wishlist);         
            LibraryBookCount.Text = "Books in Wishlist: " + currBookCount;                          // Update the status of the book total display in the header menu region
        } //END SetInitialContext


        // Outputs the list of items for the current view to the dynamic list area of the GUI view
        public void DisplayList()
        {
            DataRow listRow;

            for(int i = 0; i < currCommChannel.GetDisplayListCount(); i++)
            {
                listRow = collectionListTable.NewRow();
                listRow["ObjUniqueID"] = currCommChannel.GetDisplayListItem(i).GetISBN();
                listRow["ObjCreator"] = currCommChannel.GetDisplayListItem(i).GetAuthorsList();
                listRow["ObjTitle"] = currCommChannel.GetDisplayListItem(i).GetTitle();
                listRow["ObjStarRating"] = currCommChannel.GetDisplayListItem(i).GetStarRating();
                collectionListTable.Rows.Add(listRow);
                //CurrentViewList.Items.Add(currCommChannel.GetDisplayListItem(i));
                //CurrentViewList.ItemsSource = currCommChannel.GetDisplayList();
                //TempListBox.Text += currCommChannel.GetDisplayListItem(i) + Environment.NewLine;
            }

            CurrentViewList.ItemsSource = collectionListTable.DefaultView;
        }//END DisplayList


        // Fills the detail content region of the GUI with item details
        private void UpdateDetailRegion(List<object> bookOfInterest)
        {
            // Update the relevant areas of the detail section with the current book details
            DetailBookTitle.Text = (string)bookOfInterest[0];
            DetailISBN.Text = "ISBN: " + (string)bookOfInterest[1];
            DetailASIN.Text = "ASIN: " + (string)bookOfInterest[2];
            DetailAuthorList.Text = "Author(s): " + (string)bookOfInterest[3];
            DetailNumOfPages.Text = "Number of Pages: " + bookOfInterest[4].ToString();
            DescText.Text = (string)bookOfInterest[5];
            RatingChange(Convert.ToInt32(bookOfInterest[7]));
        }//END UpdateDetailRegion
    }//END MainWindow CLASS
}//END PersonalLibraryApp NAMESPACE
