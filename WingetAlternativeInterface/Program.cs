using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WingetAlternativeInterface
{
    class Program
    {

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Package name: ");
                string searchQuery = Console.ReadLine();

                var callWinget = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "winget.exe",
                        Arguments = $"search {searchQuery}",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                callWinget.Start();

                Dictionary<string, string> searchResult = new Dictionary<string, string>();
                int count = 0;
                string line, packageID = "", packageName = "";
                while (!callWinget.StandardOutput.EndOfStream)
                {
                    if (count > 2)
                    {
                        line = callWinget.StandardOutput.ReadLine();
                        string[] splitLine = line.Split(" ".ToCharArray());

                        int packageIDElement = 0;

                        foreach (int index in Enumerable.Range(0, splitLine.Length))
                        {
                            if (splitLine[index].Contains('.'))
                            {
                                packageID = splitLine[index];
                                packageIDElement = index;
                                break;
                            }
                        }

                        string[] nameArray = splitLine.Take(packageIDElement).ToArray();

                        packageName = string.Join(" ", nameArray);

                        searchResult.Add(packageID, packageName);
                    }
                    else
                        callWinget.StandardOutput.ReadLine();
                    count++;

                }

                foreach (KeyValuePair<string, string> kvp in searchResult)
                {
                    Console.WriteLine("Package: {0}, Name: {1}",
                        kvp.Key, kvp.Value);
                }
            }
        }
    }
}
