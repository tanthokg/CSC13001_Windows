﻿using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RuleHandler;
using System.Windows.Controls;
using System.ComponentModel;

namespace BatchRename
{
    public class SuffixRuleEditor : Window, IRuleEditor
    {
        public IRuleHandler Rule { get; set; }

        private Canvas canvas = new Canvas();
        private Button ok = new Button();
        private Button cancel = new Button();
        private TextBox editTextBox = new TextBox();
        private RuleParameter ruleParameter = new RuleParameter();


        public SuffixRuleEditor(RuleParameter ruleParameter)
        {
            //define UI
            this.Title = "Parameter Editor for Add Suffix Rule";
            this.Width = 400;
            this.Height = 360;
            this.ResizeMode = ResizeMode.NoResize;

            editTextBox.Height = 80;
            editTextBox.Width = 360;
            editTextBox.TextWrapping = TextWrapping.WrapWithOverflow;
            editTextBox.Margin = new Thickness(20, 89, 0, 0);
            editTextBox.Text = ruleParameter.OutputStrings;

            ok.Content = "Submit";
            ok.Name = "buttonSubmit";
            ok.IsDefault = true;
            ok.Click += this.OnSubmitButtonClick;
            ok.Width = 80;
            ok.Height = 35;
            ok.Margin = new Thickness(93, 196, 0, 0);

            cancel.Click += this.OnCancelButtonClick;
            cancel.IsCancel = true;
            cancel.Content = "Cancel";
            cancel.Width = 80;
            cancel.Height = 35;
            cancel.Margin = new Thickness(228, 196, 0, 0);

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
                ruleParameter.OutputStrings = str;
                DialogResult = true;
            }
        }
        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {

        }

        RuleParameter IRuleEditor.GetParameter()
        {
            RuleParameter ruleParameter = new RuleParameter();
            ruleParameter.OutputStrings = editTextBox.Text;

            return ruleParameter;
        }

        bool? IRuleEditor.ShowDialog()
        {
            return this.ShowDialog();
        }
    }

    public class AddSuffixRule : Rule, IRuleHandler
    {

        public AddSuffixRule()
		{
            this.parameter = new RuleParameter();
		}
        public override string ToString()
        {
            return parameter.OutputStrings.Length == 0 ? "Add Suffix" : "Add Suffix: " + parameter.OutputStrings;
        }

        void IRuleHandler.SetParameter(RuleParameter ruleParameter)
        {
            this.parameter = ruleParameter;
        }
        RuleParameter IRuleHandler.GetParameter()
        {
            return this.parameter;
        }

        bool IRuleHandler.IsEditable() { return true; }

        string IRuleHandler.Process(string ObjectName, bool isFileType)
        {

            if (string.IsNullOrEmpty(ObjectName))
                return "";

            string[] parts = ObjectName.Split('.');
            string extension = parts[^1];
            string fileName;
            if (isFileType)
                fileName = string.Join("", parts.SkipLast(1));
            else
                fileName = ObjectName;

            if (isFileType)
                return fileName + this.parameter.OutputStrings + "." + extension;
            return fileName + this.parameter.OutputStrings;
        }
        IRuleEditor IRuleHandler.ParamsEditorWindow()
        {
            return new SuffixRuleEditor(this.parameter);
        }
        IRuleHandler IRuleHandler.Clone()
        {
            AddSuffixRule clone = new AddSuffixRule();

            clone.parameter.InputStrings = this.parameter.InputStrings.Select(x => x.ToString()).ToList();
            clone.parameter.OutputStrings = this.parameter.OutputStrings;
            clone.parameter.Counter = this.parameter.Counter;

            return clone;
        }
    }
}
