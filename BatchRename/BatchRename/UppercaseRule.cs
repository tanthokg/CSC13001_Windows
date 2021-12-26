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
    public class UppercaseRule : Rule, IRuleHandler
    {

        public UppercaseRule()
		{
            this.parameter = new RuleParameter();
		}

        public override string ToString()
        {
            return "UPPERCASE";
        }
        string IRuleHandler.GetRuleType()
		{
            return "UppercaseRule";
		}
        
        bool IRuleHandler.IsEditable()
        {
            return false;
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

            string result = fileName.ToUpper();
            if (isFileType)
                return result + "." + extension;
            return result;
        }
        IRuleEditor IRuleHandler.ParamsEditorWindow()
        {
            return null;
        }
        RuleParameter IRuleHandler.GetParameter()
        {
            return this.parameter;
        }
        IRuleHandler IRuleHandler.Clone()
        {
            UppercaseRule clone = new UppercaseRule();
            clone.parameter.InputStrings = this.parameter.InputStrings.Select(x => x.ToString()).ToList();
            clone.parameter.OutputStrings = this.parameter.OutputStrings;
            clone.parameter.Counter = this.parameter.Counter;
            return clone;
        }
    }
}
