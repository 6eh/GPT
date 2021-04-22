using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace GPT.Pages
{
    public partial class Index
    {
        private string Label { get; set; } = "";
        private string Indexo { get; set; } = "0";

        private bool Waiting = false;
        private string ResponseStatusCode { get; set; }

        List<string> LS = new List<string>();

        private class GPTclass
        {
            public string InputText { get; set; }
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
            Console.WriteLine("-" + GPTmodel.InputText + "-");
            if (GPTmodel.InputText.Any())
            {
                try
                {
                    Console.WriteLine("!=null");
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
                    Console.WriteLine("ex.Message: " + ex.Message);
                }
                //Label = "ResponseStatusCode (" + ResponseStatusCode + ")";
                Waiting = false;
            }
            else
            {
                Console.WriteLine("=null");
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
            LS.Reverse();
            LS.Add(GPTmodel.Response);
            LS.Reverse();
            //Console.WriteLine(httpResponse.StatusCode);
            //GPTmodel.Response = httpResponse.StatusCode.ToString();
        }

        [Authorize(Roles = "Admin")] private void PublishButtonClick(int index)
        {
            Indexo = index.ToString();
        }

        private void SaveButtonClick()
        {

        }

    }
}
