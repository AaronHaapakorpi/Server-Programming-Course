using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;

namespace BikeApiTest
{
    static class Program
    {
        private static async Task Main(string[] args)
        {
            bool online = true;
            if(args.Length>1)
            {
                try
                {
                online = Boolean.Parse(args[1]);
                }

                catch(Exception e){
                    Console.WriteLine(e.Message);
                }
            }

            for (int i = 0; i < args.Length; i++)
            {
                try
                {
                    if (Convert.ToChar(args[0][i]) >= '0' && Convert.ToChar(args[0][i]) <= '9')
                    {
                        throw new System.ArgumentException("String contains a number");
                    }
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Invalid argument\n" + e.Message);
                    return;
                }
            }

            int bikeCount;
            if (online)
            {
                try
                {
                    RealTimeCityBikeDataFetcher fetcher = new RealTimeCityBikeDataFetcher();
                    bikeCount = await fetcher.GetBikeCountInStation(args[0]).ConfigureAwait(false);
                    Console.WriteLine("Online bike fetcher found that " + args[0] + " contains " + bikeCount + " bikes");
                }
                catch (NotFoundException e)
                {
                    Console.WriteLine("Not found\n" + e.Message);
                }
            }
            else
            {
                OfflineCityBikeDataFetcher xd = new OfflineCityBikeDataFetcher();
                bikeCount = await xd.GetBikeCountInStation(args[0]).ConfigureAwait(false);
                if (bikeCount != -1) Console.WriteLine("offline bike fetcher found that " + args[0] + " contains " + bikeCount + " bikes.");
                else Console.WriteLine("Bike location " +args[0] + " not found");
            }

        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(){}

        public NotFoundException(string name)
        :base (String.Format("Station {0} was not found", name)){}

        public NotFoundException(string message, Exception innerException)  :base (String.Format("Station {0} was not found", message),innerException)
        {
        }
    }
    public interface ICityBikeDataFetcher
    {
        Task<int> GetBikeCountInStation(string stationName);
    }

    public class RealTimeCityBikeDataFetcher : ICityBikeDataFetcher

    {
        public async Task<int> GetBikeCountInStation(string stationName)
        {
           using HttpClient client = new HttpClient();
           var result = await client.GetStringAsync("http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental").ConfigureAwait(false);

           BikeRentalStationList rentallist = new BikeRentalStationList();
           try
           {
               rentallist=  JsonConvert.DeserializeObject<BikeRentalStationList>(result);
           }
           catch(Exception e)
           {
               Console.WriteLine(e.Message);
               return -1;

           }

           foreach(var station in rentallist.stations){
               if(station.Name == stationName){
                   return Int32.Parse(station.BikesAvailable);
               }
           }
           throw new NotFoundException(stationName);
        }
    }
    public class OfflineCityBikeDataFetcher : ICityBikeDataFetcher
    {
        public async Task<int> GetBikeCountInStation(string stationName)
        {
            String line;
            StreamReader sr = new StreamReader("bikedata.txt");
            line = sr.ReadLine();
            try
            {
                while(line !=null)
                {
                    string countString="";
                    string stationString ="";
                    countString = line.Substring(line.LastIndexOf(':')+1);
                    stationString = line.Substring(0,line.IndexOf(":"));
                    stationString = stationString.Remove(stationString.Length-1);

                    int count = Int32.Parse(countString);

                    if(String.Equals(stationString,stationName))
                    {
                        return count;
                    }
                    line = sr.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        
            return -1;
        }
    }
}
