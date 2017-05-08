namespace LuckyNumber.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            ChiTietCuocChois = new HashSet<ChiTietCuocChoi>();
            ChiTietTrungThuongs = new HashSet<ChiTietTrungThuong>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string username { get; set; }

        [StringLength(50)]
        public string password { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(50)]
        public string nickname { get; set; }

        [StringLength(50)]
        public string phone { get; set; }

        public double? taikhoan { get; set; }

        [StringLength(50)]
        public string mamoi { get; set; }

        [StringLength(50)]
        public string magioithieu { get; set; }

        public int? soluotchoi { get; set; }

        public int? soluotchoi_km { get; set; }

        public bool? status { get; set; }

        public bool? xacnhan { get; set; }

        public int? checktt { get; set; }

        public int? diemdanh { get; set; }

        public int? online { get; set; }

        public int? fb { get; set; }

        public string token { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietCuocChoi> ChiTietCuocChois { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietTrungThuong> ChiTietTrungThuongs { get; set; }
    }
}
