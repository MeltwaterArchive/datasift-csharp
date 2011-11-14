using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Datasift.Api
{
    public class DatasiftUsage
    {
        private JObject json;
        public DatasiftUsage(string json)
        {
            this.json = JObject.Parse(json);
        }
        public long Processed { get { return json["processed"] == null ? -1 : Convert.ToInt64(json["processed"].ToString()); } set { } }
        public long Delivered { get { return json["delivered"] == null ? -1 : Convert.ToInt64(json["delivered"].ToString()); } set { } }
        public List<StreamMeta> StreamMetas { get { return json["StreamMetas"] == null ? null : getStreamMetas(json["StreamMetas"].ToString()); } set { } }
       /// <summary>
       /// Get type specfic usage
       /// </summary>
       /// <param name="type">e.g buzz,twitter</param>
       /// <returns></returns>
        public StreamMeta GetType(string type){
            if(json["types"] == null){
                return new StreamMeta("");
            }else{
               return getTheType(type);
            }
        }

        private StreamMeta getTheType(string type)
        {
 	        JObject types=(JObject)json["types"];
            return new StreamMeta(types[type].ToString());
        }

        private List<StreamMeta> getStreamMetas(string p)
        {
           List<StreamMeta> rec= new List<StreamMeta>();
            JToken StreamMetas = json["StreamMetas"];
            if (StreamMetas == null)
            {
                return null;
            }
            else
            {
                foreach (JToken item in StreamMetas)
                {
                    rec.Add(new StreamMeta(item.ToString()));
                }
                return rec;
            }
        }
    }
    public struct StreamMeta { 
        private JObject json;
        public StreamMeta(string json)
        {
            this.json = JObject.Parse(json);
        }
        public string Hash{get{return json["hash"].ToString();}set{}}
        public long Processed { get { return json["processed"] == null ? -1 : Convert.ToInt64(json["processed"].ToString()); } set { } }
        public long Delivered { get { return json["delivered"] == null ? -1 : Convert.ToInt64(json["delivered"].ToString()); } set { } }
        }
    }