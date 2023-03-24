using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Faculty
{
    public class FacultyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Faculty";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Faculty Home",
                "quan-ly/khoa/trang-chu",
                new { controller = "FacultyHome", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Business User List",
                "quan-ly/doanh-nghiep",
                new { controller = "BusinessUsers", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Add Business User",
                "quan-ly/doanh-nghiep/them-moi-doanh-nghiep",
                new { controller = "BusinessUsers", action = "Create", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Business User Details",
                "quan-ly/doanh-nghiep/chi-tiet-doanh-nghiep-{id}",
                new { controller = "BusinessUsers", action = "DetailsBusiness", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Edit Business User Details",
                "quan-ly/doanh-nghiep/cap-nhat-thong-tin-doanh-nghiep-{id}",
                new { controller = "BusinessUsers", action = "Details", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Cooperation Categories",
                "quan-ly/danh-muc-hop-tac",
                new { controller = "CooperationCategories", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "MOU List",
                "quan-ly/ky-ket-mou",
                new { controller = "MOUs", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Create MOU",
                "quan-ly/ky-ket-mou/them-ky-ket-mou",
                new { controller = "MOUs", action = "Create", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "MOU Details",
                "quan-ly/ky-ket-mou/chi-tiet-ky-ket-mou-{id}",
                new { controller = "MOUs", action = "Details", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Edit MOU Details",
                "quan-ly/ky-ket-mou/cap-nhat-ky-ket-mou-{id}",
                new { controller = "MOUs", action = "Edit", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Post List",
                "quan-ly/bai-tuyen-dung",
                new { controller = "Posts", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Add Post",
                "quan-ly/bai-tuyen-dung/them-moi-bai-tuyen-dung",
                new { controller = "Posts", action = "Create", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Post Details",
                "quan-ly/bai-tuyen-dung/chi-tiet-bai-tuyen-dung-{id}",
                new { controller = "Posts", action = "Details", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Edir Post",
                "quan-ly/bai-tuyen-dung/cap-nhat-bai-tuyen-dung-{id}",
                new { controller = "Posts", action = "Edit", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Business Cooperation List",
                "quan-ly/hoat-dong-hop-tac",
                new { controller = "BusinessCooperationCategories", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Create Business Cooperation",
                "quan-ly/hoat-dong-hop-tac/them-hoat-dong-hop-tac",
                new { controller = "BusinessCooperationCategories", action = "Create", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Business Cooperation Details",
                "quan-ly/hoat-dong-hop-tac/chi-tiet-hoat-dong-hop-tac-{id}",
                new { controller = "BusinessCooperationCategories", action = "Details", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Edit Business Cooperation Details",
                "quan-ly/hoat-dong-hop-tac/cap-nhat-hoat-dong-hop-tac-{id}",
                new { controller = "BusinessCooperationCategories", action = "Edit", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Download File",
                "quan-ly/dowload-file",
                new { controller = "Registration", action = "DownloadFile", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Registration List",
                "quan-ly/ung-tuyen",
                new { controller = "Registration", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );

            context.MapRoute(
                "Registration Details",
                "quan-ly/ung-tuyen/chi-tiet-ung-tuyen-{id}",
                new { controller = "Registration", action = "Details", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );
            context.MapRoute(
                "Faculty_default",
                "Faculty/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Faculty.Controllers" }
            );
        }
    }
}