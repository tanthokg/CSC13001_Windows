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
	class Rule_addPrefix : Rule, IRuleHandler
    {

        public override string ToString()
        {
            if (paremeters.outputStrings.Length == 0)
                return "Add Prefix";
            return "Add Prefix: " + paremeters.outputStrings;
        }

        void IRuleHandler.setParameter(ruleParemeters paremeters)
        {
            this.paremeters = paremeters;
        }
        ruleParemeters IRuleHandler.GetParemeters()
		{
            return this.paremeters;
		}

        bool IRuleHandler.isEditable() { return true; }

        string IRuleHandler.process(string ObjectName, bool isFileType)
        {
            string[] parts = ObjectName.Split('.');
            string extension = parts[^1];
            string fileName;
            if (isFileType)
                fileName = string.Join("", parts.SkipLast(1));
            else
                fileName = ObjectName;

            if (isFileType)
                return this.paremeters.outputStrings + fileName + "." + extension;
            return this.paremeters.outputStrings + fileName;
        }
        IRuleEditor IRuleHandler.parametersEditorWindow()
		{
            return null;
		}

        IRuleHandler IRuleHandler.getClone()
		{
            Rule_addPrefix clone = new Rule_addPrefix();
            clone.paremeters.inputStrings = this.paremeters.inputStrings.Select(x => x.ToString()).ToList();
            clone.paremeters.outputStrings = this.paremeters.outputStrings;
            clone.paremeters.counter = this.paremeters.counter;
            return clone;
		}

    }
}
