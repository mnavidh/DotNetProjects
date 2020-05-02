using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSVReader
{
    class ResultMap
    {
        string _countryID;

        public string CountryID
        {
            get { return _countryID; }
            set { _countryID = value; }
        }

        string _countryDesc;

        public string CountryDesc
        {
            get { return _countryDesc; }
            set { _countryDesc = value; }
        }

    }
}
