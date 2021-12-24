
using RuleHandler;
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
using System.Text.Json;

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        private BindingList<IRuleHandler> rules;
        private BindingList<IRuleHandler> chosenRules;
        private BindingList<string> itemTypes;
        private BindingList<IRuleHandler> conflictActions;
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
            ruleHandlers.Add(new ReplaceRule());
            ruleHandlers.Add(new LowercaseRule());
            ruleHandlers.Add(new UppercaseRule());
            ruleHandlers.Add(new PascalCaseRule());
            ruleHandlers.Add(new ImproperSpacesRule());
            ruleHandlers.Add(new AddPrefixRule());
            ruleHandlers.Add(new AddSuffixRule());
            ruleHandlers.Add(new AddSuffixCounterRule());
            ruleHandlers.Add(new AddPrefixCounterRule());
            ruleHandlers.Add(new ChangeExtensionRule());

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
            conflictActions = new BindingList<IRuleHandler>()
            {
                new AddPrefixCounterRule(),
                new AddSuffixCounterRule()
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
                chosenRules.Add(rules[index].Clone());
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

            IRuleHandler rule = chosenRules[index];
            if (rule.IsEditable())
            {
                IRuleEditor editWindow = rule.ParamsEditorWindow();
                if (editWindow.ShowDialog() == true)
                    chosenRules[index].SetParameter(editWindow.GetParameter());
            }
            else
            {
                MessageBox.Show("This rule does not have any parameter to edit", "Error");
            }
            ICollectionView view = CollectionViewSource.GetDefaultView(chosenRules);
            view.Refresh();

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
                ItemListView.ItemsSource = filenames;

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "All files (*.*)|*.*";
                int counter = 0;
                if (openFileDialog.ShowDialog() == true)
                {
                    string[] files = openFileDialog.FileNames;
                    List<Filename> newFilenames = new List<Filename>();

                    foreach (string file in files)
                    {
                        bool isExisted = false;
                        string currentName = Path.GetFileName(file);
                        string directoryPath = Path.GetDirectoryName(file);

                        foreach (var filename in filenames)
                        {
                            if (filename.CurrentName == currentName && filename.Path == directoryPath)
                            {
                                isExisted = true;
                                break;
                            }
                        }

                        if (!isExisted)
                        {
                            newFilenames.Add(new Filename() { CurrentName = currentName, Path = directoryPath });
                            counter++;
                        }
                    }

                    foreach (var newFilename in newFilenames)
                        filenames.Add(newFilename);
                }

                MessageBox.Show(counter + " file(s) Added Successfully", "Success");
            }
            else if (typeComboBox.SelectedItem.ToString() == "Folder")
            {
                System.Windows.Forms.FolderBrowserDialog explorerDialog = new System.Windows.Forms.FolderBrowserDialog();
                System.Windows.Forms.DialogResult result = explorerDialog.ShowDialog();
                
                int counter = 0;
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    ItemListView.ItemsSource = foldernames;

                    string path = explorerDialog.SelectedPath + "\\";
                    string[] folders = Directory.GetDirectories(path);
                    List<Foldername> newFoldernames = new List<Foldername>();

                    foreach (var folder in folders)
                    {
                        bool isExisted = false;
                        string currentName = folder.Remove(0, path.Length);

                        foreach (var foldername in foldernames)
                        {
                            if (foldername.CurrentName == currentName && foldername.Path == path)
                            {
                                isExisted = true;
                                break;
                            }
                        }

                        if (!isExisted)
                        {
                            newFoldernames.Add(new Foldername() { CurrentName = currentName, Path = path });
                            counter++;
                        }
                    }
                    foreach (var newFoldername in newFoldernames)
                        foldernames.Add(newFoldername);

                    MessageBox.Show(counter + " folder(s) Added Successfully", "Success");
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
                MessageBox.Show("Process skipped because there is no rule set", "Process aborted");
                return;
            }

            if (filenames.Count == 0 && foldernames.Count == 0)
            {
                MessageBox.Show("Process skipped because there is chosed file(s)/folder(s)", "Process aborted");
                return;
            }

            this.conflictFiles.Clear();
            this.conflictFolders.Clear();

            List<IRuleHandler> ruleSetForFiles = new List<IRuleHandler>();
            List<IRuleHandler> ruleSetForFolders = new List<IRuleHandler>();

            foreach (IRuleHandler rule in this.chosenRules)
            {
                ruleSetForFiles.Add(rule.Clone());
                ruleSetForFolders.Add(rule.Clone());
            }

            foreach (Filename file in filenames)
            {
                file.NewName = file.CurrentName;
                foreach (IRuleHandler handler in ruleSetForFiles)
                {
                    file.NewName = handler.Process(file.NewName);
                }

                if (!this.conflictFiles.ContainsKey(file.NewName))
                    this.conflictFiles.Add(file.NewName, new List<Filename> { file });
                else
                    this.conflictFiles[file.NewName].Add(file);
            }

            foreach (Foldername folder in foldernames)
            {
                folder.NewName = folder.CurrentName;
                foreach (IRuleHandler handler in ruleSetForFolders)
                {
                    folder.NewName = handler.Process(folder.NewName, false);
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

            int index = this.conflictComboBox.SelectedIndex;

            if (isOccurConflict && index == -1)
            {
                MessageBox.Show("There will be some files/folders have the same name at the end of the process, consider to add conflict resolver or change rule set and try again", "Process aborted");
                return;
            }

            //resolve conflict if selected a conflict resolver
            foreach (var (key, value) in this.conflictFiles)
            {
                if (value.Count > 1)
                {
                    IRuleHandler resolver = conflictActions[index].Clone();
                    value.ForEach(e =>
                    {
                        e.NewName = resolver.Process(e.NewName);
                    });
                }
            }

            foreach (var (key, value) in this.conflictFolders)
            {
                if (value.Count > 1)
                {
                    IRuleHandler resolver = conflictActions[index].Clone();
                    value.ForEach(e =>
                    {
                        e.NewName = resolver.Process(e.NewName);
                    });
                }
            }

            //process

            int folderCounter = 0;
            int fileCounter = 0;

            foreach (Filename file in filenames)
            {
                try
				{
                    File.Move(file.Path +  "/"  + file.CurrentName, file.Path +  "/"  + file.NewName);
                    file.CurrentName = file.NewName;
				}
                catch (FileNotFoundException exception){
                    file.Result = "Source file not exist";
                    continue; 
                }
                fileCounter++;
                file.Result = "Success";
            }

            foreach (Foldername folder in foldernames)
            {
                try
                {
                    Directory.Move(folder.Path + "/"  + folder.CurrentName, folder.Path +  "/"  + folder.NewName);
                    folder.CurrentName = folder.NewName;
                }
                catch (DirectoryNotFoundException exception) { 
                    folder.Result = "Source directory not found";
                    continue;
                }
                folderCounter++;
                folder.Result = "Success";
            }

            MessageBox.Show($"Result\n   Type file: {fileCounter}/{filenames.Count} success\n   Type folder: {folderCounter}/{foldernames.Count} success", "Process done");
        }

        private void PreviewProcess(object sender, RoutedEventArgs e)
        {
            if (chosenRules.Count == 0)
            {
                MessageBox.Show("Process skipped because there is no rule set", "Process aborted");
                return;
            }

            if (filenames.Count == 0 && foldernames.Count == 0)
            {
                MessageBox.Show("Process skipped because there is no selected file(s)/folder(s)", "Process aborted");
                return;
            }

            this.conflictFiles.Clear();
            this.conflictFolders.Clear();

            List<IRuleHandler> ruleSetForFiles = new List<IRuleHandler>();
            List<IRuleHandler> ruleSetForFolders = new List<IRuleHandler>();

            foreach (IRuleHandler rule in this.chosenRules)
            {
                ruleSetForFiles.Add(rule.Clone());
                ruleSetForFolders.Add(rule.Clone());
            }

            foreach (Filename file in filenames)
            {
                file.NewName = file.CurrentName;
                foreach (IRuleHandler handler in ruleSetForFiles)
                {
                    file.NewName = handler.Process(file.NewName);
                }

                if (!this.conflictFiles.ContainsKey(file.NewName))
                    this.conflictFiles.Add(file.NewName, new List<Filename> { file });
                else
                    this.conflictFiles[file.NewName].Add(file);
            }

            foreach (Foldername folder in foldernames)
            {
                folder.NewName = folder.CurrentName;
                foreach (IRuleHandler handler in ruleSetForFolders)
                {
                    folder.NewName = handler.Process(folder.NewName, false);
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
                MessageBox.Show("There are some files/folders have the same name at the end of the process, consider to add conflict resolver or change rule set", "Caution");
            }
        }

		private void SaveRulesToJson(object sender, RoutedEventArgs e) {
            if(chosenRules.Count == 0)
			{
                MessageBox.Show("There are no selected rule to save.");
                return;
			}

            string outputName = "save.json";
            StreamWriter output;
            try
			{
                output = new StreamWriter(outputName);
                output.WriteLine("[");
                foreach (var rule in chosenRules)
			    {
                    output.Write(rule.ToJson());
                    if (chosenRules.IndexOf(rule) != chosenRules.Count - 1)
                        output.WriteLine(",");
			    }
                output.Write("\n]");
                output.Close();
                MessageBox.Show("Done");
			}
            catch (System.IO.IOException ioe)
			{
                MessageBox.Show("Error!");
                return;
			}
		}

		private void LoadRulesFromJson(object sender, RoutedEventArgs e)
		{
            this.chosenRules.Clear();
            string inputName = "save.json";
            string content = File.ReadAllText(inputName);

            List<RuleJsonFormat> ruleJsons = new List<RuleJsonFormat>();

			try
			{
               ruleJsons = JsonSerializer.Deserialize<List<RuleJsonFormat>>(content);
			}
            catch (System.Text.Json.JsonException exception)
			{
                MessageBox.Show("Cannot parse data from the file, check the file again", "Error");
                return;
			}

            foreach(RuleJsonFormat ruleJson in ruleJsons)
			{
                IRuleHandler item = rules.SingleOrDefault(Item => Item.GetRuleType().Equals(ruleJson.RuleType));
                if(item != null)
				{
                    IRuleHandler target = item.Clone();
                    target.SetParameter(new RuleParameter
                    {
                        InputStrings = ruleJson.InputStrings,
                        OutputStrings = ruleJson.OutputStrings,
                        Counter = ruleJson.Counter,
                    });
                    this.chosenRules.Add(target);
				}
			}
		}
	}
}
