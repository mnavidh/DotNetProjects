using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CollectionsSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputText = "10/LAX/F/1/08/01/02///18/18/18////////////";
            
            string matchPatt = @"(\d{1,2})*/([A-Z]{3})*/([A-Z]{1})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*/(\d{1,3})*";

            Regex _regexObj = new Regex(matchPatt);

            if (_regexObj.IsMatch(inputText))
            {
                MatchCollection _matchColl = _regexObj.Matches(inputText);                

                foreach(Match _match in _matchColl)
                {
                    string t = _match.Groups[0].Value;
                }
            }
            
        }
    }
}
