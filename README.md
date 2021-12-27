# Windows Programming CQ2019/3 - HCMUS
## Team members: 
- 19120268 - Ngô Đặng Gia Lâm
- 19120383 - Huỳnh Tấn Thọ
## Project: Batch Rename
### Notice:
- The directory that contains the .exe file **MUST** have a folder named "rules". Otherwise, the program will **crash/inexecutable**.
- The specified "rules" folder contains all renaming rules for the program to run. If you want to add a rule, simply copy its .dll file and paste it there. Similarly, if you want to remove a rule, just delete its .dll file inside the folder.
- The "rules" folder can be empty, though that way, the program is unable to perform any renaming task.
### Core features
1. Dynamically load all renaming rules from external .dll files.
2. Select all files and folders you want to rename. You can remove files/folders individually after adding.
3. Create a set of rules for renaming the files. Each rule can be added from a menu and its parameters can be edited.
4. Apply the set of rules in numerical order to each file, make them have a new name.
5. Save the set of rules into presets for reuse purposes.
### Renaming Rules
1. Add counter to the start/end of the file, starting from 1. [^1]
2. Add a prefix/suffix to the file's name.
3. Change all files' extensions to another.
4. Remove all space at the beginning and the ending of the filename. [^1]
5. Convert all characters to lowercase and remove all spaces. [^1]
6. Convert filename to PascalCase
7. Replace certain characters/words with another.
    - Input in the first TextBox the word you want to rename. If you want to rename multiple ones, each one **must** be in one line.
    - Input in the second TextBox the result you want to rename to.
    - You can only replace file's name here. To replace certain extensions, navigate to rule 8.
8. Replace certain extensions to another. 
    - The princicle is the same as Replace Rule, but you can only replace the extensions.
9. Convert all characters to uppercase. [^1]
### Improvements
1. Storing parameters for renaming using JSON instead of .txt files
2. Handling files/folders duplication when add them to the list.
3. Let the user see the preview.
4. Presets do not only contain the set of rules, they also store each rule's parameters.
5. Save and load into a project (.proj files), including:
    - Set of current rules and their parameters.
    - List of files/folders, along with their preview and result.
6. Autosave as a temporary project every 15 seconds to quickly continue working in case of power loss or unexpected crashes.
7. Checking exceptions when editing rules
    - Characters that cannot be in filenames: first 32 characters in ASCII table and < > : " / \ | ? *
    - Characters that cannot be in foldernames: AUX, PRN, NUL, CON, COM0, COM1, COM2, COM3, COM4, COM5, COM6, COM7, COM8, COM9, LPT0, LPT1, LPT2, LPT3, LPT4, LPT5, LPT6, LPT7, LPT8, LPT9
    - File's name length cannot exceed 255 characters.
8. Create a copy of all the files and move them to a selected folder **or** rename the original files. This can be configure in the Options menu.
9. Used regular expressions to check:
	- If the name has spaces at the beginning and/or the ending.
	- If the extensions contain spaces or dots when using Change/Replace Extension Rule.
10. Handling conflicts when rename, such as files/folders have the same name. In case of conflicts, notify the users.
11. Using FLuent.Ribbon UI to improve accessibility.
12. Show the result of batch processes. For example: source file not found, duplicated names, success, etc.
13. Handling a lot of exceptions to avoid program crashes.
14. Navigation buttons to quickly move a rule to top/bottom/next/previous.
15. Configurable rules have their own window to edit parameters.
16. New file button to quickly start a new project.
17. Showing confirm message every time the users try to close the program or create a new project.
18. Drag and drop files/folders to the application.

## Project: Graphic Objects
### Core features
### Basic Graphic Objects
### Improvements

[^1]: does not have any parameter to configure
