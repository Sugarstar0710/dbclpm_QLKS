//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebQLKS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_ChucVu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_ChucVu()
        {
            this.tbl_NhanVien = new HashSet<tbl_NhanVien>();
        }
    
        public string MaCV { get; set; }
        public string TenChucVu { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_NhanVien> tbl_NhanVien { get; set; }
    }
}
