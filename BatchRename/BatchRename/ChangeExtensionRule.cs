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
    public class ChangeExtensionRuleEditor : Window, IRuleEditor
    {
        public IRuleHandler Rule { get; set; }

        private Canvas canvas = new Canvas();
        private Button ok = new Button();
        private Button cancel = new Button();
        private TextBox editTextBox = new TextBox();

        public ChangeExtensionRuleEditor(RuleParameter ruleParameter)
        {
            //define UI
            this.Title = "Parameter Editor for Change Extension Rule";
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
            return "Change file\'s extension";
        }
        string IRuleHandler.ToJson()
		{
            string RuleType = ((IRuleHandler)this).GetRuleType();
            List<string> InputStrings = this.parameter.InputStrings;
            string OutputStrings = this.parameter.OutputStrings;
            int Counter = this.parameter.Counter;

            RuleJsonFormat format = new RuleJsonFormat
            {
                RuleType = RuleType,
                InputStrings = InputStrings,
                OutputStrings = OutputStrings,
                Counter = Counter,
			};
            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(format, options);
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
                fileName = string.Join("", parts.SkipLast(1));
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
            return null;
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
