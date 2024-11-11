using System;
using System.Collections.Generic;
using System.Text;

namespace Bahrin.Harbour.Helper
{
    public interface IHelper
    {
   //     string GenerateAuthenticationToken(string name, string userName, string id, string oldToken);
     //   string GetAppBaseUrl();
        T GetJsonFileContent<T>(string v);
        void SaveJsonFileContent<t>(t companySettingModel, string v);
     ///   string GetPagesFromWeb(string v);
        t SetCacheData<t>(t data, string fileName);
        t GetCacheData<t>(string fileName);
       // void ClearCacheData(string fileName);
    }
}
