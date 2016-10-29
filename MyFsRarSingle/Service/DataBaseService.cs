using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Aida.Model;
using System.Data.SQLite;
using System.Data;
using System.Globalization;

namespace Aida.Service
{
    public class DataBaseService
    {
        private string dbPath 
        { 
            get { return SettingsService.GetAppPath() +"\\database.db"; } 
            set {}
        }

        private string[] tableList = {
            "CREATE TABLE IF NOT EXISTS " + LOSTPRICE_TABLE.table_name + " (" + 
                LOSTPRICE_TABLE.good_guid + " VARCHAR(36), " + 
                LOSTPRICE_TABLE.good_name + " VARCHAR(100))"
            , 
            "CREATE TABLE IF NOT EXISTS " + APPLICATIONS_TABLE.table_name + " ("+
                APPLICATIONS_TABLE.id + " INTEGER PRIMARY KEY AUTOINCREMENT, " + 
                APPLICATIONS_TABLE.contragent + " VARCHAR(30), " +
                APPLICATIONS_TABLE.doc_guid + " VARCHAR(36), " + 
                APPLICATIONS_TABLE.doc_number + " VARCHAR(15), " +
                APPLICATIONS_TABLE.doc_date + " VARCHAR(8), " +
                APPLICATIONS_TABLE.uploaded + " BOOLEAN)"
            ,            
            " CREATE TABLE IF NOT EXISTS " + SUPPLY_DETAIL_TABLE.table_name + " ( " + 
                SUPPLY_DETAIL_TABLE.id + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
                SUPPLY_DETAIL_TABLE.app_doc_id + " INTEGER REFERENCES " + APPLICATIONS_TABLE.table_name + " (" + APPLICATIONS_TABLE.id + ") ON DELETE CASCADE, " +
                SUPPLY_DETAIL_TABLE.good_guid + " VARCHAR(36), " + 
                SUPPLY_DETAIL_TABLE.good_name + " VARCHAR(50)," + 
                SUPPLY_DETAIL_TABLE.barcode + " VARCHAR(150), " + 
                SUPPLY_DETAIL_TABLE.good_count_doc + " REAL, " + 
                SUPPLY_DETAIL_TABLE.good_count_fact + " REAL, " + 
                SUPPLY_DETAIL_TABLE.akcis + " BOOLEAN, " + 
                SUPPLY_DETAIL_TABLE.dopis + " BOOLEAN, " + 
                SUPPLY_DETAIL_TABLE.roworder + " INTEGER, " + 
                " FOREIGN KEY (" + SUPPLY_DETAIL_TABLE.app_doc_id + ") REFERENCES " + APPLICATIONS_TABLE.table_name + " (" + APPLICATIONS_TABLE.id + ") ON DELETE CASCADE);"
            ,            
            " CREATE TABLE IF NOT EXISTS " + APP_ALKO_DETAIL_TABLE.table_name + " (" + APP_ALKO_DETAIL_TABLE.id + " INTEGER PRIMARY KEY AUTOINCREMENT," +
                APP_ALKO_DETAIL_TABLE.supplydetail_id + " INTEGER REFERENCES " + SUPPLY_DETAIL_TABLE.table_name + " (" + SUPPLY_DETAIL_TABLE.id + ") ON DELETE CASCADE, " + 
                APP_ALKO_DETAIL_TABLE.mark + " VARCHAR(68), "+
                APP_ALKO_DETAIL_TABLE.mark_serial + " INTEGER, " + 
                APP_ALKO_DETAIL_TABLE.mark_number + " INTEGER, " +
                APP_ALKO_DETAIL_TABLE.date_rozliv + " VARCHAR(8), " + 
                "FOREIGN KEY (" + APP_ALKO_DETAIL_TABLE.supplydetail_id + ") REFERENCES " + SUPPLY_DETAIL_TABLE.table_name + " (" + SUPPLY_DETAIL_TABLE.id + ") ON DELETE CASCADE);" //+
            };

        public Result InitBase()
        {
            Result result;
           
            result = CheckDataBase();

            if(!result.success)
            {
                return new Result { success = false, msg = result.msg };
            }

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();

                result = CheckTable(connection, tableList);

                if (!result.success)
                {                    
                    return new Result { success = result.success, msg = result.msg };
                }
            }

            return new Result { success = result.success, msg = result.msg };            
        }

        private Result CheckTable(SQLiteConnection connection, string[] tableList)
        {           
            foreach (var sql_cmd in tableList)
            {                
                using (SQLiteCommand cmd = new SQLiteCommand(sql_cmd, connection))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();                       
                    }
                    catch (Exception ex)
                    {                        
                        return new Result { success = false, msg = ex.Message };
                    }                    
                }
            }

            return new Result { success = true, msg = "All tables created successfully" };
        }

        private Result CheckDataBase()
        {
            if (!File.Exists(dbPath))
            {
                try
                {
                    // create db
                    SQLiteConnection.CreateFile(dbPath);                    

                    return new Result { success = true };
                }
                catch (Exception ex)
                {
                    return new Result { success = false, msg = ex.Message };
                }
            }

            return new Result { success = true };
        }

        public Result InsertRecordToDb(InsertSqlRecordModel insertRecord)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();

                var sql = insertRecord.CreateSqlInsertCmd();
                
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return new Result { success = false, msg = ex.Message };
                    }                   
                }
            }

            return new Result { success = true};
        }

        public Result InsertSupplyRowsToDb(int app_doc_id, ApplicationDocumentRow[] appDocRows) 
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();

                foreach (var item in appDocRows)
                {
                    var ins = new SqlFieldModel[] {
                        new SqlFieldModel{ 
                            fieldName = SUPPLY_DETAIL_TABLE.app_doc_id,
                            fieldValue = app_doc_id
                            },
                        new SqlFieldModel{ 
                            fieldName = SUPPLY_DETAIL_TABLE.good_guid,
                            fieldValue = "'"+item.good_guid+"'"
                            },
                        new SqlFieldModel{ 
                            fieldName = SUPPLY_DETAIL_TABLE.good_name,
                            fieldValue = "'"+item.good_name+"'"
                        },
                        new SqlFieldModel{ 
                            fieldName = SUPPLY_DETAIL_TABLE.barcode,
                            fieldValue = "'" + item.barcode + "'"
                        },
                        new SqlFieldModel{ 
                            fieldName = SUPPLY_DETAIL_TABLE.good_count_doc,
                            fieldValue = item.good_count_doc
                        },
                        new SqlFieldModel{ 
                            fieldName = SUPPLY_DETAIL_TABLE.good_count_fact,
                            fieldValue = item.good_count_fact
                        },
                        new SqlFieldModel{ 
                            fieldName = SUPPLY_DETAIL_TABLE.akcis,
                            fieldValue = item.akcis ? 1 : 0
                        },
                        new SqlFieldModel{ 
                            fieldName = SUPPLY_DETAIL_TABLE.dopis,
                            fieldValue = item.dopis ? 1 : 0
                        },
                        new SqlFieldModel{ 
                            fieldName = SUPPLY_DETAIL_TABLE.roworder,
                            fieldValue = 0
                        }
                    };

                    var insertRecord = new InsertSqlRecordModel
                    {
                        tableName = SUPPLY_DETAIL_TABLE.table_name,
                        Fields = ins
                    };

                    var sql = insertRecord.CreateSqlInsertCmd();

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                    {
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            return new Result { success = false, msg = "Ошибка добавления в базу:" + Environment.NewLine +
                                item.good_name + Environment.NewLine + ex.Message
                            };
                        }
                    }
                }
            }             

            return new Result { success = true };
        }

        public Result DeleteAllRecordsFromTable(string tableName)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();

                using (SQLiteCommand cmd = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return new Result { success = false, msg = ex.Message };
                    }
                }

                using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM " + tableName, connection))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return new Result { success = false, msg = ex.Message };
                    }
                }
            }

            return new Result { success = true };
        }

        public Result DeleteRecordFromTable(string tableName, string expression)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();

                using (SQLiteCommand cmd = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return new Result { success = false, msg = ex.Message };
                    }
                }

                using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM " + tableName + " WHERE " + expression, connection))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return new Result { success = false, msg = ex.Message };
                    }
                }
            }

            return new Result { success = true };
        }

        public Result UpdateTableRecord(UpdateSqlRecordModel updateRecord)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();

                var sql = updateRecord.CreateSqlUpdateCmd();

                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return new Result { success = false, msg = "Ошибка обновления данных: " + ex.Message };
                    }
                }
            }

            return new Result { success = true };
        }

        public List<LostPrice> ReadLostPriceTable() 
        {
            var priceList = new List<LostPrice>();

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "SELECT * FROM " + LOSTPRICE_TABLE.table_name;
                    cmd.CommandType = CommandType.Text;
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            priceList.Add(new LostPrice
                                {
                                    GoodGuid = Convert.ToString(r[LOSTPRICE_TABLE.good_guid]),
                                    GoodName = Convert.ToString(r[LOSTPRICE_TABLE.good_name])
                                });
                        }
                    }
                }
            }

            return priceList;
        }

        public List<SupplyDetailRow> ReadSupplyDetailTable(int app_id)
        {
            var supplyDetailTable = new List<SupplyDetailRow>();
            
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "SELECT * FROM " + SUPPLY_DETAIL_TABLE.table_name + " WHERE " + SUPPLY_DETAIL_TABLE.app_doc_id + " = " + app_id;
                    cmd.CommandType = CommandType.Text;
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            supplyDetailTable.Add(new SupplyDetailRow
                            {
                                id = Convert.ToInt32(r[SUPPLY_DETAIL_TABLE.id]),
                                app_doc_id = Convert.ToInt32(r[SUPPLY_DETAIL_TABLE.app_doc_id]),
                                good_guid = Convert.ToString(r[SUPPLY_DETAIL_TABLE.good_guid]),
                                good_name = Convert.ToString(r[SUPPLY_DETAIL_TABLE.good_name]),
                                barcode = Convert.ToString(r[SUPPLY_DETAIL_TABLE.barcode]),
                                good_count_doc = Convert.ToDouble(r[SUPPLY_DETAIL_TABLE.good_count_doc]),
                                good_count_fact = Convert.ToDouble(r[SUPPLY_DETAIL_TABLE.good_count_fact]),
                                akcis = Convert.ToInt16(r[SUPPLY_DETAIL_TABLE.akcis]) == 0 ? false : true,
                                dopis = Convert.ToInt16(r[SUPPLY_DETAIL_TABLE.dopis]) == 0 ? false : true,
                                roworder = Convert.ToInt16(r[SUPPLY_DETAIL_TABLE.roworder])
                            });
                        }
                    }
                }
            }

            return supplyDetailTable;
        }

        public List<ApplicationDB> GetSupplyListTable()
        {
            var supplyListTable = new List<ApplicationDB>();

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "SELECT * FROM " + APPLICATIONS_TABLE.table_name;
                    cmd.CommandType = CommandType.Text;

                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            supplyListTable.Add(new ApplicationDB
                            {
                                id = Convert.ToInt32(r[APPLICATIONS_TABLE.id]),
                                doc_guid = Convert.ToString(r[APPLICATIONS_TABLE.doc_guid]),
                                doc_number = Convert.ToString(r[APPLICATIONS_TABLE.doc_number]),
                                contragent = Convert.ToString(r[APPLICATIONS_TABLE.contragent]),
                                doc_date = Convert.ToString(r[APPLICATIONS_TABLE.doc_date]),
                                uploaded = Convert.ToInt16(r[APPLICATIONS_TABLE.uploaded]) == 0 ? false : true
                            });
                        }
                    }
                }
            }

            return supplyListTable;
        }

        public int GetApplicationTableId(string doc_guid)
        {
            var id = 0;

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "SELECT " + APPLICATIONS_TABLE.id + " FROM " + APPLICATIONS_TABLE.table_name + 
                        " WHERE " + APPLICATIONS_TABLE.doc_guid + " = '" + doc_guid + "'";
                    cmd.CommandType = CommandType.Text;

                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            id = Convert.ToInt32(r[APPLICATIONS_TABLE.id]);
                        }
                    }
                }
            }

            return id;
        }

        public int WriteNewApplicationToDB(ApplicationDoc doc)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();

                // вставым запись в таблицу 
                var sql = "INSERT INTO " + APPLICATIONS_TABLE.table_name + " (" + APPLICATIONS_TABLE.contragent + "," +
                    APPLICATIONS_TABLE.doc_guid + "," + APPLICATIONS_TABLE.doc_number + ","+
                    APPLICATIONS_TABLE.doc_date + "," + APPLICATIONS_TABLE.uploaded + ") VALUES ('" + doc.contragent + "','" +
                        doc.doc_guid + "','" + doc.doc_number + "','" + doc.doc_date + "', 0)"; 

                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        return 0;
                    } 
                }

                // прочитаем ее и получим новый id
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "SELECT " + APPLICATIONS_TABLE.id + " FROM " + APPLICATIONS_TABLE.table_name + 
                        " WHERE " + APPLICATIONS_TABLE.doc_guid + " = '" + doc.doc_guid + "'";
                    cmd.CommandType = CommandType.Text;
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            return Convert.ToInt32(r[APPLICATIONS_TABLE.id]);
                        }
                    }
                }
            }

            return 0; 
        }

        public Result SetUploadStatus(int app_id)
        {
            var updateRecord = new UpdateSqlRecordModel();
            updateRecord.tableName = APPLICATIONS_TABLE.table_name;
            updateRecord.expression = APPLICATIONS_TABLE.id + " = " + app_id;
            updateRecord.Fields = new SqlFieldModel[] { 
                new SqlFieldModel
                {
                    fieldName = APPLICATIONS_TABLE.uploaded, 
                    fieldValue = "1"
                }
            };

            return UpdateTableRecord(updateRecord);
        }

        public List<Bottle> ReadAlkoDetailTable(int app_supply_doc_id)
        {
            var bottleList = new List<Bottle>();

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();

                // прочитаем ее и получим новый id
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "SELECT * FROM " + APP_ALKO_DETAIL_TABLE.table_name +
                        " WHERE " + APP_ALKO_DETAIL_TABLE.supplydetail_id + " = " + app_supply_doc_id;

                    cmd.CommandType = CommandType.Text;
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            bottleList.Add(new Bottle {
                                pdf417 = Convert.ToString(r[APP_ALKO_DETAIL_TABLE.mark]),
                                serial = Convert.ToInt32(r[APP_ALKO_DETAIL_TABLE.mark_serial]),
                                number = Convert.ToInt32(r[APP_ALKO_DETAIL_TABLE.mark_number]),
                                //dateRozliv = Convert.ToDateTime(r[APP_ALKO_DETAIL_TABLE.date_rozliv])
                                dateRozliv = DateTime.ParseExact(Convert.ToString(r[APP_ALKO_DETAIL_TABLE.date_rozliv]), "dd.MM.yy", CultureInfo.InvariantCulture)
                            });
                        }
                    }
                }
            }

            return bottleList;
        }

        public Result WriteBottleToDB(int supply_detail_id, Bottle newBottle)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();

                string sql = "INSERT INTO " + APP_ALKO_DETAIL_TABLE.table_name +
                    " (" + APP_ALKO_DETAIL_TABLE.supplydetail_id + "," +
                    APP_ALKO_DETAIL_TABLE.mark + "," +
                    APP_ALKO_DETAIL_TABLE.mark_serial + "," +
                    APP_ALKO_DETAIL_TABLE.mark_number + "," +
                    APP_ALKO_DETAIL_TABLE.date_rozliv +
                ") VALUES (" +
                    supply_detail_id.ToString() + "," +
                    "'" + newBottle.pdf417 + "'," +
                    newBottle.serial + "," +
                    newBottle.number + "," +
                    "'" + newBottle.dateRozliv.ToString("dd.MM.yy") + "')";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return new Result { success = false, msg = ex.Message };
                    }
                }

            }

            return new Result { success = true };
        }
    }
}
