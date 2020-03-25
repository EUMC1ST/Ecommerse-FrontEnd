using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerseClient
{
    public class Session
    {
        HttpContext ctx;

        public bool SaveSession(string key, string content)
        {
            ctx.Session.SetString(key, JsonConvert.SerializeObject(content));
            return true;
            //if (context.Session.GetString(key) != null)
            //{
            //    string data = (string)JsonConvert.DeserializeObject(context.Session.GetString(key));
            //    return data;
            //}
            //else
            //{
            //    return "No hay datos";
            //}
        }
        public string GetSession(string key)
        {
            if (ctx.Session.GetString(key) != null)
            {
                string data = (string)JsonConvert.DeserializeObject(ctx.Session.GetString(key));
                return data;
            }
            else
            {
                return "No hay datos";
            }
        }
    }
}
