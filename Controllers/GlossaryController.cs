namespace GlossarySystem.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using GlossarySystem.Models;
    using GlossarySystem.Helpers;
    using System.Web.Configuration;

    [RoutePrefix("api/Glossary")]
    public class GlossaryController : ApiController
    {
        private readonly string XMLFilePath = System.Web.Hosting.HostingEnvironment.MapPath(WebConfigurationManager.AppSettings["XMLStoreFilePath"]);


        [HttpGet, Route("")]
        public List<GlossaryEntry> GetGlossary()
        {
            var result = XmlHelper.FromXmlFile<Glossary>(XMLFilePath);
            var glossaryEntries = result.GlossaryEntries
                .OrderBy(x => x.Term)
                .ThenBy(x => x.Definition)
                .ToList();
            return glossaryEntries;
        }

        [HttpGet, Route("{glossaryId}")]
        public GlossaryEntry GetGlossaryEntry(Guid glossaryId)
        {
            var result = XmlHelper.FromXmlFile<Glossary>(XMLFilePath);
            var glossaryEntry = result.GlossaryEntries.FirstOrDefault(record => record.Id == glossaryId);
            return glossaryEntry;
        }

        [HttpPost, Route("Create")]
        public IHttpActionResult Create(GlossaryEntryEditModel glossaryEntry)
        {
            if (ModelState.IsValid)
            {
                var result = XmlHelper.FromXmlFile<Glossary>(XMLFilePath);
                var newEntry = result.Create(glossaryEntry);
                XmlHelper.ToXmlFile(result, XMLFilePath);
                return Ok(newEntry);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut, Route("Update")]
        public IHttpActionResult Update(GlossaryEntryEditModel glossaryEntry)
        {
            if (ModelState.IsValid)
            {
                var result = XmlHelper.FromXmlFile<Glossary>(XMLFilePath);
                var updatedEntry = result.Update(glossaryEntry);
                XmlHelper.ToXmlFile(result, XMLFilePath);
                return Ok(updatedEntry);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete, Route("{glossaryId}")]
        public IHttpActionResult Delete(Guid glossaryId)
        {
            var result = XmlHelper.FromXmlFile<Glossary>(XMLFilePath);
            var glossaryEntry = result.GlossaryEntries.FirstOrDefault(record => record.Id == glossaryId);
            if (glossaryEntry == null)
            {
                return BadRequest("Cannot find this term");
            }

            result.Delete(glossaryEntry);
            XmlHelper.ToXmlFile(result, XMLFilePath);
            return Ok();
        }
    }
}
