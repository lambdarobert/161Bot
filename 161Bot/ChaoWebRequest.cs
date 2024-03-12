using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot
{
    //handles the boilerplate code behind web requests
    public class ChaoWebRequest
    {
        private static readonly HttpClient client = new HttpClient();
        public string Url { get; private set; }

        public class Response
        {
            public bool IsSuccess { get; private set; }
            public string Text { get; private set; }
            protected internal Response(string text = "", bool isSuccess = false)
            {
                this.IsSuccess = isSuccess;
                this.Text = text;
            }
        }

        public class JSONResponse<T>
        {
            public bool IsSuccess { get; private set; }
            
            [AllowNull]
            public T Object { get; private set; }
            protected internal JSONResponse(bool isSuccess)
            {
                this.IsSuccess = isSuccess;
            }

            protected internal JSONResponse(bool isSuccess, T jsonObject)
            {
                this.IsSuccess = isSuccess;
                this.Object = jsonObject;
            }
        }

        public ChaoWebRequest(string url)
        {
            this.Url = url;
        }

        public async Task<JSONResponse<T>> ToClassFromJSON<T>()
        {
            Console.WriteLine("Called");
            try
            {
                HttpResponseMessage response = await client.GetAsync(Url);
                response.EnsureSuccessStatusCode();
                string responseBody = await (response.Content.ReadAsStringAsync());
                Console.WriteLine("IT WORKS");
                return new JSONResponse<T>(true,  await response.Content.ReadAsAsync<T>());
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("EXCPETION WAS " + e.ToString());
                return new JSONResponse<T>(false);
            }
        }

        public async Task<Response> Get()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(Url);
                response.EnsureSuccessStatusCode();
                string responseBody = await (response.Content.ReadAsStringAsync());
                return new Response(responseBody, true);
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("EXCPETION WAS " + e.ToString());
                return new Response();
            }
        }
    }
}
