using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JDF
{
    internal class Program
    {
        static string filePath = string.Empty;

        static void Main()
        {
            filePath = @"F:\Programmering\C#\JDF\JDF\example.jdf";
            ReadOptions();
        }
        public Program(string path)
        {
            filePath = path;
            filePath = @"F:\Programmering\C#\JDF\JDF\example.jdf";
        }

        /// <summary>
        /// Adds an option with a single parameter with a belonging value
        /// </summary>
        /// <param name="name">
        /// The name for the option
        /// </param>
        /// <param name="option">
        /// The name you want to give the parameter
        /// </param>
        ///<param name="value">
        /// The value you want to assign the parameter
        /// </param>
        static void AddOptionWithSingleParam(string name, string option, string value)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Concat(name, "{", "\n"));
            sb.Append(string.Concat(">", option, "=", value));
            sb.Append(string.Concat("\n", "}"));
        }

        /// <summary>
        /// Adds an option with multiple parameters. Make sure the indexes of the values are matching the correct index in the options array
        /// </summary>
        /// <param name="name">
        /// The name for the option
        /// </param>
        /// <param name="options">
        /// The names you want to give the parameters
        /// </param>
        ///<param name="values">
        /// The values you want to assign the parameters
        /// </param>
        static void AddOptionWithMultipleParams(string name, string[] options, string[] values)
        {
            if (options.Length != values.Length) //Make sure there are as many values as options
                throw new Exception("Mismatch in option and values count");

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Concat(name, "{", "\n"));
            for (int i = 0; i < options.Length; ++i)
            {
                sb.Append(string.Concat(">", options[i], "=", values[i], "\n"));
            }
            sb.Append("}");
        }

        /// <summary>
        /// Reads all the options from your file and returns them in a list structure
        /// </summary>
        static Object[] ReadOptions()
        {
            string objects = File.ReadAllText(filePath);
            int objectCount = Regex.Matches(objects, "}").Count; //This is accurate as long as people follow syntax correctly
            Object[] options = new Object[objectCount];

            string[] lines = File.ReadAllLines(filePath);

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            string name = string.Empty;
            string[] keys = new string[2];
            string keyLine = string.Empty;
            int objIndex = 0;
            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Contains("{"))
                    name = lines[i].Substring(0, lines[i].IndexOf("{")); //Extract the name from the line

                if (lines[i].StartsWith(">"))
                {
                    keyLine = lines[i].Substring(1, lines[i].Length - 1);
                    keys = keyLine.Split('=');
                    keyValuePairs.Add(keys[0], keys[1]);
                }

                if (lines[i].Equals("}"))
                {
                    Object obj;
                    obj.name = name;
                    obj.parameters = keyValuePairs;
                    options[objIndex] = obj;
                    ++objIndex;
                }
            }
            return options;
        }

        /// <summary>
        /// Let's you write new data into the JDF file
        /// </summary>
        static void WriteNewData(string data)
        {
            
        }
    }
}
