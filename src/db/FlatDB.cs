using System.IO;

namespace GameServer
{
    class FlatDB
    {
        string path;
        FileStream? fileStream;
        StreamReader? streamReader;
        StreamWriter? streamWriter;
        List<string> file;

        public FlatDB(string fileName) {
            path = "C:/Temp/" + fileName + ".txt";
            if(!File.Exists(path)) {
                fileStream = File.Create(path);
            } else {
                fileStream = File.Open(path, FileMode.Open);
            }
            streamWriter = new StreamWriter(fileStream);
            streamReader = new StreamReader(fileStream);
            file = new List<string>();
        }

        public int getKeyIndex(string key) {
            for(int i = 0; i < file.Count; i++) {
                if(file[i].Split(':')[0] == key) {
                    return i;
                }
            }
            return -1;
        }

        public void setValue(string key, string value) {
            int index = getKeyIndex(key);
            if(index == -1) {
                file.Add(key + ":" + value + "\n");
            } else {
                file[index] = key + ":" + value + "\n";
            }
        }

        public string? getValue(string key) {
            int index = getKeyIndex(key);
            if(index == -1) {
                return "";
            } else {
                return file[index].Split(':')[1];
            }
        }

        public string getAllData() {
            string data = "";
            foreach(string line in file) {
                data += line;
            }
            return data;
        }

        public void save() {
            foreach(string line in file) {
                streamWriter?.Write(line);
                streamWriter?.Flush();
            }
        }       
    }
}