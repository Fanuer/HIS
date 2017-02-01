using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Services.Models
{
    public class FuzzyEntry:IFuzzyEntry<int>
    {

        #region FIELDS
        #endregion

        #region CTOR
        #endregion

        #region METHODS
        protected bool Equals(FuzzyEntry other)
        {
            return string.Equals(SearchQuery, other.SearchQuery) && Id == other.Id && string.Equals(Type, other.Type);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FuzzyEntry)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (SearchQuery != null ? SearchQuery.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Id;
                hashCode = (hashCode * 397) ^ (Type != null ? Type.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(IFuzzyEntry<int> other)
        {
            return Equals((FuzzyEntry)other);
        }

        public static bool operator ==(FuzzyEntry left, FuzzyEntry right)
        {
            return Equals(left, right);
        }
        public static bool operator !=(FuzzyEntry left, FuzzyEntry right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{SearchQuery ?? "'Undefined Query'"} for {Id}[{Type ?? "'Undefined Type'"}]";
        }

        #endregion

        #region PROPERTIES
        [Required]
        public string SearchQuery { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }

        #endregion
    }
}
