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
        static Object[] currentFile;
        static List<Object> newObjects = new List<Object>();

        //For testing purposes only
        //static void Main()
        //{
        //    filePath = @"F:\Programmering\C#\JDF\JDF\example.jdf";
        //    currentFile = ReadOptions();

        //    Dictionary<string, string> options = new Dictionary<string, string>();
        //    options.Add("color", "black");
        //    //AddOption("colors", options);
        //    UpdateParameter("colors", "color", "red");
        //    WriteNewData();
        //}

        public Program(string path)
        {
            filePath = path;
            currentFile = ReadOptions();
        }

        private static bool ContainsName(string name)
        {
            for (int i = 0; i < currentFile.Length; ++i)
            {
                if (currentFile[i].name == name)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Adds an option with multiple parameters. Make sure the indexes of the values are matching the correct index in the options array
        /// </summary>
        /// <param name="name">
        /// The name for the option
        /// </param>
        /// <param name="parameters">
        /// A dictionary containing the parameter names and values
        /// </param>
        public static void AddOption(string name, Dictionary<string, string> parameters)
        {
            if (ContainsName(name))
                throw new Exception("Name already exists, update it instead to avoid doubles.");

            Object obj;
            obj.name = name;
            obj.parameters = parameters;
            newObjects.Add(obj);
        }

        /// <summary>
        /// Updates a parameter in a given object
        /// </summary>
        /// <param name="name">
        /// The name of the object to update
        /// </param>
        /// <param name="paramName">
        /// The parameter you want to change
        /// </param>
        /// <param name="newValue">
        /// The new value you want to assign the parameter
        /// </param>
        public static void UpdateParameter(string name, string paramName, string newValue)
        {
            for (int i = 0; i < currentFile.Length; ++i)
            {
                if(currentFile[i].name == name)
                    currentFile[i].parameters[paramName] = newValue;
            }

            for (int i = 0; i < newObjects.Count; ++i)
            {
                if (newObjects[i].name == name)
                    newObjects[i].parameters[paramName] = newValue;
            }
        }

        /// <summary>
        /// Reads all the options from your file and returns them in a list structure
        /// </summary>
        public static Object[] ReadOptions()
        {
            string objects = File.ReadAllText(filePath);
            int objectCount = Regex.Matches(objects, "}").Count; //This is accurate as long as people follow syntax correctly
            Object[] options = new Object[objectCount];

            string[] lines = File.ReadAllLines(filePath);
            Dictionary<string, string> keyValuePairs = null;

            string name = string.Empty;
            string[] keys = new string[2];
            string keyLine = string.Empty;
            int objIndex = 0;
            for (int i = 0; i < lines.Length; ++i)
            {

                if (lines[i].Contains("{"))
                {
                    name = lines[i].Substring(0, lines[i].IndexOf("{")); //Extract the name from the line
                    keyValuePairs = new Dictionary<string, string>();
                }

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

        private static string ParseObjects()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < currentFile.Length; ++i)
            {
                sb.Append(string.Concat("\n", currentFile[i].name));
                sb.Append("{\n");
                for (int j = 0; j < currentFile[i].parameters.Count; ++j)
                {
                    sb.Append(string.Concat(">", currentFile[i].parameters.Keys.ElementAt(j), "=", currentFile[i].parameters.Values.ElementAt(j), "\n"));
                }
                sb.Append("}");
            }

            if (newObjects.Count > 0)
            {
                for (int i = 0; i < newObjects.Count; ++i)
                {
                    sb.Append(string.Concat("\n", newObjects[i].name));
                    sb.Append("{\n");
                    for (int j = 0; j < newObjects[i].parameters.Count; ++j)
                    {
                        sb.Append(string.Concat(">", newObjects[i].parameters.Keys.ElementAt(j), "=", newObjects[i].parameters.Values.ElementAt(j), "\n"));
                    }
                    sb.Append("}");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Writes all the data that has been added/updated
        /// </summary>
        public static void WriteNewData()
        {
            string data = ParseObjects();
            File.WriteAllText(filePath, data);
        }
    }
}
