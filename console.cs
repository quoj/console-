using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoListApp
{
    // Định nghĩa delegate cho sự kiện thay đổi trạng thái của công việc
    public delegate void TaskStatusChangedEventHandler(object sender, TaskStatusChangedEventArgs e);

    // Định nghĩa lớp chứa thông tin của sự kiện thay đổi trạng thái của công việc
    public class TaskStatusChangedEventArgs : EventArgs
    {
        public string TaskName { get; set; }
        public bool IsCompleted { get; set; }
    }

    // Lớp đại diện cho một công việc
    public class CongViec
    {
        public string Ten { get; set; }
        private bool daHoanThanh;
        public bool DaHoanThanh
        {
            get => daHoanThanh;
            set
            {
                if (value != daHoanThanh)
                {
                    daHoanThanh = value;
                    OnTaskStatusChanged(new TaskStatusChangedEventArgs { TaskName = Ten, IsCompleted = daHoanThanh });
                }
            }
        }

        // Sự kiện thay đổi trạng thái của công việc
        public event TaskStatusChangedEventHandler TaskStatusChanged;

        // Phương thức báo cáo sự kiện thay đổi trạng thái của công việc
        protected virtual void OnTaskStatusChanged(TaskStatusChangedEventArgs e)
        {
            TaskStatusChanged?.Invoke(this, e);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Tạo danh sách công việc mẫu
            List<CongViec> danhSachCongViec = TaoDanhSachCongViecMau();

            // Hiển thị danh sách công việc ban đầu
            Console.WriteLine("Danh sach cong viec ban đau:");
            HienThiDanhSachCongViec(danhSachCongViec);

            while (true)
            {
                Console.WriteLine("------ Quan Ly Cong Viec ------");
                Console.WriteLine("1. Them cong viec moi");
                Console.WriteLine("2. Hien thi danh sach cong viec");
                Console.WriteLine("3. Danh dau cong viec la da hoan thanh");
                Console.WriteLine("4. Danh dau cong viec la chua hoan thanh");
                Console.WriteLine("5. Thoat");

                // Nhập lựa chọn từ người dùng
                Console.Write("Nhap lua chon cua ban: ");
                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        // Thêm công việc mới
                        Console.Write("Nhap ten cong viec moi: ");
                        string tenCongViec = Console.ReadLine();
                        CongViec cvMoi = new CongViec { Ten = tenCongViec, DaHoanThanh = false };
                        cvMoi.TaskStatusChanged += CongViec_TaskStatusChanged; // Đăng ký sự kiện
                        danhSachCongViec.Add(cvMoi);
                        Console.WriteLine("Cong viec da duoc them thanh cong!");
                        break;
                    case "2":
                        // Hiển thị danh sách công việc
                        Console.WriteLine("Chon lua hien thi danh sach cong viec:");
                        Console.WriteLine("1. Hien thi danh sach cong viec da hoan thanh");
                        Console.WriteLine("2. Hien thi danh sach cong viec chua hoan thanh");
                        Console.Write("Nhap lua chon cua ban: ");
                        string luaChonHienThi = Console.ReadLine();

                        switch (luaChonHienThi)
                        {
                            case "1":
                                HienThiDanhSachCongViecHoanThanh(danhSachCongViec);
                                break;
                            case "2":
                                HienThiDanhSachCongViecChuaHoanThanh(danhSachCongViec);
                                break;
                            default:
                                Console.WriteLine("Lua chon khong hop le.");
                                break;
                        }
                        break;
                    case "3":
                        // Đánh dấu công việc là đã hoàn thành
                        Console.Write("Nhap ten cong viec da hoan thanh: ");
                        string tenDaHoanThanh = Console.ReadLine();
                        CongViec cvHoanThanh = danhSachCongViec.Find(cv => cv.Ten.Equals(tenDaHoanThanh, StringComparison.OrdinalIgnoreCase));
                        if (cvHoanThanh != null)
                        {
                            cvHoanThanh.DaHoanThanh = true;
                            Console.WriteLine("Cong viec da duoc danh dau la da hoan thanh!");
                        }
                        else
                        {
                            Console.WriteLine("Khong tim thay cong viec can danh dau la da hoan thanh!");
                        }
                        break;
                    case "4":
                        // Đánh dấu công việc là chưa hoàn thành
                        Console.Write("Nhap ten cong viec chua hoan thanh: ");
                        string tenChuaHoanThanh = Console.ReadLine();
                        CongViec cvChuaHoanThanh = danhSachCongViec.Find(cv => cv.Ten.Equals(tenChuaHoanThanh, StringComparison.OrdinalIgnoreCase));
                        if (cvChuaHoanThanh != null)
                        {
                            cvChuaHoanThanh.DaHoanThanh = false;
                            Console.WriteLine("Cong viec da duoc danh dau la chua hoan thanh!");
                        }
                        else
                        {
                            Console.WriteLine("Khong tim thay cong viec can danh dau la chua hoan thanh!");
                        }
                        break;
                    case "5":
                        // Thoát khỏi chương trình
                        Console.WriteLine("Ung dung ket thuc.");
                        return;
                    default:
                        Console.WriteLine("Lua chon khong hop le. Vui long thu lai!");
                        break;
                }

                Console.WriteLine(); // Thêm dòng trống để đẹp hơn
            }
        }

        // Xử lý sự kiện thay đổi trạng thái của công việc
        static void CongViec_TaskStatusChanged(object sender, TaskStatusChangedEventArgs e)
        {
            Console.WriteLine($"Cong viec '{e.TaskName}' da duoc {(e.IsCompleted ? "hoan thanh" : "danh dau la chua hoan thanh")}.");
        }

        // Phương thức hiển thị danh sách công việc đã hoàn thành
        static void HienThiDanhSachCongViecHoanThanh(List<CongViec> danhSachCongViec)
        {
            var congViecHoanThanh = danhSachCongViec.Where(cv => cv.DaHoanThanh).ToList();

            if (congViecHoanThanh.Count == 0)
            {
                Console.WriteLine("Khong co cong viec nao da hoan thanh.");
            }
            else
            {
                Console.WriteLine("Danh sach cong viec da hoan thanh:");
                foreach (var cv in congViecHoanThanh)
                {
                    Console.WriteLine($"{cv.Ten}");
                }
            }
        }

        // Phương thức hiển thị danh sách công việc chưa hoàn thành
        static void HienThiDanhSachCongViecChuaHoanThanh(List<CongViec> danhSachCongViec)
        {
            var congViecChuaHoanThanh = danhSachCongViec.Where(cv => !cv.DaHoanThanh).ToList();

            if (congViecChuaHoanThanh.Count == 0)
            {
                Console.WriteLine("Khong co cong viec nao chua hoan thanh.");
            }
            else
            {
                Console.WriteLine("Danh sach cong viec chua hoan thanh:");
                foreach (var cv in congViecChuaHoanThanh)
                {
                    Console.WriteLine($"{cv.Ten}");
                }
            }
        }

        // Phương thức hiển thị danh sách công việc
        static void HienThiDanhSachCongViec(List<CongViec> danhSachCongViec)
        {
            if (danhSachCongViec.Count == 0)
            {
                Console.WriteLine("Danh sach cong viec trong.");
                return;
            }

            foreach (var cv in danhSachCongViec)
            {
                Console.WriteLine($"{cv.Ten} - {(cv.DaHoanThanh ? "Da hoan thanh" : "Chua hoan thanh")}");
            }
        }

        // Tạo danh sách công việc mẫu
        static List<CongViec> TaoDanhSachCongViecMau()
        {
            List<CongViec> danhSachCongViecMau = new List<CongViec>
            {
                new CongViec { Ten = "Lam bai tap lon" },
                new CongViec { Ten = "Đoc sach mpi" },
                new CongViec { Ten = "Chuan bi cho buoi hop" },
                new CongViec { Ten = "Đi mua thuc pham" },
                new CongViec { Ten = "Lam bai kiem tra cuoi ky" }
            };

            return danhSachCongViecMau;
        }

    }
}
