using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherOfCity.Models;
using WeatherOfCity.Sevices;

namespace WeatherOfCity.Attributes
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int TimeToLiveSecond;

        public CacheAttribute(int timeToLiveSecond)
        {
            TimeToLiveSecond = timeToLiveSecond;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheConfiguration = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();
            //Enabled của RedisConfiguration = true thì chạy vào middleware
            if (!cacheConfiguration.Enabled)
            {
                //Xử lý response middleware
                await next();
                return;
            }
            var cacheSevices = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheServices>();
            // lấy endpoint cho vào key , khi gọi thì tự động lấy
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cacheResponse = await cacheSevices.GetCacheResponseAsync(cacheKey);
            cacheResponse = null;
            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            var excutedContext = await next();
            //đưa dữ liệu vào cache
            if (excutedContext.Result is OkObjectResult objectResult)
                await cacheSevices.SetCacheResponseAsync(cacheKey, objectResult.Value, TimeSpan.FromSeconds(TimeToLiveSecond));
        }
        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}|");
            }
            return keyBuilder.ToString();
        }
    }
}
