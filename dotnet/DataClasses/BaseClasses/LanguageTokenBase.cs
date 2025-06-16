
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SqlOnAir.DotNet.Lib;

namespace SqlOnAir.DotNet.Lib.DataClasses.BaseClasses
{
    public class LanguageTokenBase : SoAEntityBase
    {
        [Key]
        public string LanguageTokenId { get; set; } = $"{Guid.NewGuid()}";

        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public int? MeaningOfLife
        {
            get => null /* Unhandled formula: 42 */; set { }
        }

        public int? SortOrder { get; set; }
        public bool? Publish { get; set; }
        public string? Token
        {
            get => null /* Unhandled formula: {Name} */; set { }
        }



        protected override void LazyLoadProperties()
        {
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}