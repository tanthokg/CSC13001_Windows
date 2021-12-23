using System;
using System.Collections.Generic;
using System.Windows;

namespace RuleHandler
{

    public interface IRuleEditor
    {
        public RuleParameter GetParameter();

        public bool? ShowDialog();
    }

    public class Rule
    {
        protected RuleParameter parameter = new RuleParameter();
    }
    public interface IRuleHandler
    {
        public bool IsEditable();
        public void SetParameter(RuleParameter ruleParameter)
        {
            return;
        }

        public RuleParameter GetParameter();
        public string Process(string ObjectName, bool isFileType = true);

        public IRuleEditor ParamsEditorWindow();

        public IRuleHandler Clone();
    }



    public class RuleParameter
    {

        public RuleParameter()
        {
            InputStrings = new List<string>();
            OutputStrings = string.Empty;
            Counter = -1;
        }

        public List<string> InputStrings { get; set; }
        public string OutputStrings { get; set; }
        public int Counter { get; set; }
    }
}
