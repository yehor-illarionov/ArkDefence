using Boxed.AspNetCore;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.ViewModels;

namespace WebApplication1
{
    public static class AsyncCommandExtentions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantInfo">any existing tenant</param>
        /// <param name="connectionString">Postgres connection string</param>
        /// <returns>bool</returns>
        public static bool TestConnection<T1>(
            this IAsyncCommand<T1> command, 
            TenantInfo tenantInfo, 
            string connectionString)
        {
            return TestConection(tenantInfo, connectionString);
        }

        public static bool TestConnection<T1,T2>(
            this IAsyncCommand<T1,T2> command,
            TenantInfo tenantInfo,
            string connectionString)
        {
            return TestConection(tenantInfo, connectionString);
        }

        public static bool TestConnection<T1, T2, T3>(
          this IAsyncCommand<T1, T2, T3> command,
          TenantInfo tenantInfo,
          string connectionString)
        {
            return TestConection(tenantInfo, connectionString);
        }

        private static bool TestConection(TenantInfo tenantInfo, string connectionString)
        {
            var builder = new DbContextOptionsBuilder<NextAppContext>();
            builder.UseNpgsql(connectionString);
            using (var db = new NextAppContext(tenantInfo, builder.Options))
            {
                if (db.Database.CanConnect() == false)
                {
                    return false;
                }
            }
            return true;
        }

        public static string GetPageLinkValue<T1,T2,T3, TModel>(
            this IAsyncCommand<T1,T2,T3> command,
            LinkGenerator generator,
            string pathBase,
            string actionRouteName,
            PageResult<TModel> page) where TModel : class
        {
            return GetLinkValue<TModel>(
               generator,
               pathBase,
               actionRouteName,
               page);
        }

        public static string GetPageLinkValue<T1,T2, TModel>(
            this IAsyncCommand<T1,T2> command,
            LinkGenerator generator,
            string pathBase,
            string actionRouteName,
            PageResult<TModel> page) where TModel : class
        {
            return GetLinkValue<TModel>(
               generator,
               pathBase,
               actionRouteName,
               page);
        }

        public static string GetPageLinkValue<T1, TModel>(
            this IAsyncCommand<T1> command, 
            LinkGenerator generator, 
            string pathBase,
            string actionRouteName,
            PageResult<TModel> page) where TModel:class
        {
            return GetLinkValue<TModel>(
               generator,
               pathBase,
               actionRouteName,
               page);
        }

        private static string GetLinkValue<TModel>(LinkGenerator generator,
            string pathBase,
            string actionRouteName,
            PageResult<TModel> page) where TModel:class{
            var values = new List<string>(4);
            if (page.HasNextPage)
            {
                values.Add(GetLinkValueItem(
                    generator,
                    pathBase,
                    actionRouteName,
                    "next",
                    page.Page + 1,
                    page.Count));
            }
            if (page.HasPreviousPage)
            {
                values.Add(GetLinkValueItem(
                    generator,
                    pathBase,
                    actionRouteName,
                    "previous",
                    page.Page - 1,
                    page.Count));
            }
            values.Add(GetLinkValueItem(
                 generator,
                 pathBase,
                 actionRouteName,
                "first",
                1,
                page.Count));
            values.Add(GetLinkValueItem(
                 generator,
                 pathBase,
                 actionRouteName,
                 "last",
                 page.TotalPages,
                 page.Count));
            return string.Join(", ", values);
        }

        private static string GetLinkValueItem(
            LinkGenerator generator, 
            string pathBase,
            string actionRouteName,
            string rel, int page, int count)
        {
            var url = generator.GetPathByRouteValues(
                routeName: actionRouteName,
                values: new PageOptions { Page = page, Count = count },
                pathBase: pathBase);
            return $"<{url}>; rel=\"{rel}\"";
        }
    }
}
