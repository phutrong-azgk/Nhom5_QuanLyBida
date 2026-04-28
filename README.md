=== Hệ Thống Quản Lý Quán Bida (Billiards Management System) ===

- Tổng quan dự án
  + Đây là hệ thống quản lý chuyên biệt cho các câu lạc bộ Bida, giúp tối ưu hóa quy trình từ khâu đặt bàn, tính tiền giờ tự động đến quản lý kho và nhân sự. Dự án chú trọng vào tính chính xác của dữ liệu và hiệu suất xử lý phía Backend.
  + Công nghệ sử dụng
      * Ngôn ngữ: C#
      * Framework: .NET WinForms
      * Cơ sở dữ liệu: SQL Server (tối ưu hóa với Stored Procedures, Functions, Views)
      * Quản lý mã nguồn: Git
    
- Điểm nhấn kỹ thuật (Backend & Database)
  + Dự án này thể hiện kỹ năng xử lý hệ thống chuyên sâu thông qua:
      * Logic tính tiền: Tự động tính tiền dựa trên block thời gian (giờ chơi) kết hợp với các dịch vụ món ăn/nước uống đi kèm thông qua Stored Procedure sp_TinhTienHoaDon.
      * Quản trị cơ sở dữ liệu nâng cao:
          - Thiết lập hệ thống Backup tự động (Full, Differential, Transaction Log) thông qua các Procedure chuyên biệt (sp_BackupFull, sp_BackupDifferential, sp_BackupTransactionLog).
          - Sử dụng Transactions để đảm bảo tính toàn vẹn dữ liệu khi thực hiện các thao tác quan trọng như hoàn tất hóa đơn (sp_HoanTatHoaDon) hoặc nhập hàng (sp_NhapHangVaCapNhatGia).
          - Triển khai Functions & Views để tính điểm khách hàng thân thiết (fn_DiemKhachHang) và thống kê doanh thu theo nhân viên.
      * Phân quyền người dùng (Role-based Access Control): Hệ thống phân quyền chặt chẽ cho các nhóm đối tượng: Admin, Quản Lý, Thu Ngân và Thủ Kho.

- Cấu trúc thư mục
    + Nhom5_CK_QuanLyBida.sln: File solution chính để chạy dự án bằng Visual Studio.
    + Nhom5_QuanLyBida/: Thư mục chứa mã nguồn C# và giao diện WinForms.
    + Database/: Chứa các script SQL khởi tạo.
        QuanLyBilliards_Data.sql: Script khởi tạo toàn bộ cấu trúc bảng, procedures, functions và dữ liệu mẫu để chạy thử ngay lập tức.

- Hướng dẫn cài đặt
    + Database: Mở SQL Server Management Studio (SSMS), chạy file QuanLyBilliards_Data.sql để tạo cơ sở dữ liệu QuanLyBida kèm dữ liệu mẫu.
    + Mở Project: Khởi động Visual Studio, mở file .sln.
    + Cấu hình: Kiểm tra chuỗi kết nối (Connection String) trong code để đảm bảo trỏ đúng về SQL Server cục bộ của bạn.
    + Chạy: Nhấn F5 để build và chạy ứng dụng.
