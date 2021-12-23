
using ruleHandler;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        private BindingList<IRuleHandler> rules;
        //temp
        private BindingList<IRuleHandler> chosenRules;
        private BindingList<string> itemTypes;
        private BindingList<string> conflictActions;
        private BindingList<Filename> filenames;
        private BindingList<Foldername> foldernames;

        private Dictionary<string, List<Filename>> conflictFiles = new Dictionary<string, List<Filename>>();
        private Dictionary<string, List<Foldername>> conflictFolders = new Dictionary<string, List<Foldername>>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Initiate Data Lists/Collections
            List<IRuleHandler> ruleHandlers = new List<IRuleHandler>();
            ruleHandlers.Add(new Rule_Replace());
            ruleHandlers.Add(new Rule_lowercase());
            ruleHandlers.Add(new Rule_uppercase());
            ruleHandlers.Add(new Rule_PascalCase());
            ruleHandlers.Add(new Rule_improperSpaces());
            ruleHandlers.Add(new Rule_addPrefix());
            ruleHandlers.Add(new Rule_addSuffix());

            rules = new BindingList<IRuleHandler>();

            ruleHandlers.ForEach(E =>
            {
                rules.Add(E);
            });

            //itemTypes
            itemTypes = new BindingList<string>()
            {
                "File", "Folder"
            };
            chosenRules = new BindingList<IRuleHandler>();
            conflictActions = new BindingList<string>()
            {
                "Stop batching",
                "Add a number as prefix",
                "Add a number as suffix",
                "Add created date as suffix"
            };
            filenames = new BindingList<Filename>();
            foldernames = new BindingList<Foldername>();

            // Bind UI with lists/collections
            rulesComboxBox.ItemsSource = rules;
            typeComboBox.ItemsSource = itemTypes;
            conflictComboBox.ItemsSource = conflictActions;

            chosenListView.ItemsSource = chosenRules;
        }

        private void AddRules(object sender, RoutedEventArgs e)
        {
            int index = rulesComboxBox.SelectedIndex;
            if (index != -1)
            {
                chosenRules.Add(rules[index]);
            }
        }
        private void EditChosenFromList(object sender, RoutedEventArgs e)
        {
            int index = chosenListView.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("Invalid rule.");
                return;
            }
        }
        private void RemoveChosenFromList(object sender, RoutedEventArgs e)
        {
            int index = chosenListView.SelectedIndex;
            if (index != -1)
                chosenRules.RemoveAt(index);
        }

        private void ResetChosenRules(object sender, RoutedEventArgs e)
        {
            chosenRules.Clear();
            conflictFiles.Clear();
            conflictFolders.Clear();
        }

        private void MoveRuleToTop(object sender, RoutedEventArgs e)
        {
            int index = chosenListView.SelectedIndex;
            if (index != -1)
            {
                IRuleHandler temp = chosenRules[index];
                for (int i = index; i > 0; --i)
                    chosenRules[i] = chosenRules[i - 1];
                chosenRules[0] = temp;
            }
        }

        private void MoveRuleToPrev(object sender, RoutedEventArgs e)
        {
            int index = chosenListView.SelectedIndex;
            if (index != -1 && index != 0)
            {
                IRuleHandler temp = chosenRules[index - 1];
                chosenRules[index - 1] = chosenRules[index];
                chosenRules[index] = temp;
            }
        }

        private void MoveRuleToNext(object sender, RoutedEventArgs e)
        {
            int index = chosenListView.SelectedIndex;
            if (index != -1 && index != chosenRules.Count - 1)
            {
                IRuleHandler temp = chosenRules[index + 1];
                chosenRules[index + 1] = chosenRules[index];
                chosenRules[index] = temp;
            }
        }

        private void MoveRuleToLow(object sender, RoutedEventArgs e)
        {
            int index = chosenListView.SelectedIndex;
            if (index != -1)
            {
                IRuleHandler temp = chosenRules[index];
                for (int i = index; i < chosenRules.Count - 1; ++i)
                    chosenRules[i] = chosenRules[i + 1];
                chosenRules[chosenRules.Count - 1] = temp;
            }
        }

        private void AddItems(object sender, RoutedEventArgs e)
        {
            if (typeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please choose item type (files or folders)", "Error");
                return;
            }
            if (typeComboBox.SelectedItem.ToString() == "File")
            {

                /*typeComboBox.IsEnabled = false;
                System.Windows.Forms.FolderBrowserDialog explorerDialog = new System.Windows.Forms.FolderBrowserDialog();

                System.Windows.Forms.DialogResult result = explorerDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    ItemListView.ItemsSource = filenames;

                    string path = explorerDialog.SelectedPath + "\\";
                    string[] files = Directory.GetFiles(path);

                    foreach (var file in files)
                    {
                        string filename = file.Remove(0, path.Length);
                        filenames.Add(new Filename() { CurrentName = filename, Path = path });
                    }


                    MessageBox.Show(filenames.Count + " file(s) Added Successfully");

                    MessageBox.Show(filenames.Count + " file(s) Added Successfully", "Success");
                }*/
                typeComboBox.IsEnabled = false;
                ItemListView.ItemsSource = filenames;

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    string[] files = openFileDialog.FileNames;
                    foreach (string file in files)
                    {
                        string currentName = Path.GetFileName(file);
                        string directoryPath = Path.GetDirectoryName(file);
                        filenames.Add(new Filename() { CurrentName = currentName, Path = directoryPath });
                    }
                }

                MessageBox.Show(filenames.Count + " file(s) Added Successfully", "Success");

            }
            else if (typeComboBox.SelectedItem.ToString() == "Folder")
            {
                System.Windows.Forms.FolderBrowserDialog explorerDialog = new System.Windows.Forms.FolderBrowserDialog();
                System.Windows.Forms.DialogResult result = explorerDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    ItemListView.ItemsSource = foldernames;

                    string path = explorerDialog.SelectedPath + "\\";
                    string[] folders = Directory.GetDirectories(path);

                    foreach (var folder in folders)
                    {
                        string foldername = folder.Remove(0, path.Length);
                        foldernames.Add(new Foldername() { CurrentName = foldername, Path = path });
                    }

                    MessageBox.Show(foldernames.Count + " folder(s) Added Successfully", "Success");
                }
            }
        }

        private void ResetAddedItems(object sender, RoutedEventArgs e)
        {
            filenames.Clear();
            foldernames.Clear();
            conflictFiles.Clear();
            conflictFolders.Clear();
        }

        private void StartProcess(object sender, RoutedEventArgs e)
        {
            if (chosenRules.Count == 0)
            {
                MessageBox.Show("Process skipped because there is no rule set");
                return;
            }

            if (filenames.Count == 0 && foldernames.Count == 0)
            {
                MessageBox.Show("Process skipped because there is chosed file(s)/folder(s)");
                return;
            }

            this.conflictFiles.Clear();
            this.conflictFolders.Clear();

            foreach (Filename file in filenames)
            {
                file.NewName = file.CurrentName;
                foreach (IRuleHandler handler in chosenRules)
                {
                    file.NewName = handler.process(file.NewName);
                }

                if (!this.conflictFiles.ContainsKey(file.NewName))
                    this.conflictFiles.Add(file.NewName, new List<Filename> { file });
                else
                    this.conflictFiles[file.NewName].Add(file);
            }

            foreach (Foldername folder in foldernames)
            {
                folder.NewName = folder.CurrentName;
                foreach (IRuleHandler handler in chosenRules)
                {
                    folder.NewName = handler.process(folder.NewName, false);
                }

                if (!this.conflictFolders.ContainsKey(folder.CurrentName))
                    this.conflictFolders.Add(folder.NewName, new List<Foldername> { folder });
                else
                    this.conflictFolders[folder.NewName].Add(folder);
            }

            bool isOccurConflict = false;

            foreach (var (key, value) in this.conflictFiles)
            {
                if (value.Count > 1)
                {
                    isOccurConflict = true;
                    value.ForEach(e =>
                    {
                        e.Result = "Conflicted";
                    });
                }
            }

            foreach (var (key, value) in this.conflictFolders)
            {
                if (value.Count > 1)
                {
                    isOccurConflict = true;
                    value.ForEach(e =>
                    {
                        e.Result = "Conflicted";
                    });
                }
            }

            if (isOccurConflict)
            {
                MessageBox.Show("Error: There will be some files/folders have the same name at the end of the process, consider to add conflict resolver or change rule set and try again");
            }
        }

        private void PreviewProcess(object sender, RoutedEventArgs e)
        {
            if (chosenRules.Count == 0)
            {
                MessageBox.Show("Process skipped because there is no rule set");
                return;
            }

            if (filenames.Count == 0 && foldernames.Count == 0)
            {
                MessageBox.Show("Process skipped because there is chosed file(s)/folder(s)");
                return;
            }

            this.conflictFiles.Clear();
            this.conflictFolders.Clear();

            foreach (Filename file in filenames)
            {
                file.NewName = file.CurrentName;
                foreach (IRuleHandler handler in chosenRules)
                {
                    file.NewName = handler.process(file.NewName);
                }

                if (!this.conflictFiles.ContainsKey(file.NewName))
                    this.conflictFiles.Add(file.NewName, new List<Filename> { file });
                else
                    this.conflictFiles[file.NewName].Add(file);
            }

            foreach (Foldername folder in foldernames)
            {
                folder.NewName = folder.CurrentName;
                foreach (IRuleHandler handler in chosenRules)
                {
                    folder.NewName = handler.process(folder.NewName, false);
                }

                if (!this.conflictFolders.ContainsKey(folder.CurrentName))
                    this.conflictFolders.Add(folder.NewName, new List<Foldername> { folder });
                else
                    this.conflictFolders[folder.NewName].Add(folder);
            }

            bool isOccurConflict = false;

            foreach (var (key, value) in this.conflictFiles)
            {
                if (value.Count > 1)
                {
                    isOccurConflict = true;
                    value.ForEach(e =>
                    {
                        e.Result = "Conflicted";
                    });
                }
            }

            foreach (var (key, value) in this.conflictFolders)
            {
                if (value.Count > 1)
                {
                    isOccurConflict = true;
                    value.ForEach(e =>
                    {
                        e.Result = "Conflicted";
                    });
                }
            }

            if (isOccurConflict)
            {
                MessageBox.Show("There are some files/folders have the same name at the end of the process, consider to add conflict resolver or change rule set");
            }
        }
    }
}
