using log4net;
using SEACompliance.Core.ExceptionApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace SEACompliance.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class LogExceptionAttribute : ExceptionFilterAttribute
    {
        ILog _log = log4net.LogManager.GetLogger("LogExceptionAttribute_Info");
        //重写基类的异常处理方法
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //1.异常日志记录（正式项目里面一般是用log4net记录异常日志）
            string errorMsg = actionExecutedContext.Exception.Message + "\r\n " + actionExecutedContext.Exception.Source + "\r\n" + actionExecutedContext.Exception.StackTrace;
            errorMsg += "\r\n URL=" + actionExecutedContext.Request.RequestUri.ToString();
            Dictionary<string, object> args = actionExecutedContext.ActionContext.ActionArguments;
            if (args != null && args.Count > 0)
            {
                errorMsg += "\r\n Parameters:";
                foreach (var key in args.Keys)
                {
                    string value = args[key] == null ? "NULL" : args[key].ToString();
                    errorMsg += string.Format("\r\n {0}={1}", key, value);
                }
            }

            //2.返回调用方具体的异常信息

            if (actionExecutedContext.Exception is NotFoundException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(actionExecutedContext.Exception.Message),
                    ReasonPhrase = "error"
                };

            }
            else if (actionExecutedContext.Exception is RequestErrorException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.ExpectationFailed)
                {
                    Content = new StringContent(actionExecutedContext.Exception.Message),
                    ReasonPhrase = "error"
                };

            }
            else if (actionExecutedContext.Exception is ArgumentException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(actionExecutedContext.Exception.Message),
                    ReasonPhrase = "error"
                };

            }
            else if (actionExecutedContext.Exception is TimeoutException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.RequestTimeout)
                {
                    Content = new StringContent(actionExecutedContext.Exception.Message),
                    ReasonPhrase = "error"
                };

            }
            else if (actionExecutedContext.Exception is NotImplementedException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented)
                {
                    Content = new StringContent(actionExecutedContext.Exception.Message),
                    ReasonPhrase = "error"
                };

            }
            else
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(actionExecutedContext.Exception.Message),
                    ReasonPhrase = "error"
                };

            }
            _log.Error(errorMsg);
            base.OnException(actionExecutedContext);
        }

    }
}