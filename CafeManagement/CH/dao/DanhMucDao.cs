using System;
using System.Collections.Generic;
using CafeManagement.CH.dao;
using MySql.Data.MySqlClient;

namespace CafeManagement.CH.dao
{
    public class DanhMucDAO
    {
        // =======================================================
        // 1. LẤY TẤT CẢ DANH MỤC
        // =======================================================
        public List<string[]> GetAll()
        {
            List<string[]> list = new List<string[]>();

            string sql = "SELECT * FROM DanhMuc";

            try
            {
                using (MySqlConnection con = DBConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                list.Add(new string[]
                                {
                                    rs["MaDanhMuc"].ToString(),
                                    rs["TenDanhMuc"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return list;
        }

        // =======================================================
        // 2. THÊM DANH MỤC
        // =======================================================
        public bool Insert(string ma, string ten)
        {
            string sql = "INSERT INTO DanhMuc (MaDanhMuc, TenDanhMuc) VALUES (@ma, @ten)";

            try
            {
                using (MySqlConnection con = DBConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@ma", ma);
                        cmd.Parameters.AddWithValue("@ten", ten);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // =======================================================
        // 3. SỬA DANH MỤC
        // =======================================================
        public bool Update(string ma, string ten)
        {
            string sql = "UPDATE DanhMuc SET TenDanhMuc=@ten WHERE MaDanhMuc=@ma";

            try
            {
                using (MySqlConnection con = DBConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@ten", ten);
                        cmd.Parameters.AddWithValue("@ma", ma);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // =======================================================
        // 4. XÓA DANH MỤC
        // =======================================================
        public bool Delete(string ma)
        {
            string sql = "DELETE FROM DanhMuc WHERE MaDanhMuc=@ma";

            try
            {
                using (MySqlConnection con = DBConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@ma", ma);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // =======================================================
        // 5. LẤY TẤT CẢ TÊN DANH MỤC
        // =======================================================
        public List<string> GetAllTenDanhMuc()
        {
            List<string> list = new List<string>();

            string sql = "SELECT TenDanhMuc FROM DanhMuc";

            try
            {
                using (MySqlConnection con = DBConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                list.Add(rs["TenDanhMuc"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return list;
        }

        // =======================================================
        // 6. GET TÊN DANH MỤC
        // =======================================================
        public List<string> GetTenDanhMuc()
        {
            List<string> list = new List<string>();

            string sql = "SELECT TenDanhMuc FROM DanhMuc";

            try
            {
                using (MySqlConnection conn = DBConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                list.Add(rs["TenDanhMuc"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return list;
        }

        // =======================================================
        // 7. KIỂM TRA TÊN DANH MỤC TỒN TẠI
        // =======================================================
        public bool IsExistsTenDanhMuc(string ten)
        {
            string sql = "SELECT COUNT(*) FROM DanhMuc WHERE TenDanhMuc=@ten";

            try
            {
                using (MySqlConnection con = DBConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@ten", ten);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        return count > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }
    }
}