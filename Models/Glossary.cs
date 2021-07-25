namespace GlossarySystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Glossary
    {
        public List<GlossaryEntry> GlossaryEntries { get; set; }

        public GlossaryEntry Create(GlossaryEntryEditModel glossaryEntry)
        {
            if (GlossaryEntries == null)
            {
                GlossaryEntries = new List<GlossaryEntry>();
            }
            var newGlossaryEntry = new GlossaryEntry
            {
                Id = Guid.NewGuid(),
                Term = glossaryEntry.Term,
                Definition = glossaryEntry.Definition
            };

            GlossaryEntries.Add(newGlossaryEntry);
            return newGlossaryEntry;
        }
        public GlossaryEntry Update(GlossaryEntryEditModel glossaryEntry)
        {
            GlossaryEntries.FirstOrDefault(e => e.Id == glossaryEntry.Id).Term = glossaryEntry.Term;
            GlossaryEntries.FirstOrDefault(e => e.Id == glossaryEntry.Id).Definition = glossaryEntry.Definition;
            return GlossaryEntries.FirstOrDefault(e => e.Id == glossaryEntry.Id);
        }
        public void Delete(GlossaryEntry glossaryEntry)
        {
            GlossaryEntries.Remove(glossaryEntry);
        }
    }

    public class GlossaryEntry
    {
        public Guid Id { get; set; }
        public string Term { get; set; }
        public string Definition { get; set; }
    }
}
