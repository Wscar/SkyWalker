﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityServer.Dtos;
using Microsoft.Extensions.Logging;
using Resilience;
using Newtonsoft.Json;
namespace IdentityServer.Service
{
    public class AccountService : IAccountService
    {
        private readonly IHttpClient httpClient;
        private readonly ILogger<AccountService> logger;
        public AccountService(IHttpClient _httpClient,ILogger<AccountService> _logger)
        {
            httpClient = _httpClient;
            logger = _logger;
        }
        public async Task<AccountResult> SignInAsync(string userId, string passWord)
        {
         
            string url = "http://localhost:5001/api/account/signin";
            AccountResult accountResult=new AccountResult();
            try
            {
                var postData = new Dictionary<string, string>();
                postData.Add("userId", userId);
                postData.Add("passWord", passWord);
                //  var content = new FormUrlEncodedContent(postData);
                var response = await httpClient.PostAsync(url, postData);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    accountResult = JsonConvert.DeserializeObject<AccountResult>(result);
                    return accountResult;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("SignIn在重试后失败," + ex.Message + "堆栈信息：" + ex.StackTrace);
                // logger.LogWarning("SignIn在重试后失败," + ex.Message + "堆栈信息：" + ex.StackTrace);
                accountResult = new AccountResult
                {
                    Status = "登陆失败",
                    Message = $"异常信息：{ex.Message}{System.Environment.NewLine}堆栈信息{ex.StackTrace}"

                };
            }
            //登陆结果应该永远不为空
           
            return accountResult;
        }

        public Task<AccountResult> SignInByPhoneAsync(string phone, string passWord, string validationCode)
        {
            return null;
        }
    }
}
