using System;
using System.Collections.Generic;

namespace ruleHandler
{

	public class Rule
	{
		protected ruleParemeters paremeters = new ruleParemeters();	
	}
	public interface IRuleHandler
	{
		public string getRuleName();
		public bool isEditable();
		public void setParameter(ruleParemeters ruleParemeters)
		{
			return;
		}
		public string process(string ObjectName, bool isFileType = true);
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
