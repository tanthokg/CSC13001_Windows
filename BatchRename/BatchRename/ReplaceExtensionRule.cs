using System;
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
    public class ReplaceExtensionRuleEditor : Window, IRuleEditor
    {
        public IRuleHandler Rule { get; set; }

        private Canvas canvas = new Canvas();
        private Label label1 = new Label(), label2 = new Label();
        private Button submitBtn = new Button();
        private Button cancelBtn = new Button();
        private TextBox editInput = new TextBox();
        private TextBox editOutput = new TextBox();
        private RuleParameter ruleParameter = new RuleParameter();

        public ReplaceExtensionRuleEditor (RuleParameter ruleParameter)
        {
            // Define UI
            this.Title = "Parameter Editor for Replace Extension Rule";
            this.Width = 415;
            this.Height = 365;
            this.ResizeMode = ResizeMode.NoResize;

            label1.Content = "Please type extensions you want to replace.\nTo replace multiple ones, use enter.";
            label1.Margin = new Thickness(20, 10, 0, 0);
            label1.FontSize = 16;

            editInput.Height = 80;
            editInput.Width = 360;
            editInput.TextWrapping = TextWrapping.Wrap;
            editInput.AcceptsReturn = true;
            editInput.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            editInput.Margin = new Thickness(20, 65, 0, 0);
            editInput.Text = string.Join("\n", ruleParameter.InputStrings);

            label2.Content = "Please type characters you want to replace with";
            label2.Margin = new Thickness(20, 150, 0, 0);
            label2.FontSize = 16;

            editOutput.Height = 80;
            editOutput.Width = 360;
            editOutput.TextWrapping = TextWrapping.WrapWithOverflow;
            editOutput.Margin = new Thickness(20, 180, 0, 0);
            editOutput.Text = ruleParameter.OutputStrings;

            submitBtn.Content = "Submit";
            submitBtn.Name = "buttonSubmit";
            submitBtn.IsDefault = true;
            submitBtn.Click += this.OnSubmitButtonClick;
            submitBtn.Width = 170;
            submitBtn.Height = 40;
            submitBtn.Margin = new Thickness(20, 270, 0, 0);
            submitBtn.FontSize = 15;

            cancelBtn.Click += this.OnCancelButtonClick;
            cancelBtn.IsCancel = true;
            cancelBtn.Content = "Cancel";
            cancelBtn.Width = 170;
            cancelBtn.Height = 40;
            cancelBtn.Margin = new Thickness(210, 270, 0, 0);
            cancelBtn.FontSize = 15;

            canvas.Children.Add(label1);
            canvas.Children.Add(label2);
            canvas.Children.Add(editInput);
            canvas.Children.Add(editOutput);
            canvas.Children.Add(submitBtn);
            canvas.Children.Add(cancelBtn);

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
	class ReplaceExtensionRule : Rule, IRuleHandler
	{
        public ReplaceExtensionRule()
		{
            this.parameter = new RuleParameter();
		}

        string IRuleHandler.GetRuleType()
		{
            return "ReplaceExtensionRule";
		}
        public override string ToString()
        {
            return this.parameter.OutputStrings.Length == 0 ? "Replace file\'s extension" 
                : $"Replace file\'s extension: from \"{string.Join("\", \"",this.parameter.InputStrings)}\" to \"{this.parameter.OutputStrings}\"";
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
                if(this.parameter.InputStrings.Contains(extension))
                    return result + "." + this.parameter.OutputStrings;
                else
                    return result + "." + extension;
            return result;
        }
        IRuleEditor IRuleHandler.ParamsEditorWindow()
        {
            return new ReplaceExtensionRuleEditor(this.parameter);
        }
        RuleParameter IRuleHandler.GetParameter()
        {
            return this.parameter;
        }
        IRuleHandler IRuleHandler.Clone()
        {
            ReplaceExtensionRule clone = new ReplaceExtensionRule();
            clone.parameter.InputStrings = this.parameter.InputStrings.Select(x => x.ToString()).ToList();
            clone.parameter.OutputStrings = this.parameter.OutputStrings;
            clone.parameter.Counter = this.parameter.Counter;
            return clone;
        }
	} 
}
