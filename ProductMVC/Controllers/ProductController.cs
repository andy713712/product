using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using ProductMVC.Models;
using System.Data;
using Serilog;

namespace ProductMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AdventureWorksLT2022Context _dbLT2022;
        public IConfiguration _config;

        public ProductController(ILogger<HomeController> logger, AdventureWorksLT2022Context dbLT2022)
        {
            _logger = logger;
            _dbLT2022 = dbLT2022;
            _config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();

            string logPath = _config["FileTransferSettings:LogPath"] ?? "D:\\ProductMVC\\log";

            // 設定 Serilog 日誌配置，將日誌寫入到 Console 和檔案
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()  // 設置日誌級別
            .WriteTo.File($"{logPath}/log.txt", rollingInterval: RollingInterval.Day)  // 輸出到文件
            .CreateLogger();
        }

        public IActionResult Product()
        {
            return View();
        }

        public IActionResult QueryProductList()
        {
            try
            {
                var products = _dbLT2022.Product.ToList();

                Log.Information("QueryProductList OK");
                return Json(products);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error");
                return Json("QueryProductList Fail");
            }
        }

        public IActionResult GetProduct(int product_pid)
        {
            try
            {
                var product = _dbLT2022.Product.Where(s => s.ProductID == product_pid).FirstOrDefault();

                Log.Information("GetProduct OK");
                return Json(product);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error");
                return Json("GetProduct Fail");
            }
        }

        public IActionResult UpdateProduct(int pid, string name, string number, string color, string standard_cost, string list_price, string size, string weight, string sell_start_date, string sell_end_date, string discontinued_date, string thumbnail_photo, string thumbnail_photo_file_name)
        {
            try
            {
                var product = _dbLT2022.Product.Where(s => s.ProductID == pid).FirstOrDefault();

                if (product != null)
                {
                    product.Name = name;
                    product.ProductNumber = number.ToString();
                    product.Color = color;
                    product.StandardCost = standard_cost == null ? 0 : Convert.ToDecimal(standard_cost);
                    product.ListPrice = list_price == null ? 0 : Convert.ToDecimal(list_price);
                    product.Size = size;
                    product.Weight = weight == null ? null : Convert.ToDecimal(weight);
                    product.SellStartDate = Convert.ToDateTime(sell_start_date);
                    product.SellEndDate = sell_end_date == null ? null : Convert.ToDateTime(sell_end_date);
                    product.DiscontinuedDate = discontinued_date == null ? null : Convert.ToDateTime(discontinued_date);
                    product.ThumbNailPhoto = thumbnail_photo == null ? null : Convert.FromBase64String(thumbnail_photo);
                    product.ThumbnailPhotoFileName = thumbnail_photo_file_name;

                    _dbLT2022.Update(product);
                    _dbLT2022.SaveChanges();
                }

                Log.Information("UpdateProduct OK");
                return Ok(); // Return a success response
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error");
                return Json("UpdateProduct Fail");
            }
        }

    }
}
