using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RuleHandler;
using System.Windows.Controls;
using System.Text.Json;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace BatchRename
{
    public class PrefixRuleEditor : Window, IRuleEditor
    {
        public IRuleHandler Rule { get; set; }

        private Canvas canvas = new Canvas();
        private Label label = new Label();
        private Button submitBtn = new Button();
        private Button cancelBtn = new Button();
        private TextBox editTxtBox = new TextBox();
        private RuleParameter ruleParameter = new RuleParameter();

        public PrefixRuleEditor(RuleParameter ruleParameter)
        {
            // Define UI
            this.Title = "Parameter Editor for Add Prefix Rule";
            this.Width = 415;
            this.Height = 235;
            this.ResizeMode = ResizeMode.NoResize;


            label.Content = "Please type characters you want to add as prefix";
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
            ruleParameter.OutputStrings = editTxtBox.Text;

            return ruleParameter;
        }

        bool? IRuleEditor.ShowDialog()
        {
            return this.ShowDialog();
        }
    }
    public class AddPrefixRule : Rule, IRuleHandler
    {
        public AddPrefixRule()
		{
            this.parameter = new RuleParameter();
		}

        public override string ToString()
        {
            return parameter.OutputStrings.Length == 0 ? "Add Prefix" : "Add Prefix: " + parameter.OutputStrings;
        }
        string IRuleHandler.GetRuleType()
		{
            return "AddPrefixRule";
		}
        void IRuleHandler.SetParameter(RuleParameter paremeters)
        {
            this.parameter = paremeters;
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
                fileName = string.Join(".", parts.SkipLast(1));
            else
                fileName = ObjectName;

            if (isFileType)
                return this.parameter.OutputStrings + fileName + "." + extension;
            return this.parameter.OutputStrings + fileName;
        }
        IRuleEditor IRuleHandler.ParamsEditorWindow()
        {
            return new PrefixRuleEditor(parameter);
        }

        IRuleHandler IRuleHandler.Clone()
        {
            AddPrefixRule clone = new AddPrefixRule();
            clone.parameter.InputStrings = this.parameter.InputStrings.Select(x => x.ToString()).ToList();
            clone.parameter.OutputStrings = this.parameter.OutputStrings;
            clone.parameter.Counter = this.parameter.Counter;
            return clone;
        }

    }
}
