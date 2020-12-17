using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Configuration;
using System.Threading;
using MyConfigurationManager;
using ServiceLayer;
using Models;

namespace DataBaseService
{
    public partial class DataBaseService : ServiceBase
    {
        TableWorker operation;
        public DataBaseService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            operation = new TableWorker();
            Thread serviceThread = new Thread(new ThreadStart(operation.Start));
            serviceThread.Start();
        }

        protected override void OnStop()
        {
            Thread.Sleep(1000);
        }
        public class TableWorker
        {
            private Control settings;
            private readonly ProductsServiceDBH ordersServiseWorker;
            readonly string connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            readonly string storedProcedure = ConfigurationManager.AppSettings.Get("storedprocedure");
            public TableWorker()
            {
                CheckConfig();
                ordersServiseWorker = new ProductsServiceDBH(connectionstring, storedProcedure);
            }
            private void CheckConfig()
            {
                if (Path.GetExtension(ConfigurationManager.AppSettings.Get("path")) == ".xml")
                {
                    ConfOptions provider;
                    XMLParser parser = new XMLParser(ConfigurationManager.AppSettings.Get("path"));
                    try
                    {
                        provider = new ConfOptions(parser);
                        settings = provider.GetOptions<Control>();
                    }
                    catch (Exception e)
                    {
                        settings = new Control();
                        using (StreamWriter err = new StreamWriter(new FileStream(Path.Combine(settings.SourceDirectory, "errLog.txt"), FileMode.OpenOrCreate)))
                        {
                            err.WriteLine($"{DateTime.Now.ToString("dd MM yyyy HH mm ss")} - произошла ошибка {e.Message}");
                            err.WriteLine(e.StackTrace);
                            err.Flush();
                        }
                    }

                }
                else if (Path.GetExtension(ConfigurationManager.AppSettings.Get("path")) == ".json")
                {
                    ConfOptions provider;
                    JSONParser parser = new JSONParser(ConfigurationManager.AppSettings.Get("path"));
                    try
                    {
                        provider = new ConfOptions(parser);
                        settings = provider.GetOptions<Control>();
                    }
                    catch (Exception e)
                    {
                        settings = new Control();
                        using (StreamWriter err = new StreamWriter(new FileStream(Path.Combine(settings.SourceDirectory, "errLog.txt"), FileMode.OpenOrCreate)))
                        {
                            err.WriteLine($"{DateTime.Now.ToString("dd MM yyyy HH mm ss")} - произошла ошибка {e.Message}");
                            err.WriteLine(e.StackTrace);
                            err.Flush();
                        }
                    }

                }
                else throw new Exception("Error of extension of file");
            }
            public void Start()
            {
                var Products = ordersServiseWorker.dataAccess.Products.GetAllObj();
                List<ProductsToClient> products = ordersServiseWorker.ModelChangeToClient(Products);
                Creator<ProductsToClient> xml_xsdCreate = new Creator<ProductsToClient>(products);
                FileTransferService transfer = new FileTransferService(settings.SourceDirectory);
                transfer.FileTransfer(xml_xsdCreate.CreateXML(), xml_xsdCreate.CreateXSD());
            }
        }
    }
}
