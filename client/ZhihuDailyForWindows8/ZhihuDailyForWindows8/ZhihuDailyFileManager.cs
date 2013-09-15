/**
 * Author : ihainan
 * E-mail : ihainan72@gmail.com
 * Created Time : 2013/09/08 17:21
 * Website : http://www.ihainan.me
 * Reference : http://www.dotnetcurry.com/ShowArticle.aspx?ID=838
 **/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ZhihuDailyForWindows8
{
    class ZhihuDailyFileManager
    {   
        StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        public async Task<StorageFile> openFile(string filename)
        {
            try
            {
                StorageFile file = await storageFolder.GetFileAsync(filename);
                return file;
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            
        }

        // check whether a file was existed
        public async Task<bool> isExisted(string filename)
        {
            StorageFile file = await openFile(filename);
            if (file == null)
                return false;
            else
                return true;
        }

        
        // save the special content into the file
        public async void writeFile(string filename, string content)
        {

            StorageFile file = await openFile(filename);
            // StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
            file = null;
            if (file != null)
            {
                await Windows.Storage.FileIO.WriteTextAsync(file, content);
            }
            else
            {
                file = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(file, content);
            }
        }

        // delete a file
        public async void deleteFile(string filename)
        {
            StorageFile file = await openFile(filename);
            if(file != null)
                await file.DeleteAsync();
        }

        // load the content from a file
        public async Task<string> loadFile(string filename)
        {
            StorageFile file = await openFile(filename);
            string result = "";
            if (file != null)
            {
                using (IRandomAccessStream sessionRandomAccess =
                await file.OpenAsync(FileAccessMode.Read))
                {
                    if (sessionRandomAccess.Size > 0)
                    {
                        result = await Windows.Storage.FileIO.ReadTextAsync(file);
                        return result;
                    }
                    else
                    {
                        // empty file
                        return null;
                    }
                }
            }
            return null;
        }

        // download file from internet
        public async Task<string> downloadFile(string url, string filename = null)
        {
            // get the filename from the url
            if (filename == null)
            {
                Uri uri = new Uri(url);
                filename = System.IO.Path.GetFileName(uri.LocalPath);
            }
            
            // Write Image File
            if(!(await isExisted(filename))){
                HttpClient httpClient = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                HttpResponseMessage response;
                try
                {
                    response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                }
                catch (Exception e)
                {
                    return null;
                }
                StorageFile imageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename,
                    CreationCollisionOption.ReplaceExisting);
                using (IRandomAccessStream fs = await imageFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    DataWriter writer = new DataWriter(fs.GetOutputStreamAt(0));
                    writer.WriteBytes(await response.Content.ReadAsByteArrayAsync());
                    await writer.StoreAsync();
                    writer.Dispose();
                }
            }

            
            return filename;
        }
            

        public async Task<BitmapImage> LoadImage(string filename)
        {
            StorageFile file = await openFile(filename);
            if((await isExisted(filename)) == true){
                using (FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(stream);
                    return bitmapImage;
                }
            }
            else
                return null;
        }
    }
}
