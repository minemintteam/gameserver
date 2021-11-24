using System.IO;

namespace GameServer
{
    class FlatDB
    {
        string path;
        FileStream? fileStream;
        StreamReader? streamReader;
        StreamWriter? streamWriter;
        
        public FlatDB(string fileName) {
            path = "C:/Temp/" + fileName + ".txt";
            if(!File.Exists(path)) {
                fileStream = File.Create(path);
                streamWriter = new StreamWriter(fileStream);
                streamReader = new StreamReader(fileStream);
            } else {
                fileStream = File.Open(path, FileMode.Open);
                streamWriter = new StreamWriter(fileStream);
                streamReader = new StreamReader(fileStream);
            }
        }

        public void Write(string data) {
            var key = data.Split(':')[0];
            if(Read(key) != null) {
                Replace(key, data);
            } else {
                streamWriter?.WriteLine(data);
                streamWriter?.Flush();
            }
        }

        public void Log(string data) {
            string? line = null;
            while((line = streamReader?.ReadLine()) != null) {
                streamWriter?.WriteLine(data);
                streamWriter?.Flush();
            }
        }

        public string? Read(string searchString) {
            string? line = null;
            while((line = streamReader?.ReadLine()) != null) {
                if(line.Contains(searchString)) {
                    return line;
                }
            }
            return null;
        }

        private void Replace(string searchString, string replaceString) {
            string? line;
            while((line = streamReader?.ReadLine()) != null) {
                if(line.Contains(searchString)) {
                    streamWriter?.WriteLine(replaceString);
                } else {
                    streamWriter?.WriteLine(line);
                }
            }
        }

        public void Close() {
            fileStream?.Close();
        }
    }
}