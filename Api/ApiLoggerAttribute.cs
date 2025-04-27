using ArtiConnect.DataAccess;
using ArtiConnect.Entities;
using ArtiConnect.Managers;
using NuGet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters; 

namespace ArtiConnect.Api
{
    public class ApiLoggerAttribute : ActionFilterAttribute
    {
        // Track the last logged request to prevent duplicates
        private static string _lastRequestId = string.Empty;
        private static readonly object _lockObject = new object();

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                var controller = actionExecutedContext.ActionContext.ControllerContext.Controller;
                if (controller != null)
                {
                    // Generate a request ID
                    string requestId = $"{actionExecutedContext.Request.GetCorrelationId()}";

                    // Use a lock to ensure thread safety
                    lock (_lockObject)
                    {
                        // Skip if this is a duplicate of the last request
                        if (requestId == _lastRequestId)
                        {
                            return;
                        }

                        _lastRequestId = requestId;
                    }

                    string endpoint = actionExecutedContext.Request.RequestUri.PathAndQuery;
                    string method = actionExecutedContext.Request.Method.ToString();
                    int statusCode = actionExecutedContext.Response != null ? (int)actionExecutedContext.Response.StatusCode : 500;

                    string responseContent = "No content";
                    if (actionExecutedContext.Response?.Content != null)
                    {
                        responseContent = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
                    }

                    string requestContent = "No content";
                    if (actionExecutedContext.Request.Content != null &&
                        (method == "POST" || method == "PUT" || method == "PATCH"))
                    {
                        try
                        {
                            // Make sure the content is buffered
                            actionExecutedContext.Request.Content.LoadIntoBufferAsync().Wait();

                            // Read the content as string
                            requestContent = actionExecutedContext.Request.Content.ReadAsStringAsync().Result;

                            // If content is empty, try to read it from the beginning
                            if (string.IsNullOrEmpty(requestContent))
                            {
                                var contentStream = actionExecutedContext.Request.Content.ReadAsStreamAsync().Result;
                                if (contentStream.CanSeek)
                                {
                                    contentStream.Position = 0;
                                    using (var reader = new StreamReader(contentStream, Encoding.UTF8, true, 1024, true))
                                    {
                                        requestContent = reader.ReadToEnd();
                                    }
                                }
                            }

                            // If still empty, try to get content from properties
                            if (string.IsNullOrEmpty(requestContent))
                            {
                                var objectContent = actionExecutedContext.Request.Content as ObjectContent;
                                if (objectContent != null)
                                {
                                    requestContent = $"Object of type: {objectContent.ObjectType.Name}";
                                    if (objectContent.Value != null)
                                    {
                                        requestContent += $", Value: {objectContent.Value}";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            requestContent = $"Error reading request: {ex.Message}";
                        }
                    }

                    using (var db = new AppDbContext())
                    {
                        var apiLog = new Entities.ApiLog
                        {
                            Timestamp = DateTime.Now,
                            Endpoint = endpoint,
                            Method = method,
                            RequestData = requestContent,
                            ResponseData = responseContent,
                            StatusCode = statusCode
                        };

                        db.ApiLogs.Add(apiLog);
                        db.SaveChanges();

                        // Notify the UI about the new log
                        ApiLogManager.NotifyLogAdded(apiLog);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"API log error: {ex.Message}");
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}