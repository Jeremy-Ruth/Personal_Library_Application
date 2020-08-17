# Personal_Library_Application

This desktop application will provide a way to store, work with, and add to a personal book library. The primary features of the app to be developed are:

* **GUI** - An easy to understand user interface with 4 main persistent sections: App menu, library menu, Detail view pane, and book list.
* **Google Books API** - To facilitate easily finding books the app will make use of the Google Books API to grab and store details.
* **Manual Data Entry** - Allow the user to add details of a book manually when needed or to edit existing book data
* **Wishlist** - Store books that a user desires to own and keep the list separate from owned books for easier management
* **Series** - Add books to a series such as a trilogy. There will be a series view menu item, but books will also show if they are they are associated with a series within the individual book details
* **Star Rating** - Assign a rating to a book in a typical manner

Once the features have been built and work well for books, I plan to investigate adding the ability to create a movie library as well. This will depend on a few factors. Primarily if movies have a unique identifying number like books (or if something unique can be derived from a subset of movie data), and whether a decent API for movies is available.

## Getting Started / Installation

This section will be developed further once the program is in a more complete state. For the current version the source code files are available in the [src](https://github.com/Jeremy-Ruth/Personal_Library_Application/tree/master/src) folder. These files can be imported into a project of your favorite C# IDE (such as Visual Studio) and you should be able to compile and work with the program as is.

The program uses SQLite as a database which should work for the app without a need to install any related software. If you would like to modify the database structure (or in the current version even to add new entries in the database) it will be necessary to install, at the least, the [SQLite command line tool](https://sqlite.org/cli.html) or a GUI such as [SQLite Studio](https://sqlitestudio.pl/).

## Prerequisites 

You will need an IDE that can compile C# code in order to work with the source code. Nothing else should be needed at this time as the other files are self-contained in the included folders.

## Testing

No testing has been developed as of yet beyond some inline snippets as needed for the current work.

## Version History

Pre-Release state

**Completed Tasks:**
* Primary GUI structure
* Basic testing and implementation for data population of the GUI from the database

**Next Tasks:**
* Filtering and sorting functionality of the list view area
* Manual entry window

## Author

**Jeremy Ruth**

## License

This project is licensed under the Creative Commons Zero v1.0 Universal license. See [License.md](https://github.com/Jeremy-Ruth/Personal_Library_Application/blob/master/LICENSE) for more details.
