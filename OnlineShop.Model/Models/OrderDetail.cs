using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Model.Models
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        [Key]
        public int OrderID { get; set; }
        [Key]
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("OrderID")]
        public virtual ProductCategory ProductCategories { get; set; }
        [ForeignKey("ProductID")]
        public virtual  Order Orders { get; set; }

    }
}
