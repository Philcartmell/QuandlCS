using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using QuandlCS.Interfaces;
using QuandlCS.Utilities;

namespace QuandlCS.Connection
{
    /// <summary>
    /// 
    /// </summary>
    public class QuandlConnection : IQuandlConnection, IDisposable
    {

        private bool _disposed;
        private HttpClient _httpClient;

        public QuandlConnection()
        {
            _httpClient = new HttpClient();
        }

        #region IQuandlConnection Members

        private async Task<string> GetAsync(IQuandlRequest request)
        {
            return await _httpClient.GetStringAsync(request.ToRequestString());
        }

        private async Task<string> PostAsync(IQuandlUploadRequest request)
        {
            throw new InvalidOperationException("THIS DOESN'T WORK AT THE MOMENT");

            //string data = string.Empty;
            //using (WebClient client = new WebClient())
            //{
            //  string requestString = request.GetPOSTRequestString();
            //  string requestData = request.GetData();
            //  data = client.UploadString(requestString, "POST", requestData);
            //}
            //return data;
        }

        private string Post(IQuandlUploadRequest request)
        {
            return AsyncUtilities.RunSync(() => PostAsync(request));
        }

        public string Request(IQuandlRequest request)
        {
            return AsyncUtilities.RunSync(() => RequestAsync(request));
        }


        public async Task<string> RequestAsync(IQuandlRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request as IQuandlUploadRequest != null)
            {
                return Post(request as IQuandlUploadRequest);
            }

            if (request is IQuandlRequest) // currently always true.
            {
                return await GetAsync(request);
            }

            throw new ArgumentException("The request supplied is not of a valid type", nameof(request));
        }

        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _httpClient.Dispose();
                _httpClient = null;
            }

        }
    }
}
