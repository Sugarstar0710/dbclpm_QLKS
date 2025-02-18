using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebQLKS.Controllers;
using WebQLKS.Models;
//using System.Web.Routing;



namespace WebQLKSTests.Controllers
{
    [TestFixture] // Mark this as a Test Class
    public class ServiceControllerTests : IDisposable
    {
        private Mock<DAQLKSEntities> _mockDbContext;
        private ServiceController _controller;

        [SetUp] // Runs before each test
        public void Setup()
        {
            _mockDbContext = new Mock<DAQLKSEntities>();

            // Initialize Controller with Mock Database
            _controller = new ServiceController
            {
                ControllerContext = new ControllerContext()
            };
        }

        [TearDown] // Runs after each test
        public void TearDown()
        {
            _mockDbContext?.VerifyAll(); // Optionally verify mock interactions
        }

        // Implement IDisposable interface to avoid NUnit1032 warning
        public void Dispose()
        {
            _mockDbContext?.Object?.Dispose(); // Dispose of any resources if necessary
        }

        [Test]
        public void Index_Returns_ViewResult_With_ListOfServices()
        {
            // Arrange
            var services = new List<tbl_LoaiDichVu>
            {
                new tbl_LoaiDichVu { MaLoaiDV = "DV1", TenLoaiDV = "Service 1" },
                new tbl_LoaiDichVu { MaLoaiDV = "DV2", TenLoaiDV = "Service 2" }
            };

            _mockDbContext.Setup(db => db.tbl_LoaiDichVu.ToList()).Returns(services);

            // Act
            var result = _controller.Index() as ViewResult;
            var model = result?.Model as List<tbl_LoaiDichVu>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<tbl_LoaiDichVu>>(model);
            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public void chiTietLoaiDV_Returns_ViewResult_With_Services_For_Specific_Type()
        {
            // Arrange
            var services = new List<tbl_DichVu>
            {
                new tbl_DichVu { MaLoaiDV = "DV1", MaDV = "D001", TenDV = "Service 1" },
                new tbl_DichVu { MaLoaiDV = "DV1", MaDV = "D002", TenDV = "Service 2" }
            };

            _mockDbContext.Setup(db => db.tbl_DichVu.Where(d => d.MaLoaiDV == "DV1").ToList()).Returns(services);

            // Act
            var result = _controller.chiTietLoaiDV("DV1") as ViewResult;
            var model = result?.Model as List<tbl_DichVu>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<tbl_DichVu>>(model);
            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public void detailService_Returns_ViewResult_With_Service_Detail()
        {
            // Arrange
            var service = new tbl_DichVu { MaDV = "D001", TenDV = "Service 1", DonGia = 100 };

            _mockDbContext.Setup(db => db.tbl_DichVu.FirstOrDefault(d => d.MaDV == "D001")).Returns(service);

            // Act
            var result = _controller.detailService("D001") as ViewResult;
            var model = result?.Model as tbl_DichVu;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<tbl_DichVu>(model);
            Assert.AreEqual("D001", model?.MaDV);
        }

        //[Test]
        //public void orderService_ThrowsError_When_No_CheckIn()
        //{
        //    // Arrange
        //    var maDV = "D001";
        //    var maKH = "KH001";
        //    var systemTime = DateTime.Now;
        //    var vietNamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        //    var vietNamTime = TimeZoneInfo.ConvertTime(systemTime, vietNamTimeZone);

        //    _controller.Session["KH"] = maKH; // Simulate customer login
        //    _mockDbContext.Setup(db => db.tbl_PhieuThuePhong
        //        .Where(i => i.MaKH == maKH && i.NgayBatDauThue <= vietNamTime && vietNamTime <= i.NgayKetThucThue && (i.TrangThai == "Đã nhận phòng"))
        //        .OrderByDescending(i => i.MaPhieuThuePhong)
        //        .FirstOrDefault()).Returns((tbl_PhieuThuePhong)null); // No check-in

        //    // Act
        //    var result = _controller.orderService(maDV) as RedirectToRouteResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("ErrorService", result?.RouteValues["TempData"]);
        //}

        //[Test]
        //public void orderService_Success_When_CheckInExists()
        //{
        //    // Arrange
        //    var maDV = "D001";
        //    var maKH = "KH001";
        //    var systemTime = DateTime.Now;
        //    var vietNamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        //    var vietNamTime = TimeZoneInfo.ConvertTime(systemTime, vietNamTimeZone);

        //    _controller.Session["KH"] = maKH; // Simulate customer login
        //    _mockDbContext.Setup(db => db.tbl_PhieuThuePhong
        //        .Where(i => i.MaKH == maKH && i.NgayBatDauThue <= vietNamTime && vietNamTime <= i.NgayKetThucThue && (i.TrangThai == "Đã nhận phòng"))
        //        .OrderByDescending(i => i.MaPhieuThuePhong)
        //        .FirstOrDefault()).Returns(new tbl_PhieuThuePhong { MaKH = maKH }); // Simulate valid check-in

        //    _mockDbContext.Setup(db => db.tbl_DichVu.FirstOrDefault(dv => dv.MaDV == maDV)).Returns(new tbl_DichVu { MaDV = maDV, DonGia = 100 });

        //    // Act
        //    var result = _controller.orderService(maDV) as RedirectToRouteResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("SuccessServiceMessage", result?.RouteValues["TempData"]);
        //}
    }
}
