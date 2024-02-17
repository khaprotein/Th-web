﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestTemplate.Models;

namespace TestTemplate.Controllers
{
    public class DatSanController : Controller
    {
        // GET: DatSan
        QLDSEntities db = new QLDSEntities();
        private static int MaHD = 1; // Biến đếm mã hóa đơn
        private static int MaCTHD = 1; // Biến đếm mã CTHD

        public ActionResult Index(string id_coso, string ten_coso)
        {
            Session["ten_coso"] = ten_coso;
            Session["id_coso"] = id_coso;
            return View();
        }


        [HttpPost]
        public ActionResult Index(DatSan model)
        {

            // kiểm tra dữ liệu
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // kết hợp biến ngày đặt vs giờ đá
            string batdau = model.ngayDatSan + " " + model.gioBatDau;
            string ketthuc = model.ngayDatSan + " " + model.gioKetThuc;

            DateTime gio_da = new DateTime();
            DateTime gio_nghi = new DateTime();
            DateTime.TryParse(batdau, out gio_da);
            DateTime.TryParse(ketthuc, out gio_nghi);

            // kiểm tra tg hợp lệ
            if (!kiemtra_TG_Trandau(gio_da, gio_nghi))
            {
                ModelState.AddModelError("", "Thời gian bạn chọn không hợp lệ, vui lòng chọn lại\n  Thời gian mở cửa từ 7 giờ sáng đến 23 giờ cùng ngày.\nTổng thời gian trận đấu phải kéo dài từ 30 phút trở lên và không được đặt quá 1 tháng trước thời gian hiện tại.");
                return View(model);
            }

            string maSanTrong = TimMaSanTrong(gio_da, gio_nghi, model.ma_danhmuc);
            if (maSanTrong == null)
            {
                ModelState.AddModelError("", "Hiện cơ sở đã hết sân trống trong khung giờ bạn chọn Vui lòng chọn khung giờ khác");

                return View(model);
            }
            var danhMucSan = db.DanhMucSans.SingleOrDefault(dm => dm.MaDanhMuc == model.ma_danhmuc);
            if (danhMucSan != null)
            {
                TempData["LoaiSan"] = danhMucSan.LoaiSan;
            }

            var khachhang = Session["user"] as user_KhachHang;

            model.ma_KH = khachhang.MaKH;
            model.ma_San = maSanTrong;

            // Lưu model vào TempData để sử dụng sau này
            TempData["ThongTinDatSan"] = model;

            return RedirectToAction("XacNhanDatSan", model);
        }

        public ActionResult XacNhanDatSan(DatSan model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult XacNhanDatSan()
        {

            var model = TempData["ThongTinDatSan"] as DatSan;
            if (model == null)
            {
                // Xử lý lỗi nếu không có thông tin đặt sân
                return RedirectToAction("Index");
            }
            int soLuongKhachHangCungSDT = db.user_KhachHang.Count(kh => kh.MaKH == model.ma_KH);

            if (soLuongKhachHangCungSDT >= 6)
            {
                ModelState.AddModelError("", "Mỗi tài khoản chỉ được đặt 6 lần. Vui long chọn 1 tài khoản khác để tiếp tục!");
                return View(model);
            }

            // kết hợp biến ngày đặt vs giờ đá
            string batdau = model.ngayDatSan + " " + model.gioBatDau;
            string ketthuc = model.ngayDatSan + " " + model.gioKetThuc;

            DateTime gio_da = new DateTime();
            DateTime gio_nghi = new DateTime();
            DateTime.TryParse(batdau, out gio_da);
            DateTime.TryParse(ketthuc, out gio_nghi);

            // cập nhật lại nếu như có lịch đặt bị trùng nhưng trạng thái là "Đã huỷ"
            var lichdattrung = db.LichDats.Where(c => c.MaSan == model.ma_San && c.TrangThai == "Đã huỷ" && KiemTraTrungLich(c, gio_da, gio_nghi)) as LichDat;
            if (lichdattrung != null)
            {
                lichdattrung.TrangThai = "Chưa diễn ra";
                lichdattrung.MaKhachHang = model.ma_KH;
                db.SaveChanges();
                TempData["ThongBaoDatSan"] = "Đặt sân thành công!";
            }
            // nếu không có lịch đặt trùng nào thì tạo lịch đặt mới 
            else
            {
                string newMaSan = TimMaSanMoi();

                LichDat ld_ms = new LichDat
                {
                    MaLichDat = newMaSan,
                    MaKhachHang = model.ma_KH,
                    MaSan = model.ma_San,
                    ThoiGianBatDau = gio_da,
                    ThoiGianKetThuc = gio_nghi,
                    TrangThai = "Chưa diễn ra"
                };

                db.LichDats.Add(ld_ms);
                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
                HoaDon();
                TempData["ThongBaoDatSan"] = "Đặt sân thành công!";
            }
            // Chuyển hướng sau khi lưu vào cơ sở dữ liệu
            return RedirectToAction("Index", "Home");
        }

        // Phương thức để tạo mã sân mới
        private string TimMaSanMoi()
        {
            string maxMaSan = db.LichDats.Max(ld => ld.MaLichDat);

            if (string.IsNullOrEmpty(maxMaSan))
            {
                // Nếu không có mã sân nào hoặc mã sân không hợp lệ, gán "001" cho mã sân đầu tiên
                return "001";
            }
            else
            {
                // Tạo mã sân mới bằng cách tăng thêm 1 số
                int currentMaSanNumber = int.Parse(maxMaSan);
                currentMaSanNumber++;
                return currentMaSanNumber.ToString("D3");
            }
        }


        private string TimMaSanTrong(DateTime gioBatDau, DateTime gioKetThuc, string ma_danhmuc)
        {
            // Lấy danh sách tất cả các sân trong mã danh mục
            var sans = db.Sans.Where(s => s.MaDanhMuc == ma_danhmuc).ToList();

            string maSanTrong = null; // Mã sân mặc định là null
            bool coSanTrong = false; // Biến đánh dấu xem có sân nào thỏa mãn không

            foreach (var san in sans)
            {
                bool sanTrong = true;

                // Lấy tất cả lịch đặt của sân này
                var lichDats = db.LichDats.Where(lich => lich.MaSan == san.MaSan).ToList();

                // Kiểm tra xem có bất kỳ trùng lịch nào không
                foreach (var lich in lichDats)
                {
                    if (KiemTraTrungLich(lich, gioBatDau, gioKetThuc))
                    {
                        if (lich.TrangThai == "Đã huỷ")
                        {
                            continue;
                        }
                        else
                        {
                            sanTrong = false;
                            break;
                        }
                    }
                }
                if (sanTrong)
                {
                    maSanTrong = san.MaSan; // Lưu lại mã sân trống
                    coSanTrong = true; // Đánh dấu có sân thỏa mãn
                    break; // Thoát khỏi vòng lặp khi tìm thấy sân trống
                }
            }
            if (coSanTrong)
            {
                return maSanTrong; // Trả về mã sân trống
            }

            return null; // Không có sân nào trống
        }

        // Phương thức kiểm tra trùng lịch
        private bool KiemTraTrungLich(LichDat lich, DateTime gioBatDau, DateTime gioKetThuc)
        {
            // Kiểm tra xem thời gian bắt đầu và kết thúc của lịch đã nhập có trùng với lịch trong cơ sở dữ liệu không
            if ((gioBatDau >= lich.ThoiGianBatDau && gioBatDau < lich.ThoiGianKetThuc) ||
                (gioKetThuc > lich.ThoiGianBatDau && gioKetThuc <= lich.ThoiGianKetThuc) ||
                (gioBatDau <= lich.ThoiGianBatDau && gioKetThuc >= lich.ThoiGianKetThuc))
            {
                return true; // Có trùng lịch

            }
            return false; // Không trùng lịch
        }

        public bool kiemtra_TG_Trandau(DateTime tg_batdau, DateTime tg_ketthuc)
        {
            TimeSpan duration = tg_ketthuc - tg_batdau;

            if (tg_ketthuc <= DateTime.Now || tg_batdau.Hour < 7 || tg_ketthuc.Hour > 23 || tg_batdau > DateTime.Now.AddMonths(1))
            {
                return false; // Thời gian kết thúc không hợp lệ hoặc nằm ngoài giờ mở cửa hoặc cách thời gian hiện tại quá 1 tháng.
            }

            if (tg_batdau <= DateTime.Now.AddMinutes(30) || tg_batdau >= tg_ketthuc || duration.TotalHours < 0.5 || duration.TotalHours >= 5)
            {
                return false; // Thời gian đặt sân không hợp lệ.
            }

            return true; // Thời gian hợp lệ.
        }

        public void HoaDon()
        {
            // lấy mã lịch đặt mới nhất
            var maLDMoiNhat = db.LichDats
                .OrderByDescending(ld => ld.MaLichDat)
                .Select(ld => ld.MaLichDat)
                .FirstOrDefault().ToString();

            // lấy mã khách hàng
            var maKHDatSan = db.LichDats
                .OrderByDescending(ld => ld.MaLichDat)
                .Select(ld => ld.MaKhachHang)
                .FirstOrDefault().ToString();

            // lấy mã nhân viên
            var maNV =  (from ld in db.LichDats
                        join s in db.Sans on ld.MaSan equals s.MaSan
                        join dm in db.DanhMucSans on s.MaDanhMuc equals dm.MaDanhMuc
                        join cs in db.CoSoes on dm.MaCS equals cs.MaCS
                        join pc in db.PhanCongs on cs.MaCS equals pc.MaCS
                        select pc.MaNV)
                        .OrderByDescending(nv => nv)
                        .FirstOrDefault()
                        .ToString();

            string TrangThai = "Chưa thanh toán";

            var lastInvoice = db.HoaDons.OrderByDescending(hd => hd.MaHoaDon).FirstOrDefault();
            if (lastInvoice != null)
            {
                // Nếu có hóa đơn trong cơ sở dữ liệu, lấy giá trị của MaHoaDon lớn nhất và tăng lên 1.
                MaHD = int.Parse(lastInvoice.MaHoaDon) + 1;
            }
            // Tạo mã hóa đơn dưới dạng "001", "002", ...
            string maHD = $"{MaHD:D3}";
            MaHD++;

            // Thêm hóa đơn vào cơ sở dữ liệu
            HoaDon hoaDon = new HoaDon()
            {
                MaHoaDon = maHD,
                MaLichDat = maLDMoiNhat,
                MaKhachHang = maKHDatSan,
                MaNV = maNV,
                NgayTao = DateTime.Now,
                TrangThai = TrangThai
            };
            db.HoaDons.Add(hoaDon);
            db.SaveChanges();

            // Lấy dữ liệu để thêm vào CTHD

            // Mã CTHD
            var lastInvoiceDetails = db.CTHDs.OrderByDescending(cthd => cthd.MaCTHD).FirstOrDefault();
            if (lastInvoiceDetails != null)
            {
                // Nếu có hóa đơn trong cơ sở dữ liệu, lấy giá trị của MaHoaDon lớn nhất và tăng lên 1.
                MaCTHD = int.Parse(lastInvoiceDetails.MaCTHD) + 1;
            }
            // Tạo mã hóa đơn dưới dạng "001", "002", ...
            string maCTHD = $"{MaCTHD:D3}";
            MaCTHD++;

            // NgayDat
            var ngayDat = db.LichDats
                            .OrderByDescending(ld => ld.MaLichDat)
                            .Select(ld => ld.ThoiGianBatDau)
                            .FirstOrDefault();

            // Lấy ra loại sân, số sân, giá sân
            var loai_so_San = db.LichDats.Where(ld => ld.MaLichDat == maLDMoiNhat)
                                      .Join(db.Sans,
                                            ld => ld.MaSan,
                                            s => s.MaSan,
                                            (ld, s) => new { LichDat = ld, San = s })
                                      .Join(db.DanhMucSans,
                                             lds => lds.San.MaDanhMuc,
                                             dm => dm.MaDanhMuc,
                                             (lds, dm) => new { lds.LichDat, lds.San, DanhMucSan = dm })
                                      .OrderByDescending(res => res.LichDat.MaLichDat)
                                      .FirstOrDefault();

            ViewBag.LoaiSan = loai_so_San.DanhMucSan.LoaiSan;
            string soSan = loai_so_San.San.SoSan.ToString();
            double giaTien = (double)loai_so_San.San.GiaSan;

            //string soSan = ViewBag.SoSan.ToString();
            //string giaSan = ViewBag.GiaSan.ToString();

            // Lấy ra số giờ đặt
            var tgKetThuc = db.LichDats
                            .OrderByDescending(ld => ld.MaLichDat)
                            .Select(row => row.ThoiGianKetThuc)
                            .FirstOrDefault();

            TimeSpan thGianDatSan = (TimeSpan)(tgKetThuc - ngayDat);
            double soGioDat = thGianDatSan.TotalHours;

            // Thêm vào CTHD
            CTHD cTHD = new CTHD();

            cTHD.MaCTHD = maCTHD;
            cTHD.MaHoaDon = hoaDon.MaHoaDon;
            cTHD.NgayDat = ngayDat;
            cTHD.LoaiSan = ViewBag.LoaiSan;

            int SoSan;
            if (int.TryParse(soSan, out SoSan))
            {
                cTHD.SoSan = SoSan;
            }

            cTHD.SoGioDat = soGioDat;

            cTHD.GiaTien = giaTien;

            db.CTHDs.Add(cTHD);

            db.SaveChanges();

        }
    }
}