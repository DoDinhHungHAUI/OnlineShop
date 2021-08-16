namespace OnlineShop.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using OnlineShop.Model.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OnlineShop.Data.OnlineShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OnlineShop.Data.OnlineShopDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            CreateProductCategorySample(context);

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new OnlineShopDbContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new OnlineShopDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "onlineShop",
                Email = "hungmien0411@gmail.com",
                EmailConfirmed = true,
                BirthDay = DateTime.Now,
                FullName = "Technology Education"
            };

            manager.Create(user, "123456$");
            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByEmail("hungmien0411@gmail.com");

            manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });



        }

        private void CreateProductCategorySample(OnlineShop.Data.OnlineShopDbContext context)
        {
            if(context.ProductCategories.Count() == 0)
            {
                List<ProductCategory> listProductCategory = new List<ProductCategory>()
                {
                    new ProductCategory() { Name = "Điện lạnh", Alias = "dien-lanh", Status = true },
                    new ProductCategory() { Name = "Viễn thông", Alias = "vien-thong", Status = true },
                    new ProductCategory() { Name = "Đồ gia dụng", Alias = "do-gia-dung", Status = true },
                    new ProductCategory() { Name = "Mỹ phẩm", Alias = "my-pham", Status = true }
                };

                context.ProductCategories.AddRange(listProductCategory);
                context.SaveChanges();
            }    
        }



    }
}
