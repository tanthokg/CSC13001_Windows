﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        private BindingList<string> rules;
        private BindingList<string> chosenRules;
        private BindingList<string> itemTypes;
        private BindingList<string> conflictActions;
        private BindingList<Filename> filenames;
        private BindingList<Foldername> foldernames;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Initiate Data Lists/Collections
            rules = new BindingList<string>() {
                "Replace", "UPPERCASE", "lowercase", "PascalCase", "Add Prefix", "Add Suffix"
            };
            itemTypes = new BindingList<string>()
            {
                "File", "Folder"
            };
            chosenRules = new BindingList<string>();
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

        private void RemoveChosenFromList(object sender, RoutedEventArgs e)
        {
            int index = chosenListView.SelectedIndex;
            if (index != -1)
                chosenRules.RemoveAt(index);
        }

        private void ResetChosenRules(object sender, RoutedEventArgs e)
        {
            chosenRules.Clear();
        }

        private void MoveRuleToTop(object sender, RoutedEventArgs e)
        {
            int index = chosenListView.SelectedIndex;
            if (index != -1)
            {
                string temp = chosenRules[index];
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
                string temp = chosenRules[index - 1];
                chosenRules[index - 1] = chosenRules[index];
                chosenRules[index] = temp;
            }
        }

        private void MoveRuleToNext(object sender, RoutedEventArgs e)
        {
            int index = chosenListView.SelectedIndex;
            if (index != -1 && index != chosenRules.Count - 1)
            {
                string temp = chosenRules[index + 1];
                chosenRules[index + 1] = chosenRules[index];
                chosenRules[index] = temp;
            }
        }

        private void MoveRuleToLow(object sender, RoutedEventArgs e)
        {
            int index = chosenListView.SelectedIndex;
            if (index != -1)
            {
                string temp = chosenRules[index];
                for (int i = index; i < chosenRules.Count - 1; ++i)
                    chosenRules[i] = chosenRules[i + 1];
                chosenRules[chosenRules.Count - 1] = temp;
            }
        }

        private void AddItems(object sender, RoutedEventArgs e)
        {
            if (typeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please choose item type (files or folders)");
                return;
            }
            if (typeComboBox.SelectedItem.ToString() == "File")
            {
                typeComboBox.IsEnabled = false;
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
                }

            }
            else if (typeComboBox.SelectedItem.ToString() == "Folder")
            {
                typeComboBox.IsEnabled = false;

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

                    MessageBox.Show(foldernames.Count + " folder(s) Added Successfully");
                }
            }
        }

        private void ResetAddedItems(object sender, RoutedEventArgs e)
        {
            filenames.Clear();
            foldernames.Clear();
            typeComboBox.IsEnabled = true;
        }
    }
}
