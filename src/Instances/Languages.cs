using System.Collections.Generic;
using Itinero.Navigation.Language;
using System.IO;

namespace rideaway_backend.Instance {
    public static class Languages {
        private static Dictionary<string, ILanguageReference> languages;

        public static void initialize(){
            languages = new Dictionary<string, ILanguageReference>();
            //TODO make more abstract
            languages.Add("en", new DefaultLanguageReference());
            languages.Add("nl", new CsvLanguageReference(new FileInfo(@"language/nl_instructions.csv")));
            languages.Add("fr", new CsvLanguageReference(new FileInfo(@"language/fr_instructions.csv")));
        }

        public static ILanguageReference GetLanguage(string key){
            return languages[key];
        }

    }
}