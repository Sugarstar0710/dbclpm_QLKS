create database DAQLKS 
go

use DAQLKS
go

-----LOẠI KHÁCH HÀNG----

CREATE TABLE tbl_LoaiKhach(
	MaLoaiKH INT PRIMARY KEY,
	TenLoaiKH NVARCHAR(30)
)
select * from tbl_LoaiKhach
--INSERT--
insert into tbl_LoaiKhach
values(001,N'Ngoại Quốc')
insert into tbl_LoaiKhach
values(002,N'Trong Nước')

-----KHÁCH HÀNG-----

CREATE TABLE tbl_KhachHang(
	MaKH VARCHAR(10) PRIMARY KEY,
	HoTen NVARCHAR(30),
	TaiKhoan NVARCHAR(30),
	MatKhau VARCHAR(30),
	Email NVARCHAR(30),
	SDT VARCHAR(13),
	NgaySinh Date,
	CCCD CHAR(12),
	DiaChi NVARCHAR(50),
	MaLoaiKH INT FOREIGN KEY REFERENCES tbl_LoaiKhach(MaLoaiKH)
)
ALTER TABLE tbl_KhachHang
ADD QuocTich NVARCHAR(MAX)
select * from tbl_KhachHang
select * from tbl_NhanVien
----------------------LOẠI PHÒNG--------------------------

--CREATE

CREATE TABLE tbl_LoaiPhong(
	MaLoaiPhong CHAR(4) PRIMARY KEY,
	TenLoaiPhong NVARCHAR(20),
	DonGia MONEY
)
--ALTER
ALTER TABLE tbl_LoaiPhong
ADD MoTa NVARCHAR(MAX)

ALTER TABLE tbl_LoaiPhong
ADD img NVARCHAR(MAX)
--INSERT--
INSERT INTO tbl_LoaiPhong
VALUES('PDON',N'Phòng Đơn',150000),
('PDOI',N'Phòng Đôi',170000),
('PVIP',N'Phòng Vip',200000)
--UPDATE--
--CẬP NHẬT BẢNG LOẠI PHÒNG
UPDATE tbl_LoaiPhong
SET MoTa=N'Đây là một loại phòng được thiết kế để phục vụ một khách lưu trú duy nhất. Đây là sự lựa chọn lý tưởng cho những du khách đi công tác, người du lịch một mình hoặc những ai cần một nơi nghỉ ngơi yên tĩnh và thoải mái'
WHERE MaLoaiPhong='PDON'

UPDATE tbl_LoaiPhong
SET MoTa=N'Đây là một loại phòng được thiết kế để phục vụ hai khách lưu trú. Đây là sự lựa chọn phổ biến cho các cặp đôi, bạn bè hoặc đồng nghiệp muốn chia sẻ không gian nghỉ ngơi.'
WHERE MaLoaiPhong='PDOI'

UPDATE tbl_LoaiPhong
SET MoTa=N'Đây là loại phòng cao cấp nhất, được thiết kế để cung cấp trải nghiệm lưu trú sang trọng, tiện nghi và độc đáo. Phòng VIP thường dành cho các khách hàng đặc biệt, như doanh nhân, người nổi tiếng, hoặc những người mong muốn sự thoải mái và dịch vụ tốt nhất.'
WHERE MaLoaiPhong='PVIP'
--Load hình 
UPDATE tbl_LoaiPhong
SET img='PDON.jpg'
WHERE MaLoaiPhong='PDON'

UPDATE tbl_LoaiPhong
SET img='PDOI.jpg'
WHERE MaLoaiPhong='PDOI'

UPDATE tbl_LoaiPhong
SET img='PVIP.jpg'
WHERE MaLoaiPhong='PVIP'

SELECT* FROM tbl_LoaiPhong

-------TRẠNG THÁI PHÒNG-----------------

CREATE TABLE tbl_TrangThaiPhong(
	MaTrangThai CHAR(4) PRIMARY KEY,
	TenTrangThai NVARCHAR(20)
)
--INSERT--
INSERT INTO tbl_TrangThaiPhong VALUES
('TT01',N'Sẵn sàng'),
('TT02',N'Đang có khách'),
('TT03',N'Cần dọn dẹp')

-------------PHÒNG-------------------

--CREATE--
CREATE TABLE tbl_Phong(
	MaPhong CHAR(4) PRIMARY KEY,
	SoPhong INT,
	MaLoaiPhong CHAR(4) FOREIGN KEY REFERENCES tbl_LoaiPhong(MaLoaiPhong),
	MaTrangThai CHAR(4) FOREIGN KEY REFERENCES tbl_TrangThaiPhong(MaTrangThai)
)
--ALTER--
ALTER TABLE tbl_Phong
ADD HinhAnh NVARCHAR(MAX)
--INSERT--
INSERT INTO tbl_Phong VALUES
('P100',100,'PDON',NULL,NULL),
('P101',101,'PDON',NULL,NULL),
('P102',102,'PDON',NULL,NULL),
('P103',103,'PDOI',NULL,NULL),
('P200',200,'PDOI',NULL,NULL),
('P201',201,'PDOI',NULL,NULL),
('P202',202,'PVIP',NULL,NULL),
('P203',203,'PVIP',NULL,NULL),
('P204',204,'PVIP',NULL,NULL)
--UPDATE
UPDATE tbl_Phong
SET HinhAnh='PDON_1.jpg'
WHERE MaPhong='P100'

UPDATE tbl_Phong
SET HinhAnh='PDON_2.jpg'
WHERE MaPhong='P101'

UPDATE tbl_Phong
SET HinhAnh='PDON_3.jpg'
WHERE MaPhong='P102'

UPDATE tbl_Phong
SET HinhAnh='PDOI_1.jpg'
WHERE MaPhong='P103'

UPDATE tbl_Phong
SET HinhAnh='PDOI_2.jpg'
WHERE MaPhong='P200'

UPDATE tbl_Phong
SET HinhAnh='PDOI_3.jpg'
WHERE MaPhong='P201'

UPDATE tbl_Phong
SET HinhAnh='PVIP_2.jpg'
WHERE MaPhong='P202'

UPDATE tbl_Phong
SET HinhAnh='PVIP_3.jpg'
WHERE MaPhong='P203'

UPDATE tbl_Phong
SET HinhAnh='PVIP_4.jpg'
WHERE MaPhong='P204'
--------CHỨC VỤ--------------

--CREATE--
CREATE TABLE tbl_ChucVu(
	MaCV CHAR(4) PRIMARY KEY,
	TenChucVu NVARCHAR(30)
)

-----NHÂN VIÊN----------------
select * from tbl_NhanVien
CREATE TABLE tbl_NhanVien(
	MaNV VARCHAR(10) PRIMARY KEY,
	HoTen NVARCHAR(30),
	TaiKhoan NVARCHAR(30),
	MatKhau VARCHAR(30),
	Email VARCHAR(30),
	NgaySinh DATE,
	MaCV CHAR(4) FOREIGN KEY REFERENCES tbl_ChucVu(MaCV)

)


----PHIẾU THUÊ PHÒNG--------
select * from tbl_PhieuThuePhong
--CREATE--
CREATE TABLE tbl_PhieuThuePhong(
	MaPhieuThuePhong VARCHAR(10) PRIMARY KEY,
	NgayBatDauThue DATE,
	MaPhong CHAR(4) FOREIGN KEY REFERENCES tbl_Phong(MaPhong)
)
--ALTER--
ALTER TABLE tbl_PhieuThuePhong
ADD SLKhach INT

ALTER TABLE tbl_PhieuThuePhong
ADD SLKhachNuocNgoai INT

ALTER TABLE tbl_PhieuThuePhong
ADD NgayKetThucThue DATE,TrangThai NVARCHAR(MAX)

ALTER TABLE tbl_PhieuThuePhong
ADD MaKH VARCHAR(10) FOREIGN KEY REFERENCES tbl_KhachHang(MaKH)
ALTER TABLE tbl_PhieuThuePhong
ADD MaNV VARCHAR(10) FOREIGN KEY REFERENCES tbl_NhanVien(MaNV)
--INSERT--
INSERT INTO tbl_PhieuThuePhong VALUES
('PT1','20240701','P100',3,1,'20240710',N'Chưa nhận phòng')
--UPDATE--

-------CHI TIẾT PHIẾU THUÊ-------

--CREATE--
CREATE TABLE tbl_ChiTieuPhieuThue(
	MaChiTiet VARCHAR(10) PRIMARY KEY,
	HoTenKH NVARCHAR(30),
	NgayBatDauThue DATE,
	SoLuongKhach INT,
	SoLuongKhachNuocNgoai INT,
	CCCD CHAR(12),
	DiaChi NVARCHAR(50),
	MaPhieuThue VARCHAR(10) FOREIGN KEY REFERENCES tbl_PhieuThuePhong(MaPhieuThuePhong),
)
--ALTER--
ALTER TABLE tbl_ChiTieuPhieuThue
DROP COLUMN SoLuongKhach 
ALTER TABLE tbl_ChiTieuPhieuThue
DROP COLUMN SoLuongKhachNuocNgoai

--------HÓA ĐƠN---------

CREATE TABLE tbl_HoaDon(
	MaHD VARCHAR(10) PRIMARY KEY,
	NgayThanhToan DATE,
	TongTien MONEY,
	MaKH VARCHAR(10) FOREIGN KEY REFERENCES tbl_KhachHang(MaKH),
	MaPhieuThuePhong VARCHAR(10) FOREIGN KEY REFERENCES tbl_PhieuThuePhong(MaPhieuThuePhong),
	MaNV VARCHAR(10) FOREIGN KEY REFERENCES tbl_NhanVien(MaNV)
)
--ALTER--
ALTER TABLE tbl_HoaDon
ADD TrangThai NVARCHAR(50)

ALTER TABLE tbl_HoaDon
ADD CONSTRAINT DF_TongTien DEFAULT 0 FOR TongTien;

------LOẠI DỊCH VỤ-----------------------------

CREATE TABLE tbl_LoaiDichVu(
	MaLoaiDV CHAR(4) PRIMARY KEY,
	TenLoaiDV NVARCHAR(20)
)

--INSERT--
INSERT INTO tbl_LoaiDichVu VALUES
('DV01',N'Dịch vụ dọn dẹp'),
('DV02',N'Dịch vụ ăn uống'),
('DV03',N'Dịch vụ spa'),
('DV04',N'Dịch vụ đưa rước')
--ALTER--
ALTER TABLE tbl_LoaiDichVu
ADD img NVARCHAR(MAX)
--UPDATE--
UPDATE tbl_LoaiDichVu
SET img='clean-up.png'
WHERE MaLoaiDV='DV01'

UPDATE tbl_LoaiDichVu
SET img='restaurant.png'
WHERE MaLoaiDV='DV02'

UPDATE tbl_LoaiDichVu
SET img='spa.png'
WHERE MaLoaiDV='DV03'

UPDATE tbl_LoaiDichVu
SET img='XeDuaRuoc.png'
WHERE MaLoaiDV='DV04'

---TRẠNG THÁI DỊCH VỤ---------------------------

CREATE TABLE tbl_TrangThaiDichVu(
	MaTrangThaiDV CHAR(4) PRIMARY KEY,
	TenTrangThai NVARCHAR(20)
)
--INSERT--
INSERT INTO tbl_TrangThaiDichVu VALUES
('TT01',N'Chưa xác nhận'),
('TT02',N'Đã xác nhận'),
('TT03',N'Đã hoàn thành'),
('TT04',N'Đã hủy')

----DỊCH VỤ---------

CREATE TABLE tbl_DichVu(
	MaDV CHAR(4) PRIMARY KEY,
	TenDV NVARCHAR(50),
	DonGia MONEY,
	MaLoaiDV CHAR(4) FOREIGN KEY REFERENCES tbl_LoaiDichVu(MaLoaiDV)
)
--INSERT--
INSERT INTO tbl_DichVu VALUES
('DA01',N'Salad cá ngừ',NULL,'DV02'),
('DA02',N'Súp gà bắp non',NULL,'DV02'),
('DA03',N'Bò kho bánh mì',NULL,'DV02'),
('DA04',N'Tiramisu',NULL,'DV02'),
('DA05',N'Bánh Cupcake',NULL,'DV02'),
('DA06',N'Chè Khúc Bạch',NULL,'DV02'),
('DA07',N'Khoai Tây Nướng Ô Liu',NULL,'DV02'),
('DA08',N'Cocottes Nướng',NULL,'DV02')
INSERT INTO tbl_DichVu VALUES
('TU01',N'Capucino',NULL,'DV02'),
('TU02',N'Expresso',NULL,'DV02'),
('TU03',N'Cafe Trứng',NULL,'DV02'),
('TU04',N'Nước Ép Cam',NULL,'DV02'),
('TU05',N'Nước Ép Dưa Hấu',NULL,'DV02'),
('TU06',N'Sinh Tố Dâu',NULL,'DV02'),
('TU07',N'Tiger Nâu',NULL,'DV02'),
('TU08',N'Rượu Vang Đỏ',NULL,'DV02')
INSERT INTO tbl_DichVu VALUES
('DD01',N'Dọn dẹp phòng',NULL,'DV01')
INSERT INTO tbl_DichVu VALUES
('DR01',N'Đưa rước sân bay',NULL,'DV04')
INSERT INTO tbl_DichVu VALUES
('LD01',N'Massage body',NULL,'DV03'),
('LD02',N'Massage trị liệu cổ vai gáy',NULL,'DV03'),
('LD03',N'Massage trị liệu thái dương',NULL,'DV03'),
('LD04',N'Chăm sóc da mặt',NULL,'DV03'),
('LD05',N'Tẩy tế bào chết toàn thân',NULL,'DV03'),
('LD06',N'Tắm trắng',NULL,'DV03'),
('LD07',N'Triệt lông body',NULL,'DV03'),
('LD08',N'Triệt lông vùng nách',NULL,'DV03'),
('LD09',N'Triệt lông tay,chân',NULL,'DV03')
--ALTER--
ALTER TABLE tbl_DichVu
ADD img NVARCHAR(MAX)

ALTER TABLE tbl_DichVu
ALTER COLUMN TenDV NVARCHAR(50)

ALTER TABLE tbl_DichVu
ADD MoTa NVARCHAR(MAX)

--UPDATE--
UPDATE tbl_DichVu
SET MoTa=N'Món salad kết hợp cá ngừ tươi hoặc đóng hộp với các loại rau củ như xà lách, cà chua, dưa chuột, hành tây, và được trộn với sốt chanh hoặc mayonnaise để tăng hương vị.'
WHERE MaDV='DA01'

UPDATE tbl_DichVu
SET MoTa=N'Súp gà bổ dưỡng với thịt gà mềm, bắp non ngọt ngào, thường được nấu cùng với các loại rau củ như cà rốt, hành tây, và nêm nếm gia vị vừa ăn.'
WHERE MaDV='DA02'

UPDATE tbl_DichVu
SET MoTa=N'Món thịt bò kho đậm đà hương vị, nấu với cà rốt, khoai tây và các loại gia vị đặc trưng, thường được ăn kèm với bánh mì giòn.'
WHERE MaDV='DA03'

UPDATE tbl_DichVu
SET MoTa=N'Món tráng miệng nổi tiếng của Ý, làm từ lớp bánh quy nhúng cà phê, phủ kem mascarpone và rắc bột cacao lên trên.'
WHERE MaDV='DA04'

UPDATE tbl_DichVu
SET MoTa=N'Những chiếc bánh nhỏ xinh xắn, mềm mịn và thơm ngon, thường được trang trí với kem bơ, đường icing và các loại hạt, kẹo.'
WHERE MaDV='DA05'

UPDATE tbl_DichVu
SET MoTa=N'Món chè thanh mát với những miếng khúc bạch (thạch làm từ kem sữa) dẻo dai, thường được ăn kèm với trái cây và nước đường phèn.'
WHERE MaDV='DA06'

UPDATE tbl_DichVu
SET MoTa=N'Khoai tây cắt miếng hoặc lát, nướng giòn với dầu ô liu và các loại thảo mộc như rosemary, thêm một chút muối và tiêu.'
WHERE MaDV='DA07'

UPDATE tbl_DichVu
SET MoTa=N'Món ăn nướng trong nồi đất hoặc nồi gang nhỏ, thường là các loại thực phẩm như thịt, rau củ, phô mai được nướng chín tới, giữ nguyên hương vị thơm ngon.'
WHERE MaDV='DA08'

UPDATE tbl_DichVu
SET DonGia=120000
WHERE MaDV='DA01'

UPDATE tbl_DichVu
SET DonGia=40000
WHERE MaDV='DA02'

UPDATE tbl_DichVu
SET DonGia=80000
WHERE MaDV='DA03'

UPDATE tbl_DichVu
SET DonGia=70000
WHERE MaDV='DA04'

UPDATE tbl_DichVu
SET DonGia=30000
WHERE MaDV='DA05'

UPDATE tbl_DichVu
SET DonGia=35000
WHERE MaDV='DA06'

UPDATE tbl_DichVu
SET DonGia=45000
WHERE MaDV='DA07'

UPDATE tbl_DichVu
SET DonGia=30000
WHERE MaDV='DA08'

UPDATE tbl_DichVu
SET DonGia = 0, MoTa = N'Dọn dẹp phòng khách sạn, bao gồm thay ga trải giường, dọn dẹp phòng tắm và làm sạch toàn bộ phòng.'
WHERE MaDV = 'DD01';

UPDATE tbl_DichVu
SET DonGia = 200000.00, MoTa = N'Dịch vụ đưa đón khách tại sân bay bằng xe hơi, giúp khách hàng có chuyến đi thuận tiện và thoải mái.'
WHERE MaDV = 'DR01';

UPDATE tbl_DichVu
SET DonGia = 300000.00, MoTa = N'Dịch vụ massage body toàn thân giúp thư giãn cơ bắp và cải thiện tuần hoàn máu.'
WHERE MaDV = 'LD01';

UPDATE tbl_DichVu
SET DonGia = 350000.00, MoTa = N'Dịch vụ massage trị liệu cổ vai gáy giúp giảm căng thẳng và đau nhức vùng cổ và vai.'
WHERE MaDV = 'LD02';

UPDATE tbl_DichVu
SET DonGia = 320000.00, MoTa = N'Dịch vụ massage trị liệu thái dương giúp giảm đau đầu và căng thẳng.'
WHERE MaDV = 'LD03';

UPDATE tbl_DichVu
SET DonGia = 250000.00, MoTa = N'Dịch vụ chăm sóc da mặt giúp làm sạch sâu và cung cấp dưỡng chất cho da.'
WHERE MaDV = 'LD04';

UPDATE tbl_DichVu
SET DonGia = 180000.00, MoTa = N'Dịch vụ tẩy tế bào chết toàn thân giúp làm sạch và làm mới da.'
WHERE MaDV = 'LD05';

UPDATE tbl_DichVu
SET DonGia = 400000.00, MoTa = N'Dịch vụ tắm trắng giúp làm sáng và đều màu da.'
WHERE MaDV = 'LD06';

UPDATE tbl_DichVu
SET DonGia = 450000.00, MoTa = N'Dịch vụ triệt lông body giúp loại bỏ lông không mong muốn trên toàn bộ cơ thể.'
WHERE MaDV = 'LD07';

UPDATE tbl_DichVu
SET DonGia = 200000.00, MoTa = N'Dịch vụ triệt lông vùng nách giúp loại bỏ lông không mong muốn ở vùng nách.'
WHERE MaDV = 'LD08';

UPDATE tbl_DichVu
SET DonGia = 300000.00, MoTa = N'Dịch vụ triệt lông tay, chân giúp loại bỏ lông không mong muốn ở tay và chân.'
WHERE MaDV = 'LD09';

UPDATE tbl_DichVu
SET DonGia = 50000.00, MoTa = N'Cà phê capuccino với hương vị đậm đà và lớp bọt sữa mịn màng.'
WHERE MaDV = 'TU01';

UPDATE tbl_DichVu
SET DonGia = 45000.00, MoTa = N'Cà phê espresso đậm đặc với hương vị mạnh mẽ và tinh tế.'
WHERE MaDV = 'TU02';

UPDATE tbl_DichVu
SET DonGia = 55000.00, MoTa = N'Cà phê trứng độc đáo với hương vị béo ngậy của trứng và vị đắng của cà phê.'
WHERE MaDV = 'TU03';

UPDATE tbl_DichVu
SET DonGia = 40000.00, MoTa = N'Nước ép cam tươi mát, giàu vitamin C, giúp tăng cường sức khỏe.'
WHERE MaDV = 'TU04';

UPDATE tbl_DichVu
SET DonGia = 35000.00, MoTa = N'Nước ép dưa hấu tươi ngon, giải khát, thích hợp cho mùa hè nóng bức.'
WHERE MaDV = 'TU05';

UPDATE tbl_DichVu
SET DonGia = 50000.00, MoTa = N'Sinh tố dâu ngọt ngào, giàu chất dinh dưỡng và vitamin.'
WHERE MaDV = 'TU06';

UPDATE tbl_DichVu
SET DonGia = 70000.00, MoTa = N'Bia Tiger Nâu với hương vị đậm đà và màu sắc đặc trưng.'
WHERE MaDV = 'TU07';

UPDATE tbl_DichVu
SET DonGia = 120000.00, MoTa = N'Rượu vang đỏ với hương vị phong phú và mùi thơm quyến rũ.'
WHERE MaDV = 'TU08';



------DỊCH VỤ ĐÃ ĐẶT-----------

CREATE TABLE tbl_DichVuDaDat(
	ID VARCHAR(10) PRIMARY KEY,
	NgaySuDungDV DATE,
	MaHD VARCHAR(10) FOREIGN KEY REFERENCES tbl_HoaDon(MaHD),
	MaTrangThaiDV char(4) FOREIGN KEY REFERENCES tbl_TrangThaiDichVu(MaTrangThaiDV),
	MaNV VARCHAR(10) FOREIGN KEY REFERENCES tbl_NhanVien(MaNV),
	MaKH VARCHAR(10) FOREIGN KEY REFERENCES tbl_KhachHang(MaKH),
	MaDV CHAR(4) FOREIGN KEY REFERENCES tbl_DichVu(MaDV),
)
ALTER table tbl_DichVuDaDat
ALTER COLUMN NgaySuDungDV DATETIME;
select * from tbl_DichVuDaDat
---CHI TIẾT PHÒNG--

CREATE TABLE tbl_ChiTietPhong(
	MaPhong CHAR(4) FOREIGN KEY REFERENCES tbl_Phong(MaPhong),
	MaLoaiPhong CHAR(4) FOREIGN KEY REFERENCES tbl_LoaiPhong(MaLoaiPhong),
	TienIch NVARCHAR(20),
	HinhAnh NVARCHAR(MAX),
	DienTich INT
)
--ALTER--
ALTER TABLE tbl_ChiTietPhong
DROP COLUMN DienTich

ALTER TABLE tbl_ChiTietPhong
ALTER COLUMN MaPhong CHAR(4) NOT NULL;

ALTER TABLE tbl_ChiTietPhong
ALTER COLUMN MaLoaiPhong CHAR(4) NOT NULL;

SELECT * FROM tbl_ChiTietPhong
--INSERT--
INSERT INTO tbl_ChiTietPhong VALUES
('P100','PDON',N'Wifi Miễn Phí',NULL),
('P100','PDON',N'Máy sấy tóc',NULL),
('P100','PDON',N'View thành phố',NULL),
('P100','PDON',N'Ổ điện gần giường',NULL),
('P100','PDON',N'Neflix',NULL)
INSERT INTO tbl_ChiTietPhong VALUES
('P101','PDON',N'Wifi Miễn Phí',NULL),
('P101','PDON',N'Máy sấy tóc',NULL),
('P101','PDON',N'View thành phố',NULL),
('P101','PDON',N'Ổ điện gần giường',NULL),
('P101','PDON',N'Neflix',NULL)
INSERT INTO tbl_ChiTietPhong VALUES
('P102','PDON',N'Wifi Miễn Phí',NULL),
('P102','PDON',N'Máy sấy tóc',NULL),
('P102','PDON',N'View thành phố',NULL),
('P102','PDON',N'Ổ điện gần giường',NULL),
('P102','PDON',N'Neflix',NULL)
INSERT INTO tbl_ChiTietPhong VALUES
('P103','PDOI',N'Wifi Miễn Phí',NULL),
('P103','PDOI',N'Máy sấy tóc',NULL),
('P103','PDOI',N'View thành phố',NULL),
('P103','PDOI',N'Ổ điện gần giường',NULL),
('P103','PDOI',N'Neflix',NULL)
INSERT INTO tbl_ChiTietPhong VALUES
('P200','PDOI',N'Wifi Miễn Phí',NULL),
('P200','PDOI',N'Máy sấy tóc',NULL),
('P200','PDOI',N'View thành phố',NULL),
('P200','PDOI',N'Ổ điện gần giường',NULL),
('P200','PDOI',N'Neflix',NULL)
INSERT INTO tbl_ChiTietPhong VALUES
('P201','PDOI',N'Wifi Miễn Phí',NULL),
('P201','PDOI',N'Máy sấy tóc',NULL),
('P201','PDOI',N'View thành phố',NULL),
('P201','PDOI',N'Ổ điện gần giường',NULL),
('P201','PDOI',N'Neflix',NULL)
INSERT INTO tbl_ChiTietPhong VALUES
('P202','PVIP',N'Wifi Miễn Phí',NULL),
('P202','PVIP',N'Máy sấy tóc',NULL),
('P202','PVIP',N'View thành phố',NULL),
('P202','PVIP',N'Ổ điện gần giường',NULL),
('P202','PVIP',N'Neflix',NULL)

----THỐNG KÊ----
CREATE TABLE tbl_ThongKe(
	MaThongKe INT PRIMARY KEY,
	TenThongKe NVARCHAR(50),
	Ngay DATE,
	TongThuNhap MONEY,
	MaNV VARCHAR(10) FOREIGN KEY REFERENCES tbl_NhanVien(MaNV)
)

--CHI TIẾT THỐNG KÊ--
CREATE TABLE tbl_ChiTietThongKe(
	MaChiTiet INT PRIMARY KEY,
	MaThongKe INT FOREIGN KEY REFERENCES tbl_ThongKe(MaThongKe),
	MaHD VARCHAR(10) FOREIGN KEY REFERENCES tbl_HoaDon(MaHD)
)

select * from tbl_PhieuThuePhong
select * from tbl_KhachHang
select * from tbl_DichVuDaDat
select * from tbl_HoaDon

select hd.MaHD
from tbl_HoaDon hd
join tbl_DichVuDaDat dv on hd.MaHD=dv.MaHD
where hd.MaKH=dv.MaKH
order by hd.MaHD desc

delete from tbl_PhieuThuePhong
delete from tbl_HoaDon
delete from tbl_DichVuDaDat

INSERT INTO tbl_ChucVu VALUES
('NVLT',N'Nhân Viên Lễ Tân'),
('NVS',N'Nhân Viên Spa'),
('NVVS',N'Nhân Viên Dọn Dẹp Phòng'),
('NVDR',N'Nhân Viên Đưa Rước'),
('NVNH',N'Nhân Viên Nhà Hàng'),
('NVCS',N'Nhân Viên Chăm Sóc Khách Hàng'),
('QLDV',N'Quản Lý Dịch Vụ'),
('QLKH',N'Quản Lý Chăm Sóc Khách Hàng'),
('NVKT',N'Nhân Viên Kế Toán'),
('TPKT',N'Kế Toán Trưởng'),
('TPNS',N'Trưởng Phòng Nhân Sự'),
('GDKS',N'Giám Đốc Khách Sạn')

select * from tbl_DichVu

INSERT INTO tbl_NhanVien VALUES
('TPSN',N'Gia Bảo','tpns','123','22DH110327@st.huflit.edu.vn','20040708','TPNS'),
('GDOC',N'Huy Lieu','admin','12345','20DH111441@st.huflit.edu.vn','20020316','GDKS'),
('QLDV',N'Gia Bảo','qldv','12345','22DH110327@st.huflit.edu.vn','20040708','QLDV'),
('QLKH',N'Gia Bảo','qlkh','123','22DH110327@st.huflit.edu.vn','20040708','QLKH')

