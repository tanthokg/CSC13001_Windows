using System;
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
    class LowercaseRule : Rule, IRuleHandler
    {
        public override string ToString()
        {
            return "lowercase";
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
            result = result.ToLower();
            result = result.Replace(" ", "");
            if (isFileType)
                return result + "." + extension;
            return result;
        }

        RuleParameter IRuleHandler.GetParameter()
		{
            return null;
		}

        IRuleEditor IRuleHandler.ParamsEditorWindow()
		{
            return null;
		}

        IRuleHandler IRuleHandler.Clone()
		{
            LowercaseRule clone = new LowercaseRule();
            clone.parameter.InputStrings = this.parameter.InputStrings.Select(x => x.ToString()).ToList();
            clone.parameter.OutputStrings = this.parameter.OutputStrings;
            clone.parameter.Counter = this.parameter.Counter;
            return clone;
		}

    }
}
