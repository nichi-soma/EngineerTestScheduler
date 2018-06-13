using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TmdbScheduler.Data;
using System.Data.Entity;

namespace TmdbScheduler
{
    class Program
    {
        private const string ApiVersion = "3";
        private const string _apiBaseURI = "https://api.themoviedb.org/"+ApiVersion;
        public static string apiKey = "49819d70df6228e01af6f2b2b5ca6f41";
        static void Main(string[] args)
        {
            try
            {
                TMDBEntities context = new TMDBEntities();
                context.Database.ExecuteSqlCommand("truncate table movies");
                UpcomingMovies();
                TopRatedMovies();
                Popular();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static void UpcomingMovies()
        {
            try
            {
                List<Movy> movieDto = new List<Movy>();
                HttpWebRequest apiRequest;
                string apiResponse;
                var jsonString = string.Empty;

                apiRequest = WebRequest.Create(_apiBaseURI+"/movie/upcoming?api_key=" + apiKey + "&language=en-US&page=1") as HttpWebRequest;

                apiResponse = ResponseString(apiRequest);

                ResultDTO dto = BindData<ResultDTO>(apiResponse);

                movieDto = dto.results;

                dto.results = dto.results.Select(m =>
                {
                    m.searchText = "Upcoming";
                    return m;
                }).ToList();

                InsertData(movieDto);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static void TopRatedMovies()
        {
            try
            {
                List<Movy> movieDto = new List<Movy>();
                HttpWebRequest apiRequest;
                string apiResponse;
                var jsonString = string.Empty;

                apiRequest = WebRequest.Create(_apiBaseURI + "/movie/top_rated?api_key=" + apiKey + "&language=en-US&page=1") as HttpWebRequest;

                apiResponse = ResponseString(apiRequest);

                ResultDTO dto = BindData<ResultDTO>(apiResponse);

                movieDto = dto.results;

                dto.results = dto.results.Select(m =>
                {
                    m.searchText = "TopRated";
                    return m;
                }).ToList();

                InsertData(movieDto);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static void Popular()
        {
            try
            {
                List<Movy> movieDto = new List<Movy>();
                HttpWebRequest apiRequest;
                string apiResponse;
                var jsonString = string.Empty;

                apiRequest = WebRequest.Create(_apiBaseURI + "/movie/popular?api_key=" + apiKey + "&language=en-US&page=1") as HttpWebRequest;

                apiResponse = ResponseString(apiRequest);

                ResultDTO dto = BindData<ResultDTO>(apiResponse);

                movieDto = dto.results;

                dto.results = dto.results.Select(m =>
                {
                    m.searchText = "Popular";
                    return m;
                }).ToList();

                InsertData(movieDto);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static string ResponseString(HttpWebRequest apiRequest)
        {
            try
            {
                string apiResponse = "";
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }
                return apiResponse;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static T BindData<T>(string apiResponse)
        {
            try
            {
                T obj = JsonConvert.DeserializeObject<T>(apiResponse);
                return obj;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static void InsertData(List<Movy> movies)
        {
            try
            {
                TMDBEntities context = new TMDBEntities();
                context.Movies.AddRange(movies);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
    
    public class ResultDTO
    {
        public List<Movy> results { get; set; }
    }
}
