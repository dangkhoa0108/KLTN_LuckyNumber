namespace LuckyNumber.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CuocChoi")]
    public partial class CuocChoi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CuocChoi()
        {
            ChiTietCuocChois = new HashSet<ChiTietCuocChoi>();
            DanhSachTrungThuongs = new HashSet<DanhSachTrungThuong>();
        }

        [Key]
        public int MaCuocChoi { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayDoanSo { get; set; }

        public bool? TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietCuocChoi> ChiTietCuocChois { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhSachTrungThuong> DanhSachTrungThuongs { get; set; }
    }
}
