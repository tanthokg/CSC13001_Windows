using System;
using System.Collections.Generic;
using System.Windows;

namespace ruleHandler
{

    public interface IRuleEditor
	{
        public ruleParemeters GetParameters();

        public bool? showDialog();
	}

    public class Rule
    {
        protected ruleParemeters paremeters = new ruleParemeters();
    }
    public interface IRuleHandler
    {
        public bool isEditable();
        public void setParameter(ruleParemeters ruleParemeters)
        {
            return;
        }

        public ruleParemeters GetParemeters();
        public string process(string ObjectName, bool isFileType = true);

        public IRuleEditor parametersEditorWindow();

        public IRuleHandler getClone();
    }



    public class ruleParemeters
    {

        public ruleParemeters()
        {
            inputStrings = new List<string>();
            outputStrings = string.Empty;
            counter = -1;
        }

        public List<string> inputStrings { get; set; }
        public string outputStrings { get; set; }
        public int counter { get; set; }
    }
}
