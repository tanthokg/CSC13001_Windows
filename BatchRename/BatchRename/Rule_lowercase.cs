using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ruleHandler;
using System.Windows.Controls;

namespace BatchRename
{
    class Rule_lowercase : Rule, IRuleHandler
    {
        public override string ToString()
        {
            return "lowercase";
        }
        bool IRuleHandler.isEditable()
        {
            return false;
        }

        string IRuleHandler.process(string ObjectName, bool isFileType)
        {
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

        ruleParemeters IRuleHandler.GetParemeters()
		{
            return null;
		}

        IRuleEditor IRuleHandler.parametersEditorWindow()
		{
            return null;
		}

        IRuleHandler IRuleHandler.getClone()
		{
            Rule_lowercase clone = new Rule_lowercase();
            clone.paremeters.inputStrings = this.paremeters.inputStrings.Select(x => x.ToString()).ToList();
            clone.paremeters.outputStrings = this.paremeters.outputStrings;
            clone.paremeters.counter = this.paremeters.counter;
            return clone;
		}

    }
}
