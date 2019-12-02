using Boxed.AspNetCore;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ViewModels;

namespace WebApplication1
{
    public static class AsyncCommandExtentions
    {
        public static string GetPageLinkValue<TType, TModel>(
            this IAsyncCommand<TType> command, 
            LinkGenerator generator, 
            string pathBase,
            string actionRouteName,
            PageResult<TModel> page) where TModel:class
        {
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
