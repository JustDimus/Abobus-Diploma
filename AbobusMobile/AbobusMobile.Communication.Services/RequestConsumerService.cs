using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AbobusMobile.Communication.Services.Abstractions.Models;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.Communication.Services.Abstractions.Configuration;
using System.IO;

namespace AbobusMobile.Communication.Services
{
    public class RequestConsumerService : IRequestConsumerService
    {
        private HttpClient client = null;

        public RequestConsumerService(
            RequestConsumerServiceConfiguration settings)
        {
            this.client = new HttpClient();

            if (settings.UseRelativeUrls)
            {
                this.client.BaseAddress = new Uri(settings.BaseURL);
            }
        }

        public Task<BaseResponse> SendRequestAsync(BaseRequest request)
        {
            return Task.Run(async () => await this.SendRequest(request));
        }

        private async Task<BaseResponse> SendRequest(BaseRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var httpRequest = new HttpRequestMessage()
            {
                Method = this.GetHttpMethod(request.RequestType),
                Content = request.RequestDataExist
                    ? new StringContent(request.RequestData, Encoding.UTF8, request.RequestDataType)
                    : null,
                RequestUri = new Uri(request.RequestAddress, UriKind.Relative),
            })
            {
                foreach (var header in request.RequestHeaders)
                {
                    httpRequest.Headers.Add(header.Key, header.Value);
                }

                if (request is DownloadRequest)
                {
                    return await DownloadFile(httpRequest);
                }

                return await GetResponse(httpRequest);
            }
        }

        private async Task<BaseResponse> GetResponse(HttpRequestMessage message)
        {
            HttpResponse result = null;

            using (CancellationTokenSource source = new CancellationTokenSource(10000))
            {
                try
                {
                    var response = await this.client.SendAsync(message, source.Token);

                    result = new HttpResponse((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    result = new HttpResponse(-1, null)
                    {
                        Exception = ex
                    };
                }
            }

            return result;
        }

        private async Task<BaseResponse> DownloadFile(HttpRequestMessage message)
        {
            HttpStreamResponse result = null;

            using (CancellationTokenSource source = new CancellationTokenSource(120000))
            {
                try
                {
                    var response = await this.client.SendAsync(message, source.Token);

                    var downloadStream = new MemoryStream();

                    await response.Content.CopyToAsync(downloadStream);
                    downloadStream.Seek(0, SeekOrigin.Begin);

                    result = new HttpStreamResponse((int)response.StatusCode, downloadStream);
                }
                catch (Exception ex)
                {
                    result = new HttpStreamResponse(-1, null)
                    {
                        Exception = ex
                    };
                }
            }

            return result;
        }

        private HttpMethod GetHttpMethod(object requestType)
        {
            if (requestType is HttpMethod httpMethod)
            {
                return httpMethod;
            }

            throw new ArgumentException($"{nameof(requestType)} is not the HttpMethod");
        }
    }
}
