﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RuleHandler;

namespace BatchRename
{
    public class ChangeExtensionRuleEditor : Window, IRuleEditor
    {
        public IRuleHandler Rule { get; set; }

        /*private Canvas canvas = new Canvas();
        private Button ok = new Button();
        private Button cancel = new Button();
        private TextBox editTxtBox = new TextBox();

        public ChangeExtensionRuleEditor(RuleParameter ruleParameter)
        {
            //define UI
            this.Title = "Parameter Editor for Change Extension Rule";
            this.Width = 400;
            this.Height = 360;
            this.ResizeMode = ResizeMode.NoResize;

            editTxtBox.Height = 80;
            editTxtBox.Width = 360;
            editTxtBox.TextWrapping = TextWrapping.WrapWithOverflow;
            editTxtBox.Margin = new Thickness(20, 89, 0, 0);
            editTxtBox.Text = ruleParameter.OutputStrings;

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

            canvas.Children.Add(editTxtBox);
            canvas.Children.Add(ok);
            canvas.Children.Add(cancel);

            this.AddChild(canvas);
        }*/

        private Canvas canvas = new Canvas();
        private Label label = new Label();
        private Button submitBtn = new Button();
        private Button cancelBtn = new Button();
        private TextBox editTxtBox = new TextBox();
        private RuleParameter ruleParameter = new RuleParameter();


        public ChangeExtensionRuleEditor (RuleParameter ruleParameter)
        {
            // Define UI
            this.Title = "Parameter Editor for Change Extension Rule";
            this.Width = 415;
            this.Height = 235;
            this.ResizeMode = ResizeMode.NoResize;

            label.Content = "Please type extension you want to rename to";
            label.Margin = new Thickness(20, 10, 0, 0);
            label.FontSize = 16;

            editTxtBox.Width = 360;
            editTxtBox.Height = 80;
            editTxtBox.TextWrapping = TextWrapping.WrapWithOverflow;
            editTxtBox.Margin = new Thickness(20, 50, 0, 0);
            editTxtBox.Text = ruleParameter.OutputStrings;

            submitBtn.Content = "Submit";
            submitBtn.Name = "buttonSubmit";
            submitBtn.IsDefault = true;
            submitBtn.Click += this.OnSubmitButtonClick;
            submitBtn.Width = 170;
            submitBtn.Height = 40;
            submitBtn.Margin = new Thickness(20, 145, 0, 0);
            submitBtn.FontSize = 15;

            cancelBtn.Click += this.OnCancelButtonClick;
            cancelBtn.IsCancel = true;
            cancelBtn.Content = "Cancel";
            cancelBtn.Width = 170;
            cancelBtn.Height = 40;
            cancelBtn.Margin = new Thickness(210, 145, 0, 0);
            cancelBtn.FontSize = 15;

            canvas.Children.Add(label);
            canvas.Children.Add(editTxtBox);
            canvas.Children.Add(submitBtn);
            canvas.Children.Add(cancelBtn);

            this.AddChild(canvas);
        }
        private void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            string str = editTxtBox.Text;
            if (str.Length != 0)
            {
                DialogResult = true;
            }
        }
        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {

        }

        RuleParameter IRuleEditor.GetParameter()
        {
            RuleParameter ruleParameter = new RuleParameter();
            ruleParameter.OutputStrings = editTxtBox.Text;
            return ruleParameter;
        }

        bool? IRuleEditor.ShowDialog()
        {
            return this.ShowDialog();
        }
    }
	class ChangeExtensionRule : Rule, IRuleHandler
	{
        public ChangeExtensionRule()
		{
            this.parameter = new RuleParameter();
		}

        string IRuleHandler.GetRuleType()
		{
            return "ChangeExtensionRule";
		}
        public override string ToString()
        {
            return this.parameter.OutputStrings.Length == 0 ? "Change file\'s extension" : "Change all file\'s extension to: " + this.parameter.OutputStrings;
        }
        
        bool IRuleHandler.IsEditable()
        {
            return true;
        }

        void IRuleHandler.SetParameter(RuleParameter ruleParameter)
		{
            this.parameter = ruleParameter;
		}

        string IRuleHandler.Process(string ObjectName, bool isFileType)
        {
            if (string.IsNullOrEmpty(ObjectName))
                return "";

            string[] parts = ObjectName.Split('.');
            string extension = parts[^1];
            string fileName;
            if (isFileType)
                fileName = string.Join(".", parts.SkipLast(1));
            else
                fileName = ObjectName;

            string result = fileName;
            if (isFileType && !string.IsNullOrEmpty(this.parameter.OutputStrings))
                return result + "." + this.parameter.OutputStrings;
            return result;
        }
        IRuleEditor IRuleHandler.ParamsEditorWindow()
        {
            return new ChangeExtensionRuleEditor(this.parameter);
        }
        RuleParameter IRuleHandler.GetParameter()
        {
            return this.parameter;
        }
        IRuleHandler IRuleHandler.Clone()
        {
            ChangeExtensionRule clone = new ChangeExtensionRule();
            clone.parameter.InputStrings = this.parameter.InputStrings.Select(x => x.ToString()).ToList();
            clone.parameter.OutputStrings = this.parameter.OutputStrings;
            clone.parameter.Counter = this.parameter.Counter;
            return clone;
        }
	} 
}
