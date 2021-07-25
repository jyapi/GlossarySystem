namespace GlossarySystem.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using GlossarySystem.Models;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class HomeController : Controller
    {
        private readonly string baseUrl = WebConfigurationManager.AppSettings["baseApiUrl"] + "glossary/";

        public ActionResult Index()
        {
            IEnumerable<GlossaryEntry> glossaryEntries = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                //HTTP GET
                var responseTask = client.GetAsync("");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<GlossaryEntry>>();
                    readTask.Wait();

                    glossaryEntries = readTask.Result;
                }
                else //web api sent error response 
                {
                    glossaryEntries = Enumerable.Empty<GlossaryEntry>();

                    ModelState.AddModelError(string.Empty, "Server error. Please try again.");
                }
            }

            return View(glossaryEntries);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(GlossaryEntry glossaryEntry)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var postTask = client.PostAsJsonAsync<GlossaryEntry>("create", glossaryEntry);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var content = await result.Content.ReadAsStringAsync();
                    AddModelErrorMessages(content);
                }
            }

            return View(glossaryEntry);
        }

        public ActionResult Update(Guid glossaryId)
        {
            using (var client = new HttpClient())
            {
                GlossaryEntry glossaryEntry = null;
                client.BaseAddress = new Uri(baseUrl);
                //HTTP GET
                var responseTask = client.GetAsync(glossaryId.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<GlossaryEntry>();
                    readTask.Wait();
                    glossaryEntry = readTask.Result;
                    return View(glossaryEntry);
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error, please try again.");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Update(GlossaryEntry glossaryEntry)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                //HTTP POST
                var putTask = client.PutAsJsonAsync<GlossaryEntry>("update", glossaryEntry);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var content = await result.Content.ReadAsStringAsync();
                    AddModelErrorMessages(content);
                }
            }
            return View(glossaryEntry);
        }

        public ActionResult Delete(Guid glossaryId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                //HTTP DELETE
                var deleteTask = client.DeleteAsync(glossaryId.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Fail to delete this entry, please try again");
            return RedirectToAction("Index");
        }

        [HandleError]
        public ActionResult Error()
        {
            return View();
        }

        private void AddModelErrorMessages(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var obj = new { message = "", ModelState = new Dictionary<string, string[]>() };
                var errors = JsonConvert.DeserializeAnonymousType(message, obj);
                if (!errors.ModelState.Any())
                {
                    ModelState.AddModelError(string.Empty, "Server Error, please try again.");
                }
                foreach (var error in errors.ModelState)
                {
                    ModelState.AddModelError(error.Key.Substring(error.Key.LastIndexOf('.') + 1), string.Join(". ", error.Value));
                }
            }
        }
    }
}
