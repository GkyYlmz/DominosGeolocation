using DominosGeolocation.Data.Models;
using DominosGeolocation.Helper;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DominosGeolocation.Consumer
{
    public class Manager
    {
        public string GetFileOutputOperation()
        {
            string folderRootName = @"C:\DominosCoordinates";
            bool exists = Directory.Exists(folderRootName);

            if (!exists)
                Directory.CreateDirectory(folderRootName);

            string fileRootAndName = @"C:\DominosCoordinates\Output.txt";
            if (!File.Exists(fileRootAndName))
            {
                FileInfo fi = new FileInfo(fileRootAndName);
            }

            return fileRootAndName;
        }

        public Task ProcessWrite(DestinationSource locationWriteDataQueue, string filePath)
        {
            var distance = DistanceCalculate(locationWriteDataQueue.SourceLatitude.ToDouble(),
                                                      locationWriteDataQueue.SourceLongitude.ToDouble(),
                                                      locationWriteDataQueue.DestinationLatitude.ToDouble(),
                                                      locationWriteDataQueue.DestinationLongitude.ToDouble(),
                                                      'K'
                                                     );

            var fileTextData = Math.Round(locationWriteDataQueue.SourceLatitude.ToDouble(), 10).ToString() + "\t" +
                               Math.Round(locationWriteDataQueue.SourceLongitude.ToDouble(), 10).ToString() + "\t" +
                               Math.Round(locationWriteDataQueue.DestinationLatitude.ToDouble(), 10).ToString() + "\t" +
                               Math.Round(locationWriteDataQueue.DestinationLongitude.ToDouble(), 10).ToString() + "\t" +
                               Math.Round(distance, 3).ToString() + "\r\n";
            return WriteTextAsync(filePath, fileTextData);
        }

        private async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }

        private double Deg2Rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private double Rad2Deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        private double DistanceCalculate(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            if ((lat1 == lat2) && (lon1 == lon2))
            {
                return 0;
            }
            else
            {
                double theta = lon1 - lon2;
                double dist = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2)) + Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) * Math.Cos(Deg2Rad(theta));
                dist = Math.Acos(dist);
                dist = Rad2Deg(dist);
                dist = dist * 60 * 1.1515;
                if (unit == 'K')
                {
                    dist = dist * 1.609344;
                }
                else if (unit == 'N')
                {
                    dist = dist * 0.8684;
                }
                return (dist);
            }
        }
    }
}
