# Sqlite project

This project is a show case of how to implement a sqlite database inside Unity

The sqlite plugin supports `Insert`, `Delete`, `Modify` , `Query` functions

# Features:

1. Show Data from .db file
2. Add and Update data
    
    ![Insert&Replace.gif](Github%20readme%20fb49c5af247044de9c1134f90fd859b4/InsertReplace.gif)
    
3. Delete data
    
    ![Delete.gif](Github%20readme%20fb49c5af247044de9c1134f90fd859b4/Delete.gif)
    
4. Select data by uuid
    
    ![Select.gif](Github%20readme%20fb49c5af247044de9c1134f90fd859b4/Select.gif)
    
5. Handle data error
    
    ![Error.gif](Github%20readme%20fb49c5af247044de9c1134f90fd859b4/Error.gif)
    

# Getting Started

1. Clone the project and checkout to the `sqlite` branch
2. Open the project with any version of Unity beyond Unity 2017
3. The main behavior of UI logic is in the  `[TableContentFill.cs](https://github.com/Big-Bro222/UsefulUnityLibrary/blob/sqlite/Assets/Scripts/TableContentFill.cs)`
4. Main SQLite commands are in the `[SqlDbCommand.cs](https://github.com/Big-Bro222/UsefulUnityLibrary/blob/sqlite/Assets/Scripts/SqlDbCommand.cs)`
5. To fulfill the SQLite function, the two .dll files in the Plugin Folder are essential

# Note

1. The core part (SqlDbCommand.cs) is derived from my previous work, so the unused function is not guaranteed to work in this project (but it can also be somehow enlightening)
2. Not all sorts of Errors are caught and logged, be careful with that. (Ok I am a bit lazy and catching errors are not the most important point of the whole project )
