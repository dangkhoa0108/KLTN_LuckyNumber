namespace LuckyNumber.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhSachTrungThuong")]
    public partial class DanhSachTrungThuong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DanhSachTrungThuong()
        {
            ChiTietTrungThuongs = new HashSet<ChiTietTrungThuong>();
        }

        [Key]
        public int MaDSTrungThuong { get; set; }

        public int MaCuocChoi { get; set; }

        public double? TongTienThuong { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietTrungThuong> ChiTietTrungThuongs { get; set; }

        public virtual CuocChoi CuocChoi { get; set; }
    }
}
