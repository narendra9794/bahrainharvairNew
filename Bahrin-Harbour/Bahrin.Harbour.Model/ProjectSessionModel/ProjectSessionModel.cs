using Bahrin.Harbour.Model.AccountModel;
using PuthaganModel.Admin;
using PuthaganModel.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bahrin.Harbour.Model.ProjectSession
{
    public class ProjectSessionModel
    {
        public ProjectSessionModel()
        {
            admin = new AdminModel();
        }
        public static AdminModel? admin { get; set; }
        public static DateFormateSetttingModel? dateFormat { get; set; }
        public static Int32 localTimeZoneOffset { get; set; }
    }
}
