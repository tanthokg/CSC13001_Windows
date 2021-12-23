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
	class Rule_uppercase : Rule, IRuleHandler
    {
        public override string ToString()
        {
            return "UPPERCASE";
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

            string result = fileName.ToUpper();
            if (isFileType)
                return result + "." + extension;
            return result;
        }
        IRuleEditor IRuleHandler.parametersEditorWindow()
		{
            return null;
		}
        ruleParemeters IRuleHandler.GetParemeters()
		{
            return null;
		}
        IRuleHandler IRuleHandler.getClone()
		{
            Rule_uppercase clone = new Rule_uppercase();
            clone.paremeters.inputStrings = this.paremeters.inputStrings.Select(x => x.ToString()).ToList();
            clone.paremeters.outputStrings = this.paremeters.outputStrings;
            clone.paremeters.counter = this.paremeters.counter;
            return clone;
		}
    }
}
