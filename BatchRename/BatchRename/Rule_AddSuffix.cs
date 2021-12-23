using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ruleHandler;
using System.Windows.Controls;

namespace BatchRename
{
    class addPreffixParemetor : Window, IRuleEditor
	{

        public IRuleHandler Rule { get; set; }

        private Canvas canvas = new Canvas();
        private Button ok = new Button();
        private Button cancel = new Button();
        private TextBox editTextBox = new TextBox();
        private ruleParemeters ruleParemeter = new ruleParemeters();
        public addPreffixParemetor(ruleParemeters ruleParemeters)
        {
            //define UI
            this.Title = "Parameter Editor";
            this.Width = 400;
            this.Height = 360;
            this.ResizeMode = ResizeMode.NoResize;

            editTextBox.Height = 80;
            editTextBox.Width = 360;
            editTextBox.TextWrapping = TextWrapping.WrapWithOverflow;
            editTextBox.Margin = new Thickness(20,89,0,0);
            editTextBox.Text = ruleParemeters.outputStrings;

            ok.Content = "Submit";
            ok.Name = "buttonSubmit";
            ok.IsDefault = true;
            ok.Click += this.OnSubmitButtonClick;
            ok.Width = 80;
            ok.Height = 35;
            ok.Margin = new Thickness(93,196,0,0);

            cancel.Click += this.OnCancelButtonClick;
            cancel.IsCancel = true;
            cancel.Content = "Cancel";
            cancel.Width = 80;
            cancel.Height = 35;
            cancel.Margin = new Thickness(228,196,0,0);

            canvas.Children.Add(editTextBox);
            canvas.Children.Add(ok);
            canvas.Children.Add(cancel);

            this.AddChild(canvas);
        }
        private void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            string str = editTextBox.Text;
            if (str.Length != 0)
            {
                DialogResult = true;
            }
        }
        private void OnCancelButtonClick(object sender, RoutedEventArgs e) { 
            
        }

        ruleParemeters IRuleEditor.GetParameters()
		{
            ruleParemeters ruleParemeters = new ruleParemeters();
            ruleParemeters.outputStrings = editTextBox.Text;

            return ruleParemeters;
		}
        
        bool? IRuleEditor.showDialog()
		{
			return this.ShowDialog();
		}
    }
	
    class Rule_addSuffix : Rule, IRuleHandler
    {
        public override string ToString()
        {
            if (paremeters.outputStrings.Length == 0)
                return "Add Suffix";
            return "Add Suffix: " + paremeters.outputStrings;
        }

        void IRuleHandler.setParameter(ruleParemeters ruleParemeters)
        {
            this.paremeters = ruleParemeters;
        }
        ruleParemeters IRuleHandler.GetParemeters()
		{
            return this.paremeters;
		}

        bool IRuleHandler.isEditable() { return true; }

        string IRuleHandler.process(string ObjectName, bool isFileType)
        {
            string[] parts = ObjectName.Split('.');
            string extension = parts[^1];
            string fileName;
            if (isFileType)
                fileName = string.Join("", parts.SkipLast(1));
            else
                fileName = ObjectName;

            if (isFileType)
                return fileName + this.paremeters.outputStrings + "." + extension;
            return fileName + this.paremeters.outputStrings;
        }
        IRuleEditor IRuleHandler.parametersEditorWindow()
		{
            return new addPreffixParemetor(this.paremeters);
		}
        IRuleHandler IRuleHandler.getClone()
		{
            Rule_addSuffix clone = new Rule_addSuffix();

            clone.paremeters.inputStrings = this.paremeters.inputStrings.Select(x => x.ToString()).ToList();
            clone.paremeters.outputStrings = this.paremeters.outputStrings;
            clone.paremeters.counter = this.paremeters.counter;

            return clone;
		}
    }
}
