//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataHelpers.Tests
{
    using System;
    using System.Collections.Generic;
    
    public partial class CurrencyRate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CurrencyRate()
        {
            this.SalesOrderHeaders = new HashSet<SalesOrderHeader>();
        }
    
        public int CurrencyRateID { get; set; }
        public System.DateTime CurrencyRateDate { get; set; }
        public string FromCurrencyCode { get; set; }
        public string ToCurrencyCode { get; set; }
        public decimal AverageRate { get; set; }
        public decimal EndOfDayRate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    
        public virtual Currency Currency { get; set; }
        public virtual Currency Currency1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; }
    }
}
