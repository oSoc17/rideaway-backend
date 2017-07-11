using Itinero.Navigation.Language;
using System.Collections.Generic;
using System.IO;

namespace rideaway_backend.Instance {
    public class CsvLanguageReference : ILanguageReference {
        private Dictionary<string, string> Dictionary;
        private FileInfo FileInfo;

        public CsvLanguageReference(FileInfo FileInfo){
            this.FileInfo = FileInfo;
            initialize();
        }

        public void initialize(){
            Dictionary = new Dictionary<string, string>();
            using (var reader = new StreamReader(FileInfo.OpenRead())){
                while (!reader.EndOfStream){
                    var line = reader.ReadLine();
                    var translation = line.Split(',');
                    Dictionary.Add(translation[0], translation[1]);
                }
            }
        }
        
        public string this[string value]
        {
            get {
                if (Dictionary.ContainsKey(value)){
                    return Dictionary[value];
                } 
                else{
                    return value;
                }
            }
                
        }

    }
}