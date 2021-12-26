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

namespace BatchRename
{
    public class PascalCaseRuleEditor : Window, IRuleEditor
    {
        public IRuleHandler Rule { get; set; }

        private Canvas canvas = new Canvas();
        private Label label = new Label();
        private Button submitBtn = new Button();
        private Button cancelBtn = new Button();
        private TextBox editTxtBox = new TextBox();
        private RuleParameter ruleParameter = new RuleParameter();

        public PascalCaseRuleEditor(RuleParameter ruleParameter)
        {
            // Define UI
            this.Title = "Parameter Editor for PascalCase Rule";
            this.Width = 415;
            this.Height = 250;
            this.ResizeMode = ResizeMode.NoResize;

            label.Content = "Please type characters you want to treat as word\ndelimiter/separator";
            label.Margin = new Thickness(20, 10, 0, 0);
            label.FontSize = 16;

            editTxtBox.Width = 360;
            editTxtBox.Height = 80;
            editTxtBox.TextWrapping = TextWrapping.WrapWithOverflow;
            editTxtBox.Margin = new Thickness(20, 65, 0, 0);
            editTxtBox.Text = ruleParameter.InputStrings[0];

            submitBtn.Content = "Submit";
            submitBtn.Name = "buttonSubmit";
            submitBtn.IsDefault = true;
            submitBtn.Click += this.OnSubmitButtonClick;
            submitBtn.Width = 170;
            submitBtn.Height = 40;
            submitBtn.Margin = new Thickness(20, 160, 0, 0);
            submitBtn.FontSize = 15;

            cancelBtn.Click += this.OnCancelButtonClick;
            cancelBtn.IsCancel = true;
            cancelBtn.Content = "Cancel";
            cancelBtn.Width = 170;
            cancelBtn.Height = 40;
            cancelBtn.Margin = new Thickness(210, 160, 0, 0);
            cancelBtn.FontSize = 15;

            canvas.Children.Add(editTxtBox);
            canvas.Children.Add(label);
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
            ruleParameter.InputStrings.Clear();
            ruleParameter.InputStrings.Add(editTxtBox.Text);

            return ruleParameter;
        }

        bool? IRuleEditor.ShowDialog()
        {
            return this.ShowDialog();
        }
    }
    public class PascalCaseRule : Rule, IRuleHandler
    {

        public PascalCaseRule()
        {
            this.parameter = new RuleParameter();
            this.parameter.InputStrings.Clear();
            this.parameter.InputStrings.Add("_");
        }
        private string UpperCaseFirstLetter(string str)
        {
            if (str.Length == 0)
                return str;
            return str.ToUpper()[0] + str.Substring(1);
        }

        public override string ToString()
        {
            return "PascalCase";
        }

        string IRuleHandler.GetRuleType()
        {
            return "PascalCaseRule";
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
            if (string.IsNullOrEmpty(ObjectName))
                return "";

            string[] parts = ObjectName.Split('.');
            string extension = parts[^1];
            string fileName;
            if (isFileType)
                fileName = string.Join(".", parts.SkipLast(1));
            else
                fileName = ObjectName;

            string[] result = fileName.Split(this.parameter.InputStrings[0]);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (string str in result)
            {
                stringBuilder.Append(UpperCaseFirstLetter(str));
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
