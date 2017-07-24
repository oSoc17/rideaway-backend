using System.Collections.Generic;
using System.IO;
using Itinero.Navigation.Language;

namespace rideaway_backend.Instance {
    /// <summary>
    /// Static instance of all the language references.
    /// </summary>
    public static class Languages {
        private static Dictionary<string, ILanguageReference> languages;

        /// <summary>
        /// Initialise the language references for the supported languages.
        /// </summary>
        public static void initialize () {
            languages = new Dictionary<string, ILanguageReference> ();
            //TODO make more abstract
            languages.Add ("en", new DefaultLanguageReference ());
            languages.Add ("nl", new CsvLanguageReference (new FileInfo (@"language/nl_instructions.csv")));
            languages.Add ("fr", new CsvLanguageReference (new FileInfo (@"language/fr_instructions.csv")));
        }

        /// <summary>
        /// Get a language reference.
        /// </summary>
        /// <param name="key">key of the language reference to get.</param>
        /// <returns>Language reference corresponding to the given key.</returns>
        public static ILanguageReference GetLanguage (string key) {
            return languages[key];
        }

    }
}