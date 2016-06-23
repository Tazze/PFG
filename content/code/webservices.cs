class WebServices
    {
        private static string server = "";
        private static HttpClient client;

        public WebServices()
        {
            server = "http://localhost/";
            client = new HttpClient();
        }
        public WebServices(string ser)
        {
            server = ser;
            client = new HttpClient();
        }
        public async Task<HttpResponseMessage> PostPictureAsync(Stream data)
        {
            string address = server + "MirrorClient/GetUserInfoStream";
            Uri url = new Uri(address);
            data.Seek(0, SeekOrigin.Begin);
            StreamContent stream = new StreamContent(data);
            HttpResponseMessage result = await client.PostAsync(url, stream);
            return result;
        }
    }