using System;
using System.IO;
using Xamarin.Forms;
using TodoScheduler.Android.Services;
using TodoScheduler.Infrastructure.Services.PlatformSpecificServices;

[assembly: Dependency(typeof(FileSystemService))]

namespace TodoScheduler.Android.Services
{
    public class FileSystemService : IFileSystemService
    {
        public string GetDataBasePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, "TodoDatabase.db3");
        }
    }
}