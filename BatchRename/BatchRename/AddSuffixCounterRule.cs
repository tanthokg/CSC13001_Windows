using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RuleHandler;

namespace BatchRename
{
    public class AddSuffixCounterRule : Rule, IRuleHandler
    {
        public AddSuffixCounterRule()
        {
            this.parameter = new RuleParameter();
            this.parameter.Counter = 1;
        }

        public override string ToString()
        {
            return "Add counter as suffix";
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
                fileName = string.Join("", parts.SkipLast(1));
            else
                fileName = ObjectName;

            string result = fileName;
            if (isFileType)
            {
                string counterString = this.parameter.Counter++.ToString();
                // counterString = this.parameter.Counter < 11 ? "0" + counterString : counterString;
                return result + " " + counterString + "." + extension;
            }
            return result;
        }
        IRuleEditor IRuleHandler.ParamsEditorWindow()
        {
            return null;
        }
        RuleParameter IRuleHandler.GetParameter()
        {
            return null;
        }
        IRuleHandler IRuleHandler.Clone()
        {
            AddSuffixCounterRule clone = new AddSuffixCounterRule();
            clone.parameter.InputStrings = this.parameter.InputStrings.Select(x => x.ToString()).ToList();
            clone.parameter.OutputStrings = this.parameter.OutputStrings;
            clone.parameter.Counter = this.parameter.Counter;
            return clone;
        }
    }
}
