using JustBasic.Model.Models;
using JustBasic.Web.Models;
using System;

namespace JustBasic.Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdatePostCategory(this PostCategory postCategory, PostCategoryViewModel postCategoryVm)
        {
            postCategory.ID = postCategoryVm.ID;
            postCategory.Name = postCategoryVm.Name;
            postCategory.Description = postCategoryVm.Description;
            postCategory.Alias = postCategoryVm.Alias;
            postCategory.ParentID = postCategoryVm.ParentID;
            postCategory.DisplayOrder = postCategoryVm.DisplayOrder;
            postCategory.Image = postCategoryVm.Image;
            postCategory.HomeFlag = postCategoryVm.HomeFlag;

            postCategory.CreatedDate = postCategoryVm.CreatedDate;
            postCategory.CreatedBy = postCategoryVm.CreatedBy;
            postCategory.UpdatedDate = postCategoryVm.UpdatedDate;
            postCategory.UpdatedBy = postCategoryVm.UpdatedBy;
            postCategory.MetaKeyword = postCategoryVm.MetaKeyword;
            postCategory.MetaDescription = postCategoryVm.MetaDescription;
            postCategory.Status = postCategoryVm.Status;
        }

        public static void UpdateProductCategory(this ProductCategory productCategory, ProductCategoryViewModel productCategoryVm)
        {
            productCategory.ID = productCategoryVm.ID;
            productCategory.Name = productCategoryVm.Name;
            productCategory.Description = productCategoryVm.Description;
            productCategory.Alias = productCategoryVm.Alias;
            productCategory.ParentID = productCategoryVm.ParentID;
            productCategory.DisplayOrder = productCategoryVm.DisplayOrder;
            productCategory.Image = productCategoryVm.Image;
            productCategory.HomeFlag = productCategoryVm.HomeFlag;

            productCategory.CreatedDate = productCategoryVm.CreatedDate;
            productCategory.CreatedBy = productCategoryVm.CreatedBy;
            productCategory.UpdatedDate = productCategoryVm.UpdatedDate;
            productCategory.UpdatedBy = productCategoryVm.UpdatedBy;
            productCategory.MetaKeyword = productCategoryVm.MetaKeyword;
            productCategory.MetaDescription = productCategoryVm.MetaDescription;
            productCategory.Status = productCategoryVm.Status;
            productCategory.Icon = productCategoryVm.Icon;
        }

        public static void UpdatePost(this Post post, PostViewModel postVm)
        {
            post.ID = postVm.ID;
            post.Name = postVm.Name;
            post.Description = postVm.Description;
            post.Alias = postVm.Alias;
            post.CategoryID = postVm.CategoryID;
            post.Content = postVm.Content;
            post.Image = postVm.Image;
            post.HomeFlag = postVm.HomeFlag;
            post.ViewCount = postVm.ViewCount;

            post.CreatedDate = postVm.CreatedDate;
            post.CreatedBy = postVm.CreatedBy;
            post.UpdatedDate = postVm.UpdatedDate;
            post.UpdatedBy = postVm.UpdatedBy;
            post.MetaKeyword = postVm.MetaKeyword;
            post.MetaDescription = postVm.MetaDescription;
            post.Status = postVm.Status;
        }

        public static void UpdateProduct(this Product product, ProductViewModel productVm)
        {
            product.ID = productVm.ID;
            product.Name = productVm.Name;
            product.Description = productVm.Description;
            product.Alias = productVm.Alias;
            product.CategoryID = productVm.CategoryID;
            product.Content = productVm.Content;
            product.Image = productVm.Image;
            product.MoreImages = productVm.MoreImages;
            product.Price = productVm.Price;
            product.PromotionPrice = productVm.PromotionPrice;
            product.Warranty = productVm.Warranty;
            product.HomeFlag = productVm.HomeFlag;
            product.HotFlag = productVm.HotFlag;
            product.ViewCount = productVm.ViewCount;

            product.CreatedDate = productVm.CreatedDate;
            product.CreatedBy = productVm.CreatedBy;
            product.UpdatedDate = productVm.UpdatedDate;
            product.UpdatedBy = productVm.UpdatedBy;
            product.MetaKeyword = productVm.MetaKeyword;
            product.MetaDescription = productVm.MetaDescription;
            product.Status = productVm.Status;
            product.Tags = productVm.Tags;
            product.Quantity = productVm.Quantity;
            product.OriginalPrice = productVm.OriginalPrice;
        }

        public static void UpdateFeedback(this Feedback feedback, FeedbackViewModel feedbackVm)
        {
            feedback.Name = feedbackVm.Name;
            feedback.Email = feedbackVm.Email;
            feedback.Message = feedbackVm.Message;
            feedback.Status = feedbackVm.Status;
            feedback.CreatedDate = DateTime.Now;
        }

        public static void UpdateOrder(this Order order, OrderViewModel orderVm)
        {
            order.CustomerName = orderVm.CustomerName;
            order.CustomerAddress = orderVm.CustomerAddress;
            order.CustomerEmail = string.IsNullOrEmpty(orderVm.CustomerEmail)?"": orderVm.CustomerEmail;
            order.CustomerMobile = orderVm.CustomerMobile;
            order.CustomerMessage = orderVm.CustomerMessage;
            order.PaymentMethod = orderVm.PaymentMethod;
            order.Description = orderVm.Description;
            order.TotalAmount = orderVm.TotalAmount;
            order.TotalDiscount = orderVm.TotalDiscount;
            order.CreatedDate = DateTime.Now;
            order.CreatedBy = orderVm.CreatedBy;
            order.Status = orderVm.Status;
            order.CustomerId = orderVm.CustomerId;

            //order.CustomerName = orderVm.CustomerName;
            //order.CustomerAddress = orderVm.CustomerName;
            //order.CustomerEmail = orderVm.CustomerName;
            //order.CustomerMobile = orderVm.CustomerName;
            //order.CustomerMessage = orderVm.CustomerName;
            //order.PaymentMethod = orderVm.CustomerName;
            //order.CreatedDate = DateTime.Now;
            //order.CreatedBy = orderVm.CreatedBy;
            //order.Status = orderVm.Status;
            //order.CustomerId = orderVm.CustomerId;
        }

        public static void UpdateApplicationGroup(this ApplicationGroup appGroup, ApplicationGroupViewModel appGroupViewModel)
        {
            appGroup.ID = appGroupViewModel.ID;
            appGroup.Name = appGroupViewModel.Name;
        }

        public static void UpdateApplicationRole(this ApplicationRole appRole, ApplicationRoleViewModel appRoleViewModel, string action = "add")
        {
            if (action == "update")
                appRole.Id = appRoleViewModel.Id;
            else
                appRole.Id = Guid.NewGuid().ToString();
            appRole.Name = appRoleViewModel.Name;
            appRole.Description = appRoleViewModel.Description;
        }

        public static void UpdateUser(this ApplicationUser appUser, ApplicationUserViewModel appUserViewModel, string action = "add")
        {
            appUser.Id = appUserViewModel.Id;
            appUser.FullName = appUserViewModel.FullName;
            appUser.BirthDay = appUserViewModel.BirthDay;
            appUser.Email = appUserViewModel.Email;
            appUser.UserName = appUserViewModel.UserName;
            appUser.PhoneNumber = appUserViewModel.PhoneNumber;
        }

        public static void UpdatePrice(this Price price, PriceViewModel priceVm)
        {
            price.ID = priceVm.ID;
            price.ProductID = priceVm.ProductID;
            price.SizeID = priceVm.SizeID;
            price.ColorID = priceVm.ColorID;
            price.SalePrice = priceVm.SalePrice;
            price.PromotionPrice = priceVm.PromotionPrice;
            price.IsPrimary = priceVm.IsPrimary;
        }

        public static void UpdateSlide(this Slide slide, SlideViewModel slideVm)
        {
            slide.ID = slideVm.ID;
            slide.Name = slideVm.Name;
            slide.Description = slideVm.Description;
            slide.Image = slideVm.Image;
            slide.Url = slideVm.Url;
            slide.DisplayOrder = slideVm.DisplayOrder;
            slide.Status = slideVm.Status;
            slide.Content = slideVm.Content;
        }

        public static void UpdateColor(this Color color, ColorViewModel colorVm)
        {
            color.ID = colorVm.ID;
            color.Name = colorVm.Name;
            color.Description = colorVm.Description;
            color.Image = colorVm.Image;
            color.Status = colorVm.Status;
        }

        public static void UpdateSize(this Size size, SizeViewModel sizeVm)
        {
            size.ID = sizeVm.ID;
            size.Name = sizeVm.Name;
            size.Description = sizeVm.Description;
            size.Status = sizeVm.Status;
        }

        public static void UpdatePage(this Page page, PageViewModel pageVm)
        {
            page.ID = pageVm.ID;
            page.Name = pageVm.Name;
            page.Alias = pageVm.Alias;
            page.Content = pageVm.Content;
            page.Status = pageVm.Status;
        }

        public static void UpdateFooter(this Footer footer, FooterViewModel footerVm)
        {
            footer.ID = footerVm.ID;
            footer.Name = footerVm.Name;
            footer.Content = footerVm.Content;
        }

        public static void UpdateMenu(this Menu menu, MenuViewModel menuVm)
        {
            menu.ID = menuVm.ID;
            menu.Name = menuVm.Name;
            menu.URL = menuVm.URL;
            menu.DisplayOrder = menuVm.DisplayOrder;
            menu.GroupID = menuVm.GroupID;
            menu.MenuGroup = menuVm.MenuGroup;
            menu.Target = menuVm.Target;
            menu.Status = menuVm.Status;
        }
    }
}