# BÁO CÁO TUẦN 2 – XÂY DỰNG RESTFUL API

## 1. Mục tiêu
- Tìm hiểu kiến trúc RESTful API.
- Hiểu cách giao tiếp giữa máy khách và máy chủ.
- Sử dụng các phương thức HTTP GET và POST.
- Xử lý các mã trạng thái HTTP.

## 2. Công việc đã thực hiện
- Tạo BookDto để biểu diễn thông tin sách.
- Tạo CreateBookRequest để nhận dữ liệu thêm sách.
- Xây dựng API lấy danh sách sách.
- Xây dựng API xem chi tiết sách.
- Xây dựng API thêm sách mới.
- Tạo chương trình Library.Client sử dụng HttpClient.
- Kiểm thử quá trình giao tiếp giữa Client và API.

## 3. Các API đã xây dựng

| Phương thức | Endpoint | Chức năng |
|---|---|---|
| GET | /api/books | Lấy danh sách sách |
| GET | /api/books/{id} | Xem chi tiết sách |
| POST | /api/books | Thêm sách mới |

## 4. Kết quả kiểm thử
- HTTP 200: Lấy dữ liệu thành công.
- HTTP 201: Thêm sách thành công.
- HTTP 400: Dữ liệu không hợp lệ.
- HTTP 404: Không tìm thấy sách.

## 5. Kết luận
Đã xây dựng được RESTful API cơ bản cho hệ thống thư viện. Chương trình Client có thể gửi yêu cầu HTTP đến Server và nhận dữ liệu phản hồi. Dữ liệu sách hiện được lưu trong bộ nhớ để phục vụ kiểm thử.