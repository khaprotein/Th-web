using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestTemplate.Models;

namespace TestTemplate.Controllers
{
    public class TinTucController : Controller
    {
        // GET: TinTuc
        QLDSEntities db = new QLDSEntities();
      
        public ActionResult New_1()
        {
            Tintuc tt1 = new Tintuc
            {
                TieuDe = "HLV Polking mất việc vì Thái Lan thua Trung Quốc",
                NgayDang = DateTime.Now,

                MoTa = "",
                NoiDung1 ="<p>Theo trưởng đoàn Nualphan Lamsam, HLV Mano Polking từng được tin tưởng, nhưng thất bại trước Trung Quốc khiến bà và LĐBĐ Thái Lan (FAT) phải thay đổi, vì mục tiêu đưa Thái Lan dự World Cup.</p>" +
                "<p>HLV Polking đáng lẽ bị sa thải từ tháng 9 khi Thái Lan vuột King’s Cup vào tay Iraq. Lúc đó, HLV Masatada Ishii đã sẵn sàng thay thế, nhưng cuối cùng, bà Nualphan và các cầu thủ Thái Lan vẫn tin tưởng HLV Polking. Tuy nhiên, trận thua ngược Trung Quốc 1-2 ở vòng loại hai World Cup 2026 khiến quan điểm của đội tuyển cũng như FAT với ông Polking thay đổi.</p>" +
                "<p>Trận thua Trung Quốc ảnh hưởng rất lớn đến cơ hội đi tiếp vào vòng loại ba, bà Nualphan viết trên trang Facebook cá nhân hôm qua 22/11. Tôi nghĩ đã đến lúc phải thay đổi, để Thái Lan chuẩn bị tốt hơn khi gặp các đội nhóm đầu châu Á.</p>" 
              
                ,NoiDung2 = "<p>Bà Nualphan đã báo cáo với Chủ tịch FAT Somyot Poompanmoung và nói chuyện riêng với HLV Mano Polking. Vì vậy, HLV người Brazil biết sẽ mất việc từ trước khi cùng Thái Lan thắng Singapore 3-1 ở lượt trận thứ hai ngày 21/11.</p>" +
                "<p>Thái Lan hiện đứng nhì bảng C với ba điểm, bằng Trung Quốc nhưng hơn hiệu số bàn thắng bại (+1 so với -2), Hàn Quốc dẫn đầu với sáu điểm và Singapore chưa giành điểm xếp cuối. Thái Lan còn hai trận gặp Hàn Quốc, một trận gặp Trung Quốc trên sân khách và Singapore trên sân nhà.</p>" +
                "<p>Bà Nualphan bác bỏ những đồn đoán rằng bà lạm quyền, khi khẳng định không bao giờ can thiệp vào công việc của HLV trưởng. Trong vai trò trưởng đoàn của đội tuyển, bà chỉ cố gắng cung cấp những cái tên tốt nhất trong danh sách HLV mong muốn, bên cạnh đó, tạo điều kiện thuận lợi về sinh hoạt, trang thiết bị cho đội.</p>" +
                 "<p> </p>"
                 ,
                NoiDung3 = "<p>Về việc bổ nhiệm ông Masatada Ishii, Madam Pang nhấn mạnh HLV người Nhật Bản đã chứng minh được năng lực tại J-League 1 và Thai League 1 với bốn danh hiệu cùng Kashima Antlers và sáu danh hiệu cùng Buriram United. Bên cạnh đó, nền bóng đá Nhật Bản luôn được đánh giá cao ở Thái Lan, dù Akira Nishino - HLV người Nhật đầu tiên - chưa thành công với ĐTQG và U23 Thái Lan.</p>" +
                "<p>Ishii sẵn sàng làm việc ngay lập tức, Pang cho hay. Ông ấy còn hiểu biết, quen thuộc và có nhiều thông tin nhất về các tuyển thủ Thái Lan.</p>" +
                "<p>Tờ Siam Sport của Thái Lan cảnh báo HLV Masatada Ishii đối mặt nhiều khó khăn khi nhậm chức lúc này. Trận đấu đầu tiên của nhà cầm quân 56 tuổi là giao hữu gặp Nhật Bản vào ngày 1/1/2024, sau đó là phải vượt qua vòng bảng Asian Cup 2023 và vòng loại hai World Cup 2026.</p>"
               + "<p> </p>"

            };
            return View(tt1);
        }
        public ActionResult New_2()
        {
            Tintuc tt2 = new Tintuc
            {
                TieuDe = "Báo Đông Nam Á than phiền kết quả chia bảng U23 Asian Cup",
                NgayDang = DateTime.Now,

                MoTa = "Truyền thông Thái Lan, Indonesia và Malaysia cùng cho rằng đội nhà rơi vào bảng khó khăn ở VCK U23 châu Á 2024.",

                NoiDung1 = "<p>Truyền thông Indonesia thất vọng hơn cả với kết quả bốc thăm, khi báo CNN nhận định \"đoàn quân của Shin Tae-yong sẽ đối mặt với những bài kiểm tra khó khăn. Tuy nhiên, Thái Lan cũng gặp những đối thủ đáng gườm không kém tại bảng C\", tờ báo viết thêm.</p>"
                + "<p>Tờ Bolasport bình luận rằng Indonesia đã rơi vào bảng \"địa ngục\", cùng chủ nhà Qatar, Australia và Jordan. Họ khuyên HLV Shin cần cảnh giác cao độ vì ba đối thủ này đều đạt thành tích tốt ở vòng loại. Qatar và Australia có nền bóng đá hàng đầu châu lục, còn đội tuyển Jordan cũng đang đứng thứ 82 FIFA.</p>" 

                ,NoiDung2 = "<p>Đây là lần đầu Indonesia dự VCK U23 châu Á, và họ cũng là tân binh duy nhất trong 16 đội dự giải năm tới. Trang Bola thậm chí giật tít: \"Chia buồn Indonesia.Chia buồn các đội Đông Nam Á\".</p>"
                + "<p>Thái Lan cũng rơi vào bảng khó khăn với Arab Saudi, Iraq và Tajikistan. Hai đối thủ đầu tiên đều từng vô địch giải đấu, còn Tajikistan dự kiến là đối thủ vừa tầm với thầy trò Issara Sritaro. Tờ Thairath nhận định bảng đấu của Thái Lan \"quá nặng\", Sanook cho rằng bảng C \"quá nóng bỏng\", còn Khaosod dự đoán Thái Lan sẽ phải làm nhiệm vụ \"gian khó\".</p>"
                + "<p>Bảng D là bảng duy nhất có hai đại diện Đông Nam Á, là Việt Nam và Malaysia, bên cạnh Uzbekistan với Kuwait. Uzbekistan được coi là mạnh nhất bảng khi từng vô địch năm 2018, và suất còn lại nhiều khả năng do ba đội còn lại cạnh tranh. Theo thứ bậc FIFA, Việt Nam đang đứng vị trí 94, Kuwait 136 còn Malaysia 137. Vì thế, báo Malaysia Bernama cho rằng đội nhà đã ở bảng \"khó khăn\". Còn tờ Sinar Harian nhận định thầy trò Juan Torres Garrido đã rơi vào \"bãi mìn\".</p>"
                + "<p>VCK U23 châu Á diễn ra hai năm một lần, và được coi là vòng loại cuối của Olympic trong những năm trùng Thế vận hội. Giải U23 châu Á 2024 diễn ra từ 15/4 đến 3/5 tại Qatar, với 16 đội chia làm bốn bảng, mỗi bảng bốn đội đá vòng tròn một lượt. Hai đội đầu bảng sẽ vào tứ kết. Ba đội đứng đầu giải sẽ dự Olympic Paris 2024, còn đội đứng thứ tư sẽ đá trận vớt với đại diện châu Phi Guinea để tranh suất tới Olympic.</p>"

                ,NoiDung3="<p> </p>"

            };
            return View(tt2);
        }
    }
}