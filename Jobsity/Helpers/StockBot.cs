using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Jobsity.Models;

namespace Jobsity.Helpers
{
    public class StockBot
    {
        string StockCode;
        private string Endpoint => "https://stooq.com/q/l/?s=" + StockCode + "&f=sd2t2ohlcv&h&e=csv​";

        public StockBot(string sc)
        {
            StockCode = sc;
        }

        BotResponse ParseData(Stream rawResponse)
        {
            using (TextFieldParser parser = new TextFieldParser(rawResponse))
            {
                //this vars are control... accoding to the response structure
                const int closedColIndex = 6;
                bool isHeaders = true; //this flag helps us skip the first row

                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData) //read until end
                {
                    //Process row
                    string[] fields = parser.ReadFields();

                    if (isHeaders) //skip header
                    {
                        isHeaders = false; //change flag
                        continue;
                    }

                    var value = fields[closedColIndex];
                    if (value == "N/D")
                    {
                        return new BotResponse(false, "Stock Code Incorrect");
                    }

                    return new BotResponse(true, value); //always return 2nd row
                }

                return new BotResponse(false, "Stock code not found");
            }
        }

        public BotResponse GetStock()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //call api using HttpClient
                    var responseTask = client.GetAsync(Endpoint);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //read result as stream
                        var readTask = result.Content.ReadAsStreamAsync();
                        readTask.Wait();

                        var stockValue = ParseData(readTask.Result);

                        // in case stock was incorrect
                        if (!stockValue.Success)
                        {
                            return stockValue;
                        }

                        //return is string (with format)
                        return new BotResponse(true, StockCode.ToUpper() + " quote is $" + stockValue.Content + " per share.");
                    }
                    return new BotResponse(false, "Stock search did not work. Status: " + result.StatusCode);
                }
            }
            catch(Exception e)
            {
                return new BotResponse(false, e.Message);
            }
        }

        
    }
}