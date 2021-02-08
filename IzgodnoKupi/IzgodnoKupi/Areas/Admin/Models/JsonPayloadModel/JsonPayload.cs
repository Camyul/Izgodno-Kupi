namespace IzgodnoKupi.Web.Areas.Admin.Models.JsonPayloadModel
{
    public class JsonPayloadModel
    {
        public string Base = "/";
        public int n = 0;
        public string q = "";

        public JsonPayloadModel(string categoryName)
        {
            this.q = categoryName;
        }
    }
}
