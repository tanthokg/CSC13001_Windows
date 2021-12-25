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
    public class ReplaceRuleEditor : Window, IRuleEditor
    {
        public IRuleHandler Rule { get; set; }

        private Canvas canvas = new Canvas();
        private Button ok = new Button();
        private Button cancel = new Button();
        private TextBox editInput = new TextBox();
        private TextBox editOutput = new TextBox();
        private RuleParameter ruleParameter = new RuleParameter();


        public ReplaceRuleEditor(RuleParameter ruleParameter)
        {
            //define UI
            this.Title = "Parameter Editor for Replace Rule";
            this.Width = 400;
            this.Height = 400;
            this.ResizeMode = ResizeMode.NoResize;

            editInput.Height = 80;
            editInput.Width = 360;
            editInput.TextWrapping = TextWrapping.Wrap;
            editInput.AcceptsReturn = true;
            editInput.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            editInput.Margin = new Thickness(15, 80, 0, 0);
            editInput.Text = string.Join("\n", ruleParameter.InputStrings);

            editOutput.Height = 80;
            editOutput.Width = 360;
            editOutput.TextWrapping = TextWrapping.WrapWithOverflow;
            editOutput.Margin = new Thickness(15, 200, 0, 0);
            editOutput.Text = ruleParameter.OutputStrings;

            ok.Content = "Submit";
            ok.Name = "buttonSubmit";
            ok.IsDefault = true;
            ok.Click += this.OnSubmitButtonClick;
            ok.Width = 80;
            ok.Height = 35;
            ok.Margin = new Thickness(80, 300, 0, 0);

            cancel.Click += this.OnCancelButtonClick;
            cancel.IsCancel = true;
            cancel.Content = "Cancel";
            cancel.Width = 80;
            cancel.Height = 35;
            cancel.Margin = new Thickness(220, 300, 0, 0);

            canvas.Children.Add(editInput);
            canvas.Children.Add(editOutput);
            canvas.Children.Add(ok);
            canvas.Children.Add(cancel);

            this.AddChild(canvas);
        }
        private void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            string str = editInput.Text;
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
            string[] inputs = editInput.Text.Split("\n");

            foreach (string input in inputs)
            {
                ruleParameter.InputStrings.Add(input.Trim((char)13));
            }

            ruleParameter.OutputStrings = editOutput.Text;

            return ruleParameter;
        }

        bool? IRuleEditor.ShowDialog()
        {
            return this.ShowDialog();
        }
    }
    public class ReplaceRule : Rule, IRuleHandler
    {
        public ReplaceRule()
        {
            this.parameter = new RuleParameter();
        }
        
        string IRuleHandler.GetRuleType()
		{
            return "ReplaceRule";
		}

        public override string ToString()
        {
            string result = "Replace", input = " ";
            foreach (string s in parameter.InputStrings)
            {
                if (String.IsNullOrEmpty(s))
                    break;
                input += s + ", ";
            }
            if (input.Length > 1)
            {
                input = input.Substring(0, input.Length - 2);
                result += input;
                result += " with " + parameter.OutputStrings;
            }
            return result;
        }

        bool IRuleHandler.IsEditable() { return true; }

        void IRuleHandler.SetParameter(RuleParameter ruleParameter)
        {
            this.parameter = ruleParameter;
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

            string result = fileName;
            this.parameter.InputStrings.ForEach(s =>
            {
                result = result.Replace(s, this.parameter.OutputStrings);
            });

            if (isFileType)
                return result + "." + extension;
            return result;
        }
        IRuleEditor IRuleHandler.ParamsEditorWindow()
        {
            return new ReplaceRuleEditor(parameter);
        }
        IRuleHandler IRuleHandler.Clone()
        {
            ReplaceRule clone = new ReplaceRule();
            clone.parameter.InputStrings = this.parameter.InputStrings.Select(x => x.ToString()).ToList();
            clone.parameter.OutputStrings = this.parameter.OutputStrings;
            clone.parameter.Counter = this.parameter.Counter;
            return clone;
        }
    }
}
