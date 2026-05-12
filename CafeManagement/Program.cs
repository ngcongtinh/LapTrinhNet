using System;
using System.Windows.Forms;
using CafeManagement.CH.dao;
using CH.Controller;
using CH.View;

namespace CH.Main
{
    internal static class Program
    {
        /// <summary>
        /// Điểm bắt đầu chương trình
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // =========================
                // 1. KHỞI TẠO DATABASE
                // =========================
                DBConnection.InitializeDatabase();

                // =========================
                // 2. CẤU HÌNH GIAO DIỆN
                // =========================
                Application.EnableVisualStyles();

                Application.SetCompatibleTextRenderingDefault(false);

                // =========================
                // 3. KHỞI TẠO VIEW
                // =========================
                LoginView loginView = new LoginView();

                // =========================
                // 4. KHỞI TẠO CONTROLLER
                // =========================
                LoginController loginController =
                    new LoginController(loginView);

                // =========================
                // 5. CHẠY CHƯƠNG TRÌNH
                // =========================
                Application.Run(loginView);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khởi động hệ thống:\n" + ex.Message,
                    "System Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}