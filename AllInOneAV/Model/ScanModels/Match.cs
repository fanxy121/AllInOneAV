using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ScanModels
{
    public class Match
    {
        public string AvID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        //public DateTime CreateTime { get; set; }
    }

    public class MatchComparer : IEqualityComparer<Match>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(Match x, Match y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return ReplaceInvalidChar(x.Name) == ReplaceInvalidChar(y.Name) && ReplaceInvalidChar(x.AvID) == ReplaceInvalidChar(y.AvID) && ReplaceInvalidChar(x.Location) == ReplaceInvalidChar(y.Location) && ReplaceInvalidChar(x.Name) == ReplaceInvalidChar(y.Name);
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(Match match)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(match, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashMatchName = match.Name == null ? 0 : match.Name.GetHashCode();

            //Get hash code for the Code field.
            int hashMatchLocation = match.Location == null ? 0 : match.Location.GetHashCode();

            int hashMatchAvID = match.AvID == null ? 0 : match.AvID.GetHashCode();

            //Calculate the hash code for the product.
            return hashMatchName ^ hashMatchLocation ^ hashMatchAvID;
        }

        public string ReplaceInvalidChar(string str)
        {
            return str.Replace("/", "").Replace("\"", "").Replace("\\", "").Replace(":", "").Replace("?", "").Replace("*", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("'", "");
        }

    }
}
