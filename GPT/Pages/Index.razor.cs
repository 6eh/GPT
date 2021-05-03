using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using GPT.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace GPT.Pages
{
    public partial class Index
    {
        private string Label { get; set; } = "";

        private bool Waiting = false;
        private string ResponseStatusCode { get; set; }

        //class GenPosts
        //{
        //    public string Content { get; set; }
        //    public string Header { get; set; } = "";
        //    public string Status { get; set; } = "";
        //}

        //List<string> LS = new List<string>();

        private List<PostStatus> LS = new();

        private AuthenticationState _context;

        private class GPTclass
        {
            public string InputText { get; set; } = "";
            public string Response { get; set; } = "";
            public string Predictions { get; set; }
        }

        private GPTclass GPTmodel = new GPTclass();

        class JSON
        {
            public string Predictions { get; set; }
        }

        //private string FindText { get; set; } = "";


        private async Task HandleValidSubmit()
        {
            Label = "";
            //Console.WriteLine("-" + GPTmodel.InputText + "-");
            if (GPTmodel.InputText.Any() && GPTmodel.InputText != "")
            {
                try
                {
                    //Console.WriteLine("!=null");
                    Label = "Ждём ответа от нейросети ...";
                    Waiting = true;
                    //await Task.Delay(1000);
                    //WaitAnimation();
                    //await LabelWright();
                    await Task.Delay(1000);
                    await POST();
                    await Task.Delay(100);
                    Label = "";
                    //StateHasChanged();  // Отрисовка в UI
                    
                }
                catch (Exception ex)
                {
                    Label = ex.Message;
                    //Console.WriteLine("ex.Message: " + ex.Message);
                }
                //Label = "ResponseStatusCode (" + ResponseStatusCode + ")";
                Waiting = false;
            }
            else
            {
                //Console.WriteLine("=null");
                Label = "Напишите что-нибудь!";
                ////StateHasChanged();  // Отрисовка в UI
            }
        }

        private async Task WaitAnimation()
        {
            Waiting = true;
            string _label = "Ждём ответа от нейросети ";
            string _labelPlus = "";
            int i = 0;
            while (true)
            {
                i++;
                _labelPlus += ".";
                if (i > 8)
                {
                    i = 0;
                    _labelPlus = "";
                }
                Label = _label + _labelPlus;
                await Task.Delay(30);
                Console.WriteLine(Label);
                ////StateHasChanged();  // Отрисовка в UI
                if (ResponseStatusCode != null)
                {
                    Waiting = false;
                    break;
                }
            }
        }

        private async Task LabelWright()
        {
            Label = "Ждём ответа от нейросети ...";
        }
        private async Task POST()
        {
            var url = "https://api.aicloud.sbercloud.ru/public/v1/public_inference/gpt3/predict";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.ContentType = "application/json";

            var data = @"{ ""text"": """ + GPTmodel.InputText + @"""}";

            //GPTmodel.Response = data;

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                //GPTmodel.Response = result;

                JSON jsonResponse = JsonConvert.DeserializeObject<JSON>(result);

                GPTmodel.Response = jsonResponse.Predictions;
                //label = "";
            }
            ResponseStatusCode = httpResponse.StatusCode.ToString();

            PostStatus GP = new() { Content = GPTmodel.Response, Header = GPTmodel.InputText };
            LS.Reverse();
            //LS.Add(GPTmodel.Response);
            LS.Add(GP);
            LS.Reverse();
            //Console.WriteLine(httpResponse.StatusCode);
            //GPTmodel.Response = httpResponse.StatusCode.ToString();
        }

        [Authorize(Roles = "Admin")]
        private void PublishButtonClick(int index)
        {
            //Indexo = index.ToString();
            SaveOrPublishPost(index, true);
        }

        private void SaveButtonClick(int index)
        {
            SaveOrPublishPost(index, false);
        }

        private void SaveOrPublishPost(int index, bool published = false)
        {
            Guid guId = Guid.NewGuid(); // Генерация ID поста

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Post post = new Post()
                {
                    Id = guId.ToString(),
                    Header = LS.ElementAt(index).Header,
                    Content = LS.ElementAt(index).Content,
                    Date = DateTime.Now,
                    Published = published,
                    UserId = _context.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Rating = 0
                };
                //Console.WriteLine("Id: {0}, Content: {1}", post.Id, post.Content);

                db.Posts.Add(post);
                db.SaveChanges();

                string _status;
                if (published)
                    _status = "Опубликовано";
                else
                    _status = "Сохранено";
                    
                LS.ElementAt(index).Status = _status;
            }
        }
    }
}
