using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Wrap;
using System.Linq;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Resilience.HttpInterface
{
    public class ResilienceHttpClient : IHttpClient
    {
        private readonly HttpClient httpClient;
        private Func<string, IEnumerable<Policy>> policyCreator;
        // 把policy 打包成PolicyWrap 进行本地缓存
        private readonly ConcurrentDictionary<string, PolicyWrap> policyWraps;
        private readonly ILogger<ResilienceHttpClient> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        public ResilienceHttpClient(Func<string, IEnumerable<Policy>> _policyCreator, ILogger<ResilienceHttpClient> _logger, IHttpContextAccessor _httpContextAccessor)
        {
            httpClient = new HttpClient();
            policyCreator = _policyCreator;
            policyWraps = new ConcurrentDictionary<string, PolicyWrap>();
            logger = _logger;
            httpContextAccessor = _httpContextAccessor;
        }
        public  Task<HttpResponseMessage> PostAsync<T>(string url, T item, string authorizationToken = null, string requestId = "", string authorizationMethod = "Bearer")
        {
            HttpRequestMessage httpRequestMessageFunc() => CreateHttpRequestMessage(HttpMethod.Post, url, item);
            return this.DoPostAsync(HttpMethod.Post, url, httpRequestMessageFunc, authorizationToken, requestId, authorizationMethod);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, Dictionary<string, string> form, string authorizationToken = null, string requestId = "", string authorizationMethod = "Bearer")
        {
            HttpRequestMessage httpRequestMessageFunc() => CreateHttpRequestMessage(HttpMethod.Post, url, form);
            return await this.DoPostAsync(HttpMethod.Post, url, httpRequestMessageFunc, authorizationToken, requestId, authorizationMethod);
        }
        private Task<HttpResponseMessage> DoPostAsync(HttpMethod method, string url, Func<HttpRequestMessage> httpRequestMessageFunc, string authorizationToken, string requestId = "", string authorizationMethod = "Bearer")
        {   
            //判断请求是否时Post请求或者Put请求
            if (method != HttpMethod.Post || method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or  put ", nameof(method));
            }
            var origin = GetOriginFromUri(url);
            //进行Http调用
            return HttpInvoker(origin, async (x) =>
            {
                HttpRequestMessage requestMessage = httpRequestMessageFunc();
                if (requestMessage == null)
                {
                    logger.LogError($"requestMessage为空,类名：{nameof(ResilienceHttpClient)},方法名：{nameof(HttpInvoker)}");
                }
                this.SetAuthorizationHeader(requestMessage);
                if (authorizationToken != null)
                {
                    //加上token
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }
                if (requestId != null)
                {
                    //加上请求Id
                    requestMessage.Headers.Add("x-requestid", requestId);
                }
                var response = await httpClient.SendAsync(requestMessage);
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new HttpRequestException("response.StatusCode=500");
                }
                return response;
            });
        }
        /// <summary>
        /// 进行Http的调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="origin">主机地址</param>
        /// <param name="action">进行HTTP请求的具体代码</param>
        /// <returns></returns>
        private async Task<T> HttpInvoker<T>(string origin, Func<Context, Task<T>> action)
        {
            var normalizedOrigin = NormalizeOrigin(origin);
            if(!policyWraps.TryGetValue(normalizedOrigin, out PolicyWrap policyWrap))
            {
                policyWrap =  Policy.WrapAsync(policyCreator(normalizedOrigin).ToArray());
                policyWraps.TryAdd(normalizedOrigin, policyWrap);
            }
            
            return await policyWrap.ExecuteAsync(action, new Context(origin));
        }
        /// <summary>
        /// 获得host地址
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private string GetOriginFromUri(string uri)
        {
            var url = new Uri(uri);
            var origin = $"{url.Scheme}://{url.DnsSafeHost}:{url.Port}";
            return origin;
        }
        /// <summary>
        /// 规则化主机地址
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        private static string NormalizeOrigin(string origin)
        {
            //去掉所有的空格，转换为小写
            return origin?.Trim().ToLower();
        }
        /// <summary>
        /// 设置http请求的验证头
        /// </summary>
        /// <param name="requestMessage"></param>
        private void SetAuthorizationHeader(HttpRequestMessage requestMessage)
        {
            //把验证头添加进去
            var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                requestMessage.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }
        }
        private HttpRequestMessage CreateHttpRequestMessage<T>(HttpMethod method, string url, T item)
        {
            var requestMessage = new HttpRequestMessage(method, url) { Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json") };

            return requestMessage;
        }
        private HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string url, Dictionary<String, string> form)
        {
            var requestMessage = new HttpRequestMessage(method, url)
            {
                Content = new FormUrlEncodedContent(form)
            };
            return requestMessage;
        }
    }
}
