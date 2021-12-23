﻿using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RuleHandler;
using System.Windows.Controls;

namespace BatchRename
{
    public class PascalCaseRuleEditor : Window, IRuleEditor
    {
        public IRuleHandler Rule { get; set; }

        private Canvas canvas = new Canvas();
        private Button ok = new Button();
        private Button cancel = new Button();
        private TextBox editTextBox = new TextBox();
        private RuleParameter ruleParameter = new RuleParameter();


        public PascalCaseRuleEditor(RuleParameter ruleParameter)
        {
            //define UI
            this.Title = "Parameter Editor for Pascal Case Rule";
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
                ruleParameter.InputStrings[0] = str;
                DialogResult = true;
            }
        }
        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {

        }

        RuleParameter IRuleEditor.GetParameter()
        {
            RuleParameter ruleParameter = new RuleParameter();
            ruleParameter.InputStrings[0] = editTextBox.Text;

            return ruleParameter;
        }

        bool? IRuleEditor.ShowDialog()
        {
            return this.ShowDialog();
        }
    }
    public class PascalCaseRule : Rule, IRuleHandler
    {
        private string upperCaseFirstLetter(string str)
        {
            if (str.Length == 0)
                return str;
            return str.ToUpper()[0] + str.Substring(1);
        }

        public override string ToString()
        {
            return "PascalCase";
        }
        bool IRuleHandler.IsEditable() { return true; }

        void IRuleHandler.SetParameter(RuleParameter ruleParemeters)
        {
            this.parameter = ruleParemeters;
        }
        RuleParameter IRuleHandler.GetParameter()
        {
            return this.parameter;
        }

        string IRuleHandler.Process(string ObjectName, bool isFileType)
        {
            string[] parts = ObjectName.Split('.');
            string extension = parts[^1];
            string fileName;
            if (isFileType)
                fileName = string.Join("", parts.SkipLast(1));
            else
                fileName = ObjectName;

            string[] result = fileName.Split(this.parameter.InputStrings[0]);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (string str in result)
            {
                stringBuilder.Append(upperCaseFirstLetter(str));
            }

            if (isFileType)
                return stringBuilder.ToString() + "." + extension;
            return stringBuilder.ToString();
        }
        IRuleEditor IRuleHandler.ParamsEditorWindow()
        {
            return new PascalCaseRuleEditor(parameter);
        }

        IRuleHandler IRuleHandler.Clone()
        {
            PascalCaseRule clone = new PascalCaseRule();
            clone.parameter.InputStrings = this.parameter.InputStrings.Select(x => x.ToString()).ToList();
            clone.parameter.OutputStrings = this.parameter.OutputStrings;
            clone.parameter.Counter = this.parameter.Counter;
            return clone;
        }
    }
}