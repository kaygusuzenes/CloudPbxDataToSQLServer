using JsonToText.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Threading;
using System.Web.Script.Serialization;

namespace JsonToText
{
    class Program
    {

        static void Main(string[] args)
        {
            bool statu;
            string date1, date2;
            DateTime addDay;
            //change this connection
            SqlConnection conn = new SqlConnection("Data Source=[adress];Initial Catalog=[database];Integrated Security=False;User ID=sa;Password=[saPass];Connect Timeout=15;Encrypt=False;Packet Size=4096");
            DateTime today = DateTime.Now;
            addDay = today.AddDays(1);
            date1 = today.ToString("ddMMyyyy");
            date2 = addDay.ToString("ddMMyyyy");
            try
            {
                Console.WriteLine("Reading data...");
                //change this url
                string Url = string.Format(@"http://site.com/?username=&password=&action=cdr&date1={0}0000&date2={1}0000", date1, date2);

                var json = new WebClient().DownloadString(Url);
                Thread.Sleep(1000);
                var serializer = new JavaScriptSerializer();
                var resultObj = serializer.Deserialize<List<Json>>(json);
                Console.WriteLine("Database connection...");
                Thread.Sleep(1000);
                Console.WriteLine("Json data converting for sql server...");
                //change the model
                foreach (var item in resultObj[0].data)
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        repeated(item.id);
                        if (statu == true)
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            //change this command and parameters
                            cmd.CommandText = "Insert into [DbName] (callno,calldate,calltype,src,dst,duration,disposition,queue,record) values (@callno,@calldate,@calltype,@src,@dst,@duration,@disposition,@queue,@record)";
                            cmd.Parameters.AddWithValue("@callno", item.id);
                            cmd.Parameters.AddWithValue("@calldate", item.calldate);
                            cmd.Parameters.AddWithValue("@calltype", item.calltype);
                            cmd.Parameters.AddWithValue("@src", item.src);
                            cmd.Parameters.AddWithValue("@dst", item.dst);
                            cmd.Parameters.AddWithValue("@duration", item.duration);
                            cmd.Parameters.AddWithValue("@disposition", item.disposition);
                            cmd.Parameters.AddWithValue("@queue", item.queue);
                            cmd.Parameters.AddWithValue("@record", item.record);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
                Console.WriteLine("Database jobs is finished. Exiting now...");
                Thread.Sleep(2000);

            }

            catch (Exception ex)
            {
                Console.WriteLine("Fail.");
            }
            void repeated(string callno)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select Id,callno from SesKayitVerileri where callno=@p1", conn);
                cmd.Parameters.AddWithValue("@p1", callno);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    statu = false;
                }
                else
                {
                    statu = true;
                }
                conn.Close();
            }

        }

    }
}

