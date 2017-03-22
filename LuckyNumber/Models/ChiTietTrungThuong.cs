namespace LuckyNumber.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietTrungThuong")]
    public partial class ChiTietTrungThuong
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaDSTrungThuong { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SoDuDoan { get; set; }

        public double? TienThuong { get; set; }
        public virtual DanhSachTrungThuong DanhSachTrungThuong { get; set; }

        public virtual User User { get; set; }
    }
}
