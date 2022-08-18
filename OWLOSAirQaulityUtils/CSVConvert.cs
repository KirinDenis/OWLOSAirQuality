using System;
using System.Collections.Generic;

namespace OWLOSAirQaulityUtils
{
    public class CSV
    {
        public List<List<string>> body = new List<List<string>>();
    }

    public class CSVConvert
    {
        public CSV Serialise(string data)
        {
            CSV result = new CSV();
            List<string> line = new List<string>();
            line.Add("line 1 row 1");
            line.Add("line 1 row 2");
            line.Add("line 1 row 3");
            line.Add("line 1 row 4");
            result.body.Add(line);

            line = new List<string>();
            line.Add("line 2 row 1");
            line.Add("line 2 row 2");
            line.Add("line 2 row 3");
            line.Add("line 2 row 4");
            result.body.Add(line);

            return result;
        }

        public string Deserialise(CSV csvData)
        {
            string result = string.Empty;
            
            foreach(List<string> line in csvData.body)
            {
                foreach (string str in line)
                {
                    result += str + ";";
                }
                result += "\n";
            }

            return result;
        }

    }
}
