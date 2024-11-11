using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace Bahrin.Harbour.Helper
{
    public class Helper : IHelper
    {
        private readonly IConfiguration configuration;
        private readonly IMemoryCache cache;
        private readonly IFileProvider fileProvider;
        private readonly static string _saltKey = "BAHRINGHARBOUR";
        private readonly static int _linkExpirationTime = 10;
        public Helper(IConfiguration iConfig, IMemoryCache _cache)
        {
            configuration = iConfig;
            cache = _cache;
          //  fileProvider = env.ContentRootFileProvider;
        }
        public static bool ValidateEmail(string email)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }
        public static bool ValidatePhoneNumber(string phoneNumber)
        {
            Regex regex = new Regex(@"^((\+1)?[\s-]?)?\(?[2-9]\d\d\)?[\s-]?[2-9]\d\d[\s-]?");
            Match match = regex.Match(phoneNumber);
            if (match.Success)
                return true;
            else
                return false;
        }
        public static string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            int pos = decrypted.IndexOf(_saltKey);
            decrypted = decrypted.Remove(pos);
            return decrypted;
        }
        public static string EnryptString(string strEncrypted)
        {
            strEncrypted = strEncrypted + _saltKey + DateTime.UtcNow.ToString();
            byte[] b = ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
        public static string EnryptToken(string token)
        {
         var  tokenWithTime = token + _saltKey + DateTimeOffset.Now.AddMinutes(_linkExpirationTime).ToUnixTimeSeconds().ToString();
         return  Convert.ToBase64String(Encoding.UTF8.GetBytes((tokenWithTime)));
        }

        public static DecryptTokenResponse DecryptToken(string token)
        {
            DecryptTokenResponse decryptTokenResponse = new DecryptTokenResponse();
            var StrToen = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            string[] splitString = StrToen.Split(_saltKey);


        //    DateTimeOffset dateTimeOffset = new DateTimeOffset(DateTime.Parse(splitString[1]));


            decryptTokenResponse.token = splitString[0];
            decryptTokenResponse.ValidUnixTime = splitString[1];
            return decryptTokenResponse;    
        }

        public static string SplitLongLines(string input, int maxLength)
        {
            string[] lines = input.Split( "\n" );
            StringBuilder result = new StringBuilder();

            foreach (string line in lines)
            {
                StringBuilder currentLine = new StringBuilder();
                string[] words = line.Split(' ');

                foreach (string word in words)
                {
                    if (currentLine.Length + word.Length + 1 > maxLength)
                    {
                        result.AppendLine(currentLine.ToString().Trim());
                        currentLine.Clear();
                    }

                    currentLine.Append(word + " ");
                }

                if (currentLine.Length > 0)
                {
                    result.AppendLine(currentLine.ToString().Trim());
                }
            }

            return result.ToString().Trim();
        }

        public static async Task<bool> FileExistsAsync(string path)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(path);
                    return response.IsSuccessStatusCode; // Returns true if the status code is 200-299
                }
            }
            catch (HttpRequestException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static Int32 GenerateRandomNumber()
        {
            return RandomNumberGenerator.GetInt32(1000, 9999);
        }
        public static int GenerateOTPNumber(int size = 6)
        {
            int minValue = (int)Math.Pow(10, size - 1);
            int maxValue = (int)Math.Pow(10, size) - 1;
            return RandomNumberGenerator.GetInt32(minValue, maxValue);
        }

        public static Int32 ConvertDateTimetoUnixTimeStamp(DateTime dateTime)
        {
            Int32 unixTimestamp = (int)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            return unixTimestamp;
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp);
            return dateTime;
        }

        public static string RandomPassword(int size = 6)
        {
            if (size < 6) size = 6;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            password.Append(RandomChar("abcdefghijklmnopqrstuvwxyz"));

            password.Append(RandomChar("ABCDEFGHIJKLMNOPQRSTUVWXYZ"));

            password.Append(RandomChar("0123456789"));

            password.Append(RandomChar("!@#$%^&*()_+-=[]{}|;:'\",.<>?/"));

            for (int i = password.Length; i < size; i++)
            {
                string allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;:'\",.<>?/";
                password.Append(RandomChar(allChars));
            }

            return ShuffleString(password.ToString());
        }

        public static string FormatClientId(int? clientId )
        {
            if (clientId == null)
            {
                return null;
            }
            string idString = clientId.Value.ToString("D9");
            return "#" + string.Join("-", Enumerable.Range(0, idString.Length / 3)
                                              .Select(i => idString.Substring(i * 3, Math.Min(3, idString.Length - i * 3)))
                                              .ToArray());
        }

        private static char RandomChar(string chars)
        {
            Random random = new Random();
            return chars[random.Next(chars.Length)];
        }

        private static string ShuffleString(string input)
        {
            Random random = new Random();
            return new string(input.OrderBy(_ => random.Next()).ToArray());
        }

        /*   public static string RandomPassword(int size = 0)
           {
               StringBuilder builder = new StringBuilder();
               builder.Append(RandomString(4, true, true)); 
               builder.Append(RandomNumber(1000, 9999));   
               builder.Append(RandomString(2, false, true)); 
               return builder.ToString();
           }

           public static string RandomString(int size, bool lowerCase, bool includeSpecialChars = false)
           {
               StringBuilder builder = new StringBuilder();
               Random random = new Random();
               char ch;
               string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
               string specialChars = "!@#$%^&*()_+-=[]{}|;:'\",.<>?/";

               for (int i = 0; i < size; i++)
               {
                   if (includeSpecialChars && random.Next(0, 2) == 1) 
                   {
                       ch = specialChars[random.Next(0, specialChars.Length)];
                   }
                   else
                   {
                       ch = chars[random.Next(0, chars.Length)];
                   }
                   builder.Append(ch);
               }

               if (lowerCase)
                   return builder.ToString().ToLower();
               return builder.ToString();
           }

           public static int RandomNumber(int min, int max)
           {
               Random random = new Random();
               return random.Next(min, max);
           }*/

        /*public string GenerateAuthenticationToken(string name, string userName, string userId, string oldToken)
        {
            oldToken = string.IsNullOrEmpty(oldToken) ? "" : oldToken;
            ClearCacheData(oldToken);
            name = string.IsNullOrEmpty(name) ? "name" : name;
            userName = string.IsNullOrEmpty(userName) ? "userName" : userName;
            var tokenHandler = new JwtSecurityTokenHandler();
            //   var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("AppSetting:Screat"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, name),
                        new Claim("UserName", userName),
                        new Claim("UserId", userId),
                    // new Claim(ClaimTypes.Role,user.Type)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            //SetCacheData("jwtToken", tokenString);
            return tokenString;
        }*/

        //public string ExpireAuthenticationToken(string name, string userName, string userId)
        //{
        //    name = string.IsNullOrEmpty(name) ? "name" : name;
        //    userName = string.IsNullOrEmpty(userName) ? "userName" : userName;
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("AppSetting:Screat"));
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(ClaimTypes.Name, name),
        //            new Claim("UserName", userName),
        //            new Claim("UserId", userId),
        //            // new Claim(ClaimTypes.Role,user.Type)
        //        }),
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.ValidateToken(oldToken);
        //    var tokenString = tokenHandler.WriteToken(token);
        //    return tokenString;
        //}

        /*  public string GetAppBaseUrl()
          {
             return configuration.GetValue<string>("AppSetting:AppBaseUrl");
          }*/
        public void SaveJsonFileContent<t>(t data, string fileName)
        {
            try
            {
                string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "AppSettings");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, fileName + ".json");
                string json = JsonSerializer.Serialize(data);
                File.WriteAllText(path, json);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public t GetJsonFileContent<t>(string fileName)
        {
            try
            {
                string fileContent;
                string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "AppSettings", fileName + ".json");
                if (filePath.Contains("PuthagamUnitTest"))
                {
                    filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "AppSettings", fileName + ".json");
                }
                //filePath = fileProvider.GetFileInfo(fileName).PhysicalPath; //Chech need to implement it is not working for now because file location is c
                // Try to obtain the file contents from the cache.
                if (cache.TryGetValue(filePath, out fileContent) && false)
                {
                    t item = JsonSerializer.Deserialize<t>(fileContent);
                    return item;
                }
                else
                {
                    // The cache doesn't have the entry, so obtain the file 
                    // contents from the file itself.
                    fileContent = File.ReadAllText(filePath);
                    if (fileContent != null)
                    {
                        // Obtain a change token from the file provider whose
                        // callback is triggered when the file is modified.
                        IChangeToken changeToken = fileProvider.Watch(fileName);

                        // Configure the cache entry options for a five minute
                        // sliding expiration and use the change token to
                        // expire the file in the cache if the file is
                        // modified.
                        MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                            .AddExpirationToken(changeToken);

                        // Put the file contents into the cache.
                        cache.Set(filePath, fileContent, cacheEntryOptions);

                        t item = JsonSerializer.Deserialize<t>(fileContent);
                        return item;
                    }
                    return JsonSerializer.Deserialize<t>(fileContent);
                }
            }
            catch (FileNotFoundException)
            {
                t defaultVal = default(t);
                return defaultVal;
            }
            catch (DirectoryNotFoundException)
            {
                t defaultVal = default(t);
                return defaultVal;
            }
            catch (Exception)
            {
                t defaultVal = default(t);
                return defaultVal;
            }
        }
      
      /*  public string GetPagesFromWeb(string v)
        {
            string response = string.Empty;
            try
            {
                string filePath = Path.Combine(configuration.GetValue<string>("AppSetting:WebAppBaseUrl"), "Pages", v).Replace("\\", "//");
                var webRequest = WebRequest.Create(filePath);

                using (var fileContent = webRequest.GetResponse())
                using (var content = fileContent.GetResponseStream())
                using (var reader = new StreamReader(content))
                {
                    response = reader.ReadToEnd();
                }
                v = v.Replace("Others\\", "");
                response = response.Replace("<html><head><title>" + v + "</title></head><body>", "").Replace("</html></body>", "");
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }*/
        public t SetCacheData<t>(t data, string fileName)
        {
            try
            {
                if (cache.TryGetValue(fileName, out t cacheData))
                {
                    t item = cacheData;
                    return item;
                }
                else
                {
                    IChangeToken changeToken = fileProvider.Watch(fileName);
                    MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromDays(5)).AddExpirationToken(changeToken);
                    // Put the file contents into the cache.
                    cache.Set(fileName, data, cacheEntryOptions);

                    t item = data;
                    return item;
                }
            }
            catch (FileNotFoundException)
            {
                t defaultVal = default(t);
                return defaultVal;
            }
            catch (DirectoryNotFoundException)
            {
                t defaultVal = default(t);
                return defaultVal;
            }
            catch (Exception)
            {
                t defaultVal = default(t);
                return defaultVal;
            }
        }
        public t GetCacheData<t>(string fileName)
        {
            try
            {
                var dddd = cache.Get(fileName);
                if (cache.TryGetValue(fileName, out t cacheData))
                {
                    t item = cacheData;
                    return item;
                }
                t defaultVal = default(t);
                return defaultVal;
            }
            catch (FileNotFoundException)
            {
                t defaultVal = default(t);
                return defaultVal;
            }
            catch (DirectoryNotFoundException)
            {
                t defaultVal = default(t);
                return defaultVal;
            }
            catch (Exception)
            {
                t defaultVal = default(t);
                return defaultVal;
            }
        }
      /*  public void ClearCacheData(string fileName)
        {
            try
            {
                cache.Remove(fileName);
                if(ProjectSessionModel.admin != null)
                {
                    APIClearCacheData(fileName);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }*/
        //public bool ItemCeched(string itemId)
        //{
        //    bool isCeched = false;
        //    try
        //    {
        //        if (cache.TryGetValue(itemId, out var fileContent))
        //        {
        //            isCeched = true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return isCeched;
        //}
        // Generate a random password of a given length (optional)
       

      /*  void APIClearCacheData(string key)
        {
            try
            {
                string baseUrl = configuration.GetValue<string>("AppSetting:APIBaseURL");
                List<string> APIS = configuration.GetValue<string>("AppSetting:APIRoute").Split(",").ToList();
                foreach (var item in APIS)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(baseUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string apiEndPoint = item + "/ClearCacheData/" + key;
                        // New code:
                        var data = client.GetAsync(apiEndPoint).Result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }*/
    }


    public class DecryptTokenResponse
    {
        public string token { get; set; }
        public string ValidUnixTime { get; set; }
    }
}
