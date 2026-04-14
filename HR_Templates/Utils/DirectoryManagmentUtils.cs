
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public static partial class DirectoryManagmentUtils {
    public const int DisposeTimeout = 5;
    const string FolderKey = "WorkSessionDirectory";
    const string PathKey = "CurrentPath";
    static readonly object modifyUserDirectoriesLocker = new object();

    public class DirectoryInfo {
        public string Name { get; set; }
        public DateTime LastUsageTime { get; set; }
    }


    
    public static string GetDocumentSampleFolderPath(HttpContext context) {
        return Path.Combine(DirectoryManagmentUtils.GetCurrentDataDirectory(context), @"Templates");
    }

    static string GetCurrentDataDirectory(HttpContext Context) {
    
        lock(modifyUserDirectoriesLocker) {
            var currentDataDirectory = Context.Session.GetString(FolderKey);
            DirectoryInfo directoryInfo = ActualDirectories.Where(i => i.Name == currentDataDirectory).SingleOrDefault();
            if(directoryInfo == null || (Context.Session.GetString(PathKey) != Context.Request.Path && Context.Request.Method == "GET")) {
                currentDataDirectory = CreateNewFolder();
                Context.Session.SetString(PathKey, Context.Request.Path);
                Context.Session.SetString(FolderKey, currentDataDirectory);
                directoryInfo = new DirectoryInfo { Name = currentDataDirectory, LastUsageTime = DateTime.Now };
                ActualDirectories.Add(directoryInfo);

                PurgeOldUserDirectories();
            } else {
                directoryInfo.LastUsageTime = DateTime.Now;
            }
            return currentDataDirectory;
        }
    }

    static IList<DirectoryInfo> actualDirectories;
    static IList<DirectoryInfo> ActualDirectories {
        get {
            if(actualDirectories == null)
                actualDirectories = new List<DirectoryInfo>();
            return actualDirectories;
        }
    }
    static string RootFilesPath { get { return "App_Data"; } }
    static string InitialFilesPath { get { return Path.Combine(RootFilesPath, "Docs"); } }

    static string CreateNewFolder() {
        string FilesDirectoty = GenerateFilesFolderName();
        CopyFiles(InitialFilesPath, FilesDirectoty);
        return FilesDirectoty;
    }
    static void CopyFiles(string sourceFilePath, string destinationPath) {
        IEnumerable<string> documentFileCollection = GetFilesInDirectory(sourceFilePath, "*.xlsx", "*.xls", "*.csv", "*.docx", "*.doc", "*.rtf", "*.txt");
        if(!Directory.Exists(destinationPath))
            Directory.CreateDirectory(destinationPath);
        foreach(var filePath in documentFileCollection) {
            string destinationFile = Path.Combine(destinationPath, Path.GetFileName(filePath));
            File.Copy(filePath, destinationFile, true);
            File.SetAttributes(destinationFile, FileAttributes.Normal);
        }

        foreach(string directoryPath in Directory.GetDirectories(sourceFilePath)) {
            string directoryName = Path.GetFileName(directoryPath);
            CopyFiles(directoryPath, Path.Combine(destinationPath, directoryName));
        }
    }
    static IEnumerable<string> GetFilesInDirectory(string path, params string[] allowedExtensions) {
        IEnumerable<string> documentFileCollection = new string[0];
        foreach(string extension in allowedExtensions) {
            documentFileCollection = documentFileCollection.Concat(Directory.GetFiles(path, extension));
        }
        return documentFileCollection;
    }
    static string GenerateFilesFolderName() {
        string currentFolder = null;
        while(string.IsNullOrEmpty(currentFolder) || Directory.Exists(Path.Combine(RootFilesPath, currentFolder))) {
            currentFolder = Guid.NewGuid().ToString();
        }
        return Path.Combine(RootFilesPath, currentFolder);
    }
    public static void PurgeOldUserDirectories() {
        //if(!Utils.IsSiteMode)
        //    return;

        lock(modifyUserDirectoriesLocker) {
            string[] existingDirectories = Directory.GetDirectories(RootFilesPath);
            foreach(string directoryPath in existingDirectories) {
                Guid guid = Guid.Empty;
                if(!Guid.TryParse(Path.GetFileName(directoryPath), out guid)) continue;

                DirectoryInfo directoryInfo = ActualDirectories.Where(i => i.Name == directoryPath).SingleOrDefault();
                if(directoryInfo == null || (DateTime.Now - directoryInfo.LastUsageTime).TotalMinutes > DisposeTimeout) {
                    Directory.Delete(directoryPath, true);
                    if(directoryInfo != null)
                        ActualDirectories.Remove(directoryInfo);
                }
            }
        }
    }


    public static string GetSessionFolderKey(HttpContext Context)
    {
        var currentDataDirectory = Context.Session.GetString(FolderKey);
        return currentDataDirectory;
    }
}

